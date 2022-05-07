IF object_id('INSPECCION.Usp_ListarInspeccionAndamio') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ListarInspeccionAndamio]
GO

CREATE PROCEDURE [INSPECCION].[Usp_ListarInspeccionAndamio]      
	@nombreProyecto VARCHAR(120) = NULL,      
	@fechaInicio DATETIME = NULL,      
	@fechaFin DATETIME = NULL,      
	@offset INT = NULL,      
	@fetch INT = NULL,      
	@verHistorial BIT = 0,
	@modoHistorico BIT = 0,
	@cantidadAnios INT = 0
AS      
BEGIN      
	SELECT IA.InspeccionAndamio_id AS InspeccionAndamio_id,    
		IA.EstadoInspeccionAndamio_id AS EstadoInspeccionAndamio_id,    
		IA.ZonaTrabajo,    
		IA.MarcaAndamio,       
		IA.SobreCargaUso,    
		IA.Responsable,    
		IA.Observacion,    
		IA.Cliente_id,     
		IA.FechaCreacion AS FechaCreacion,    
		IA.Proyecto,    
		RTRIM(LTRIM(CL.NombreCompleto)) AS NombreCliente,    
		(    
			SELECT STRING_AGG(Nombres, ', ')    
			FROM (    
					SELECT DISTINCT Nombres    
					FROM (    
							SELECT CASE    
									WHEN GP.Descripcion IS NULL THEN TA.nombre    
									ELSE CONCAT(TA.nombre, ' - ', GP.Descripcion)    
								END AS Nombres    
							FROM COMERCIAL.TipoAndamio TA    
								INNER JOIN INSPECCION.CheckList C ON C.TipoAndamio_id = TA.tipo_sol_id    
								LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id = 'ST'    
								AND GP.ParametroPadre_id = TA.tipo_sol_id    
								AND GP.GrupoParametros_id = C.SubTipoAndamio_id    
							WHERE C.Activo = 1    
								AND C.InspeccionAndamio_id = IA.InspeccionAndamio_id    
						) AS A1    
				) AS A2    
		) AS TipoAndamio_Nombres,    
		EIA.Nombre AS Estado,    
		IA.IdUsuarioCreacion AS IdUsuarioCreacion,    
		UR.NombreCompleto AS NombreUsuario    
	FROM INSPECCION.InspeccionAndamio IA    
		INNER JOIN INSPECCION.EstadoInspeccionAndamio EIA ON EIA.EstadoInspeccionAndamio_id = IA.EstadoInspeccionAndamio_id    
		INNER JOIN GL_MaestroPersonas UR ON UR.Persona = IA.IdUsuarioCreacion    
		INNER JOIN GL_MaestroPersonas CL ON CL.Persona = IA.Cliente_id  
	WHERE IA.Activo=1      
		AND (@nombreProyecto IS NULL OR IA.Proyecto LIKE '%' + @nombreProyecto + '%') 
		AND (@fechaInicio IS NULL OR CONVERT(DATE,IA.FechaCreacion) >= @fechaInicio)      
		AND (@fechaFin IS NULL OR CONVERT(DATE,IA.FechaCreacion) <= @fechaFin)      
		AND (@verHistorial = 0 OR IA.EstadoInspeccionAndamio_id = 3) -- 3: Historico      
		AND (@verHistorial != 0 OR IA.EstadoInspeccionAndamio_id IN(1, 2)) -- 1: Borrador , 2: Completado      
		AND (@modoHistorico = 1 OR IA.FechaCreacion > DATEADD(month,-(@cantidadAnios*12),GETDATE()))
		AND (@modoHistorico = 0 OR IA.FechaCreacion < DATEADD(month,-(@cantidadAnios*12),GETDATE()))
	ORDER BY IA.FechaCreacion DESC       
		OFFSET @OffSet ROWS FETCH NEXT @Fetch ROWS ONLY      
END
GO

IF object_id('INSPECCION.Usp_ListarPreguntaInspeccionAndamio') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ListarPreguntaInspeccionAndamio]
GO

CREATE PROCEDURE [INSPECCION].[Usp_ListarPreguntaInspeccionAndamio]
	@tipoAndamio_id INT = NULL,
	@subTipoAndamio_id INT = NULL
AS
BEGIN
	SELECT PIA.PreguntaInspeccionAndamio_id,
		PIA.PreguntaInspeccionAndamio,
		PIA.TipoAndamio_id,
		PIA.SubTipoAndamio_id,
		PIA.Orden,
		PIA.Activo    
	FROM ASESORIA.PreguntaInspeccionAndamio PIA     
	WHERE PIA.Activo = 1
		AND PIA.TipoAndamio_id=@tipoAndamio_id AND PIA.Activo=1    
		AND ((@subTipoAndamio_id IS NULL AND PIA.SubTipoAndamio_id IS NULL)    
			OR (@subTipoAndamio_id IS NOT NULL AND PIA.SubTipoAndamio_id=@subTipoAndamio_id))    
	ORDER BY PIA.Orden;    
END
GO

IF object_id('INSPECCION.Usp_ListarPreguntaInspeccionAndamioSubTipoAndamio') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ListarPreguntaInspeccionAndamioSubTipoAndamio]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_ListarPreguntaInspeccionAndamioSubTipoAndamio]
AS
BEGIN
	SELECT PIA.TipoAndamio_id AS TipoAndamio_id,  
		PIA.SubTipoAndamio_id AS SubTipoAndamio_id,  
		GP.Descripcion AS SubTipoAndamio_Nombre  
	FROM ASESORIA.PreguntaInspeccionAndamio PIA   
	INNER JOIN  Asesoria.GrupoParametros AS GP ON GP.ParametroPadre_id=pia.TipoAndamio_id AND GP.Grupo_id='ST' AND GP.GrupoParametros_id=PIA.SubTipoAndamio_id
	WHERE PIA.Activo = 1
		AND GP.Activo = 1		
	GROUP BY PIA.TipoAndamio_id,PIA.SubTipoAndamio_id,GP.Descripcion   
END
GO


IF object_id('INSPECCION.Usp_ListarTipoAndamio') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ListarTipoAndamio]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_ListarTipoAndamio]
AS
BEGIN
	SELECT tipo_sol_id as TipoAndamio_id,
		nombre AS Nombre
	FROM COMERCIAL.TipoAndamio
	WHERE Activo = 1
END
GO

IF object_id('INSPECCION.Usp_CRUD_InspeccionAndamio') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_CRUD_InspeccionAndamio]
GO

CREATE PROCEDURE [INSPECCION].[Usp_CRUD_InspeccionAndamio]        
 @strMode CHAR(2) = NULL,         
 @InspeccionAndamio_id INT = NULL,        
 @EstadoInspeccionAndamio_id INT = NULL,        
 @Cliente_id INT = NULL,        
 @Proyecto VARCHAR(512) = NULL,        
 @Direccion VARCHAR(400) = NULL,        
 @ZonaTrabajo VARCHAR(400) = NULL,        
 @Responsable VARCHAR(400) = NULL,        
 @Cargo VARCHAR(400) = NULL,        
 @MarcaAndamio VARCHAR(400) = NULL,                  
 @SobreCargaUso VARCHAR(50) = NULL,        
 @Observacion VARCHAR(400) = NULL,        
 @IdUsuarioCreacion INT = NULL,        
 @IdUsuarioEdicion INT = NULL,        
 @FechaCreacion DATETIME = NULL,        
 @FechaEdicion DATETIME = NULL,        
 @Activo BIT = NULL        
AS        
BEGIN        
IF (@strMode='C1')        
BEGIN        
 INSERT INTO INSPECCION.InspeccionAndamio(        
  EstadoInspeccionAndamio_id,        
  Cliente_id,        
  Proyecto,        
  Direccion,        
  ZonaTrabajo,        
  Responsable,        
  Cargo,        
  MarcaAndamio,        
  IdUsuarioCreacion,        
  FechaCreacion,        
  Activo)        
 OUTPUT INSERTED.InspeccionAndamio_id        
 VALUES(        
  @EstadoInspeccionAndamio_id,        
  @Cliente_id,        
  @Proyecto,        
  @Direccion,        
  @ZonaTrabajo,        
  @Responsable,        
  @Cargo,        
  @MarcaAndamio,        
  @IdUsuarioCreacion,        
  @FechaCreacion,        
  @Activo);        
END        
IF (@strMode='C2')        
BEGIN        
 INSERT INTO INSPECCION.InspeccionAndamio(        
  EstadoInspeccionAndamio_id,        
  Cliente_id,        
  Proyecto,        
  Direccion,        
  ZonaTrabajo,        
  Responsable,        
  Cargo,        
  MarcaAndamio,               
  SobreCargaUso,        
  Observacion,        
  IdUsuarioCreacion,        
  FechaCreacion,        
  Activo)        
 OUTPUT INSERTED.InspeccionAndamio_id        
 VALUES(        
  @EstadoInspeccionAndamio_id,        
  @Cliente_id,        
  @Proyecto,        
  @Direccion,        
  @ZonaTrabajo,        
  @Responsable,        
  @Cargo,        
  @MarcaAndamio,              
  @SobreCargaUso,        
  @Observacion,        
  @IdUsuarioCreacion,        
  @FechaCreacion,        
  @Activo);        
END        
ELSE IF (@strMode='R')        
BEGIN        
 --SELECT * FROM INSPECCION.InspeccionAndamio      
 SELECT IA.*,EIA.Nombre as Estado, CL.NombreCompleto AS NombreCliente      
 FROM INSPECCION.InspeccionAndamio IA        
 INNER JOIN INSPECCION.EstadoInspeccionAndamio EIA ON EIA.EstadoInspeccionAndamio_id=IA.EstadoInspeccionAndamio_id        
 INNER JOIN GL_MaestroPersonas CL ON CL.Persona = IA.Cliente_id      
 WHERE IA.Activo = 1        
  AND IA.InspeccionAndamio_id=@InspeccionAndamio_id        
END        
ELSE IF (@strMode='U1')        
BEGIN        
 UPDATE INSPECCION.InspeccionAndamio        
 SET  
  EstadoInspeccionAndamio_id = @EstadoInspeccionAndamio_id,        
  Cliente_id = @Cliente_id,        
  Proyecto = @Proyecto,        
  Direccion = @Direccion,        
  ZonaTrabajo = @ZonaTrabajo,        
  Responsable = @Responsable,        
  Cargo = @Cargo,        
  MarcaAndamio = @MarcaAndamio,        
  IdUsuarioEdicion = @IdUsuarioEdicion,        
  FechaEdicion = @FechaEdicion        
 WHERE InspeccionAndamio_id = @InspeccionAndamio_id;        
        
 SELECT @InspeccionAndamio_id as InspeccionAndamio_id;        
END        
ELSE IF (@strMode='U2')        
BEGIN        
 UPDATE INSPECCION.InspeccionAndamio        
 SET Proyecto = @Proyecto,  
  EstadoInspeccionAndamio_id = @EstadoInspeccionAndamio_id,           
  SobreCargaUso = @SobreCargaUso,        
  Observacion = @Observacion,        
  IdUsuarioEdicion = @IdUsuarioEdicion,        
  FechaEdicion = @FechaEdicion        
 WHERE InspeccionAndamio_id = @InspeccionAndamio_id;        
        
 SELECT @InspeccionAndamio_id as InspeccionAndamio_id;        
END        
ELSE IF (@strMode='U3')        
BEGIN        
 UPDATE INSPECCION.InspeccionAndamio        
 SET EstadoInspeccionAndamio_id = @EstadoInspeccionAndamio_id,        
  Cliente_id = @Cliente_id,        
  Proyecto = @Proyecto,        
  Direccion = @Direccion,        
  ZonaTrabajo = @ZonaTrabajo,        
  Responsable = @Responsable,        
  Cargo = @Cargo,        
  MarcaAndamio = @MarcaAndamio,            
  SobreCargaUso = @SobreCargaUso,        
  Observacion = @Observacion,        
  IdUsuarioEdicion = @IdUsuarioEdicion,        
  FechaEdicion = @FechaEdicion        
 WHERE InspeccionAndamio_id = @InspeccionAndamio_id;        
        
 SELECT @InspeccionAndamio_id as InspeccionAndamio_id;        
END        
ELSE IF (@strMode='U4')        
BEGIN        
 UPDATE INSPECCION.InspeccionAndamio        
 SET        
  EstadoInspeccionAndamio_id = @EstadoInspeccionAndamio_id,        
  IdUsuarioEdicion = @IdUsuarioEdicion,        
  FechaEdicion = @FechaEdicion        
 WHERE InspeccionAndamio_id = @InspeccionAndamio_id;        
        
 SELECT @InspeccionAndamio_id as InspeccionAndamio_id;        
END        
--ELSE IF (@strMode='D')        
--BEGIN        
--END        
END
GO

IF object_id('INSPECCION.Usp_CRUD_CheckList') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_CRUD_CheckList]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_CRUD_CheckList]
	@strMode CHAR(1) = NULL,
	@CheckList_id INT = NULL,
	@InspeccionAndamio_id INT = NULL,
	@TipoAndamio_id INT = NULL,
	@SubTipoAndamio_id INT = NULL,
	@IdUsuarioCreacion INT = NULL,
	@IdUsuarioEdicion INT = NULL,
	@FechaCreacion DATETIME = NULL,
	@FechaEdicion DATETIME = NULL,
	@Activo BIT = NULL
AS
BEGIN
IF (@strMode='C')
BEGIN
	INSERT INTO INSPECCION.CheckList(
		InspeccionAndamio_id,
		TipoAndamio_id,
		SubTipoAndamio_id,
		IdUsuarioCreacion,
		FechaCreacion,
		Activo)
	OUTPUT INSERTED.CheckList_id
	VALUES(
		@InspeccionAndamio_id,
		@TipoAndamio_id,
		@SubTipoAndamio_id,
		@IdUsuarioCreacion,
		@FechaCreacion,
		@Activo);
END
ELSE IF (@strMode='R')
BEGIN
	SELECT C.*, TA.nombre as TipoAndamio_Nombre, GP.Descripcion as SubTipoAndamio_Nombre
	FROM INSPECCION.CheckList C
		INNER JOIN COMERCIAL.TipoAndamio TA ON C.TipoAndamio_id = TA.tipo_sol_id
		LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id='ST' AND GP.ParametroPadre_id=TA.tipo_sol_id AND GP.GrupoParametros_id=C.SubTipoAndamio_id
	WHERE C.Activo = 1
		AND (@CheckList_id IS NULL OR CheckList_id = @CheckList_id)
		AND (@InspeccionAndamio_id IS NULL OR InspeccionAndamio_id = @InspeccionAndamio_id)
END
ELSE IF (@strMode='U')
BEGIN
	UPDATE INSPECCION.CheckList
	SET 
		InspeccionAndamio_id = @InspeccionAndamio_id,
		TipoAndamio_id = @TipoAndamio_id,
		SubTipoAndamio_id = @SubTipoAndamio_id,
		IdUsuarioEdicion = @IdUsuarioEdicion,
		FechaEdicion = @FechaEdicion
	WHERE CheckList_id=@CheckList_id
END
ELSE IF (@strMode='D')
BEGIN
	BEGIN TRAN
		DELETE FROM INSPECCION.Cumplimiento WHERE CheckList_id = @CheckList_id;
		UPDATE INSPECCION.CheckList SET Activo = 0 WHERE CheckList_id = @CheckList_id;
	COMMIT
END
END
GO

IF object_id('INSPECCION.Usp_CRUD_Cumplimiento') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_CRUD_Cumplimiento]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_CRUD_Cumplimiento]
	@strMode CHAR(1) = NULL,
	@Cumplimiento_id INT = NULL,
	@CheckList_id INT = NULL,
	@PreguntaInspeccionAndamio_id INT = NULL,
	@RespuestaCumplimiento_id INT = NULL,
	@IdUsuarioCreacion INT = NULL,
	@IdUsuarioEdicion INT = NULL,
	@FechaCreacion DATETIME = NULL,
	@FechaEdicion DATETIME = NULL,
	@Activo BIT = NULL
AS
BEGIN
IF (@strMode='C')
BEGIN
	INSERT INTO INSPECCION.Cumplimiento(
		CheckList_id,
		PreguntaInspeccionAndamio_id,
		RespuestaCumplimiento_id,
		IdUsuarioCreacion,
		IdUsuarioEdicion,
		FechaCreacion,
		FechaEdicion,
		Activo)
	OUTPUT INSERTED.Cumplimiento_id
	VALUES(
		@CheckList_id,
		@PreguntaInspeccionAndamio_id,
		@RespuestaCumplimiento_id,
		@IdUsuarioCreacion,
		@IdUsuarioEdicion,
		@FechaCreacion,
		@FechaEdicion,
		@Activo);
END
ELSE IF (@strMode='R')
BEGIN
	SELECT PIA.*, C.Cumplimiento_id,C.PreguntaInspeccionAndamio_id,C.RespuestaCumplimiento_id,C.Activo
	FROM ASESORIA.PreguntaInspeccionAndamio PIA
	INNER JOIN INSPECCION.Cumplimiento C ON C.PreguntaInspeccionAndamio_id = PIA.PreguntaInspeccionAndamio_id
	WHERE PIA.Activo = 1
		AND C.Activo = 1
		AND CheckList_id = @CheckList_id
END
--ELSE IF (@strMode='U')
--BEGIN
--END
ELSE IF (@strMode='D')
BEGIN
	DELETE FROM INSPECCION.Cumplimiento WHERE CheckList_id = @CheckList_id
END
END
GO

IF object_id('INSPECCION.Usp_ValidaPersonaCliente') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ValidaPersonaCliente]
GO

CREATE PROCEDURE [INSPECCION].[Usp_ValidaPersonaCliente]    
 @Cliente_id INT,    
 @Documento varchar(50),    
 @Password varchar(100)    
AS    
BEGIN    
 SELECT * FROM ( 
	 SELECT DISTINCT MP.Persona as Usuario_id, MP.Documento, MP.NombreCompleto, EE.Empresa AS Cliente_id    
	 FROM GL_MaestroPersonas MP    
	 INNER JOIN SEG.EmpresaEmpleado EE ON EE.Empleado = MP.Persona    
	 INNER JOIN SEG.TipoRelacionEmpresaEmpleado TEE ON TEE.IdTipoRelacionEmpresaEmpleado = EE.IdTipoRelacionEmpresaEmpleado  
	 INNER JOIN GL_MaestroPersonas CL ON CL.Persona = EE.Empresa  
	 WHERE MP.Documento = @Documento     
	  AND MP.clave = SEG.EncriptaClave(@Password)     
	  AND EE.Empresa = @Cliente_id    
	  AND TEE.IdTipoRelacionEmpresaEmpleado = 4 --INSPECTOR  
	  AND MP.Estado = 'A'    
	  AND CL.Estado = 'A'    
	  AND CL.EsCliente = 'S'    
	  )
   AS TABLA
END
GO

IF object_id('INSPECCION.Usp_ListarCliente') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_ListarCliente]
GO

CREATE PROCEDURE [INSPECCION].[Usp_ListarCliente]    
 @DocumentoUsuario varchar(50)    
AS    
BEGIN    
 SELECT * FROM
 (SELECT DISTINCT CL.Persona  AS Cliente_id, RTRIM(LTRIM(CL.NombreCompleto)) AS Nombre, CL.Documento AS Documento    
 FROM GL_MaestroPersonas CL    
 INNER JOIN SEG.EmpresaEmpleado EE ON EE.Empresa=CL.Persona    
 INNER JOIN SEG.TipoRelacionEmpresaEmpleado TEE ON TEE.IdTipoRelacionEmpresaEmpleado = EE.IdTipoRelacionEmpresaEmpleado  
 INNER JOIN GL_MaestroPersonas MP ON MP.Persona = EE.Empleado    
 WHERE CL.Estado = 'A'    
  AND CL.EsCliente = 'S'    
  AND MP.Estado = 'A'    
  AND MP.Documento = @DocumentoUsuario    
  AND TEE.IdTipoRelacionEmpresaEmpleado = 4 --INSPECTOR
  ) AS TABLA
ORDER BY Nombre   
END 
GO

IF object_id('INSPECCION.Usp_CRUD_RegistroFotografico') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_CRUD_RegistroFotografico]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_CRUD_RegistroFotografico]
	@strMode CHAR(2) = NULL,
	@RegistroFotografico_id INT = NULL,
	@InspeccionAndamio_id INT = NULL,
	@Descripcion VARCHAR(400) = NULL,
	@Nombre VARCHAR(255) = NULL,
	@NombreOriginal VARCHAR(255) = NULL,
	@EsFoto BIT = NULL,
	@IdUsuarioCreacion INT = NULL,
	@IdUsuarioEdicion INT = NULL,
	@FechaCreacion DATETIME = NULL,
	@FechaEdicion DATETIME = NULL,
	@Activo BIT = NULL
AS
BEGIN
IF (@strMode='C')
BEGIN
	INSERT INTO INSPECCION.RegistroFotografico(
		InspeccionAndamio_id,
		Descripcion,
		Nombre,
		NombreOriginal,
		EsFoto,
		IdUsuarioCreacion,
		FechaCreacion,
		Activo)
	OUTPUT INSERTED.RegistroFotografico_id
	VALUES(
		@InspeccionAndamio_id,
		@Descripcion,
		@Nombre,
		@NombreOriginal,
		@EsFoto,
		@IdUsuarioCreacion,
		@FechaCreacion,
		@Activo);
END
ELSE IF (@strMode='R1')
BEGIN
	SELECT * FROM INSPECCION.RegistroFotografico 
	WHERE Activo = 1
		AND InspeccionAndamio_id = @InspeccionAndamio_id
END
ELSE IF (@strMode='R2')
BEGIN
	SELECT * FROM INSPECCION.RegistroFotografico 
	WHERE Activo = 1
		AND Nombre = @Nombre
END
ELSE IF (@strMode='U')
BEGIN
	UPDATE INSPECCION.RegistroFotografico
	SET 
		InspeccionAndamio_id = @InspeccionAndamio_id,
		Descripcion = @Descripcion,
		Nombre = @Nombre,
		NombreOriginal = @NombreOriginal,
		EsFoto = @EsFoto,
		IdUsuarioEdicion = @IdUsuarioEdicion,
		FechaEdicion = @FechaEdicion
	WHERE RegistroFotografico_id = @RegistroFotografico_id
END
ELSE IF (@strMode='D')
BEGIN
	BEGIN TRAN
		DELETE FROM INSPECCION.RegistroFotografico WHERE RegistroFotografico_id = @RegistroFotografico_id;
	COMMIT
END
END
GO

IF object_id('INSPECCION.SPU_OBTENER_REPORTE') IS NOT NULL
	DROP PROC INSPECCION.SPU_OBTENER_REPORTE;
GO
  
CREATE PROCEDURE INSPECCION.SPU_OBTENER_REPORTE    
(    
@InspeccionAndamio_id INT    
)    
AS    
BEGIN    
    
	SELECT IA.*, 
	EIA.Nombre as Estado, 
	CL.NombreCompleto AS NombreCliente,
	PE.NombreCompleto AS NombreUsuarioCreacion,
	RTRIM(LTRIM(CL.NombreCompleto)) AS NombreCliente,    
	(    
		SELECT STRING_AGG(Nombres, ', ')    
		FROM (    
				SELECT DISTINCT Nombres    
				FROM (    
						SELECT CASE    
								WHEN GP.Descripcion IS NULL THEN TA.nombre    
								ELSE CONCAT(TA.nombre, ' - ', GP.Descripcion)    
							END AS Nombres    
						FROM COMERCIAL.TipoAndamio TA    
							INNER JOIN INSPECCION.CheckList C ON C.TipoAndamio_id = TA.tipo_sol_id    
							LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id = 'ST'    
							AND GP.ParametroPadre_id = TA.tipo_sol_id    
							AND GP.GrupoParametros_id = C.SubTipoAndamio_id    
						WHERE C.Activo = 1    
							AND C.InspeccionAndamio_id = IA.InspeccionAndamio_id    
					) AS A1    
			) AS A2    
	) AS TipoAndamio_Nombres
	FROM INSPECCION.InspeccionAndamio IA        
	INNER JOIN INSPECCION.EstadoInspeccionAndamio EIA ON EIA.EstadoInspeccionAndamio_id=IA.EstadoInspeccionAndamio_id           
	INNER JOIN GL_MaestroPersonas CL ON CL.Persona = IA.Cliente_id  
	INNER JOIN GL_MaestroPersonas PE ON PE.Persona = IA.IdUsuarioCreacion
	WHERE IA.Activo = 1        
   AND IA.InspeccionAndamio_id=@InspeccionAndamio_id  
 --SELECT C.*, TA.nombre as TipoAndamio_Nombre, GP.Descripcion as SubTipoAndamio_Nombre      
 -- FROM INSPECCION.CheckList C      
 --  INNER JOIN COMERCIAL.TipoAndamio TA ON C.TipoAndamio_id = TA.tipo_sol_id      
 --  LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id='ST' AND GP.ParametroPadre_id=TA.tipo_sol_id AND GP.GrupoParametros_id=C.SubTipoAndamio_id      
 -- WHERE C.Activo = 1 AND C.InspeccionAndamio_id = @InspeccionAndamio_id    
    
  SELECT -- TIPO DE ANDAMIOS    
   CL.TipoAndamio_id, CL.SubTipoAndamio_id, TA.nombre as TipoAndamio_Nombre, GP.Descripcion as SubTipoAndamio_Nombre,    
   -- CHECKLIST    
   CL.CheckList_id,    
   -- CUMPLIMIENTO    
   C.Cumplimiento_id, C.PreguntaInspeccionAndamio_id, PIA.Orden, PIA.PreguntaInspeccionAndamio, C.RespuestaCumplimiento_id, RC.RespuestaCumplimiento    
  FROM ASESORIA.PreguntaInspeccionAndamio PIA    
  INNER JOIN INSPECCION.Cumplimiento C ON C.PreguntaInspeccionAndamio_id = PIA.PreguntaInspeccionAndamio_id    
  INNER JOIN ASESORIA.RespuestaCumplimiento RC ON RC.RespuestaCumplimiento_id = C.RespuestaCumplimiento_id    
  INNER JOIN INSPECCION.CheckList CL ON CL.CheckList_id = C.CheckList_id    
  INNER JOIN COMERCIAL.TipoAndamio TA ON CL.TipoAndamio_id = TA.tipo_sol_id     
  LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id='ST' AND GP.ParametroPadre_id=TA.tipo_sol_id AND GP.GrupoParametros_id=CL.SubTipoAndamio_id    
  WHERE PIA.Activo = 1    
   AND C.Activo = 1    
   AND C.CheckList_id IN (    
    SELECT CheckList_id FROM INSPECCION.CheckList CHK    
    WHERE CHK.Activo = 1 AND CHK.InspeccionAndamio_id = @InspeccionAndamio_id    
   )    
   ORDER BY C.CheckList_id ASC, PIA.Orden    
    
    
 SELECT * FROM INSPECCION.RegistroFotografico RF    
 WHERE RF.InspeccionAndamio_id = @InspeccionAndamio_id AND RF.Activo = 1;    
  
  
 --SELECT C.*, TA.nombre as TipoAndamio_Nombre, GP.Descripcion as SubTipoAndamio_Nombre    
 -- FROM INSPECCION.CheckList C    
 --  INNER JOIN COMERCIAL.TipoAndamio TA ON C.TipoAndamio_id = TA.tipo_sol_id    
 --  LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id='ST' AND GP.ParametroPadre_id=TA.tipo_sol_id AND GP.GrupoParametros_id=C.SubTipoAndamio_id    
 -- WHERE C.Activo = 1 AND C.InspeccionAndamio_id = @InspeccionAndamio_id  
  
  SELECT -- TIPO DE ANDAMIOS  
   CL.TipoAndamio_id, CL.SubTipoAndamio_id, TA.nombre as TipoAndamio_Nombre, GP.Descripcion as SubTipoAndamio_Nombre,  
   -- CHECKLIST  
   CL.CheckList_id,  
   -- CUMPLIMIENTO  
   C.Cumplimiento_id, C.PreguntaInspeccionAndamio_id, PIA.Orden, PIA.PreguntaInspeccionAndamio, C.RespuestaCumplimiento_id, RC.RespuestaCumplimiento  
  FROM ASESORIA.PreguntaInspeccionAndamio PIA  
  INNER JOIN INSPECCION.Cumplimiento C ON C.PreguntaInspeccionAndamio_id = PIA.PreguntaInspeccionAndamio_id  
  INNER JOIN ASESORIA.RespuestaCumplimiento RC ON RC.RespuestaCumplimiento_id = C.RespuestaCumplimiento_id  
  INNER JOIN INSPECCION.CheckList CL ON CL.CheckList_id = C.CheckList_id  
  INNER JOIN COMERCIAL.TipoAndamio TA ON CL.TipoAndamio_id = TA.tipo_sol_id   
  LEFT JOIN ASESORIA.GrupoParametros GP ON GP.Grupo_id='ST' AND GP.ParametroPadre_id=TA.tipo_sol_id AND GP.GrupoParametros_id=CL.SubTipoAndamio_id  
  WHERE PIA.Activo = 1  
   AND C.Activo = 1  
   AND C.CheckList_id IN (  
    SELECT CheckList_id FROM INSPECCION.CheckList CHK  
    WHERE CHK.Activo = 1 AND CHK.InspeccionAndamio_id = @InspeccionAndamio_id  
   )  
   ORDER BY C.CheckList_id ASC, PIA.Orden  
  
  
 SELECT * FROM INSPECCION.RegistroFotografico RF  
 WHERE RF.InspeccionAndamio_id = @InspeccionAndamio_id AND RF.Activo = 1;  
END 
GO

IF object_id('INSPECCION.Usp_CRUD_ConfiguracionApp') IS NOT NULL
	DROP PROC [INSPECCION].[Usp_CRUD_ConfiguracionApp]
GO  

CREATE PROCEDURE [INSPECCION].[Usp_CRUD_ConfiguracionApp]
	@strMode CHAR(1) = NULL,
	@Usuario_id INT = NULL,
	@ModoHistorico BIT = NULL,
	@CantidadAnios INT = NULL,
	@Fecha DATETIME = NULL,
	@IdUsuario INT = NULL,
	@Activo BIT = NULL
AS
BEGIN
IF (@strMode='C')
BEGIN
	IF EXISTS(SELECT * FROM INSPECCION.ConfiguracionApp WHERE Usuario_id=@usuario_id)
		BEGIN
			UPDATE INSPECCION.ConfiguracionApp
			SET ModoHistorico = @ModoHistorico,
				CantidadAnios = @CantidadAnios,
				IdUsuarioEdicion = @IdUsuario,
				FechaEdicion = @Fecha,
				Activo = @Activo
			WHERE Usuario_id = @Usuario_id
		END
	ELSE
		BEGIN
			INSERT INTO INSPECCION.ConfiguracionApp(
				Usuario_id,
				ModoHistorico,
				CantidadAnios,
				IdUsuarioCreacion,
				FechaCreacion,
				Activo)
			OUTPUT INSERTED.ConfiguracionApp_id
			VALUES(
				@Usuario_id,
				@ModoHistorico,
				@CantidadAnios,
				@IdUsuario,
				@Fecha,
				@Activo);
		END
END
ELSE IF (@strMode='R')
BEGIN
	SELECT Usuario_id, ModoHistorico, CantidadAnios
	FROM INSPECCION.ConfiguracionApp
	WHERE Usuario_id=@Usuario_id
END
--ELSE IF (@strMode='U')
--BEGIN
--END
--ELSE IF (@strMode='D')
--BEGIN
--END
END
GO

 --RollBack
 --DROP PROC [INSPECCION].[Usp_ListarInspeccionAndamio];
 --DROP PROC [INSPECCION].[Usp_ListarPreguntaInspeccionAndamio];
 --DROP PROC [INSPECCION].[Usp_ListarPreguntaInspeccionAndamioSubTipoAndamio];
 --DROP PROC [INSPECCION].[Usp_ListarProyecto];
 --DROP PROC [INSPECCION].[Usp_ListarTipoAndamio];
 --DROP PROC [INSPECCION].[Usp_CRUD_InspeccionAndamio];
 --DROP PROC [INSPECCION].[Usp_CRUD_CheckList];
 --DROP PROC [INSPECCION].[Usp_CRUD_Cumplimiento];
 --DROP PROC [INSPECCION].[Usp_ValidaPersonaCliente];
 --DROP PROC [INSPECCION].[Usp_ListarCliente];
 --DROP PROC [INSPECCION].[Usp_CRUD_RegistroFotografico];
 --DROP PROC INSPECCION.SPU_OBTENER_REPORTE;
