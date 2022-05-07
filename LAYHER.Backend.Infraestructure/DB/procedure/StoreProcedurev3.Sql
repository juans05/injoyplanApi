IF object_id('AUC.Usp_ListaProyectoCliente') IS NOT NULL
     DROP PROC AUC.Usp_ListaProyectoCliente
GO

CREATE OR ALTER   PROCEDURE [AUC].[Usp_ListaProyectoCliente]
	@Proyecto varchar(15)
AS
BEGIN
	Select 
			A.afe,A.companyowner,
			A.localname,
			B.NombreCompleto as NombreCliente,
			A.id_ejecutivo as IdComercial,
			C.NombreCompleto as NombreComercial,
			D.vc_idContrato as IdContrato,
			D.vc_DescripContrato as NombreContrato
	From CB_MestroProyectos A 
	inner join GL_MaestroPersonas B on A.Client_ID = B.Persona
	inner join GL_MaestroPersonas C on A.id_ejecutivo = C.Persona
	inner join FC_MaestroContratos D on A.afe = D.vc_idProyecto
	Where (A.afe = @Proyecto or '0' = @Proyecto) 
	ORDER BY A.afe;
END
GO

IF object_id('PU.Usp_ListProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionUnidad
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListProgramacionUnidad]
	@ProgramacionUnidad int,
	@TipoProgramacion int,
	@Proyecto varchar(15),
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@Almacen varchar(10),
	@Persona int
AS
BEGIN
	select 
			B.Nombre as NombreEstado,C.Metro as Longitud,D.Nombre as NombreUnidad,F.DescripcionLocal,I.NombreCompleto,
			K.Nombre as NombreTipoProgramacion,L.NombreCompleto as NombreUsuarioCreacion,
			J.Id as IdFormulario, J.NumeroDocumento, J.RazonSocial,J.ModeloVehiculo,J.PlacaTracto,J.PlacaCarreta,
			A.Id as IdProgramacionUnidad,A.* 
	from PU.ProgramacionUnidad A
	left join PU.Formulario J on A.IdFormulario = J.Id
	left join PU.TipoEstado B on A.IdEstado = B.Id
	left join PU.MetrajeTrailer C on A.IdMetrajeTrailer = C.Id 
	left join PU.TipoUnidad D on A.IdTipoUnidad = D.Id
	left join LG_MaestroAlmacen F on A.IdAlmacen = F.AlmacenCodigo
	left join PU.Formulario G on A.IdFormulario = G.Id
	left join CB_MestroProyectos H on A.IdProyecto = H.afe
	left join GL_MaestroPersonas I on H.Client_ID = I.Persona
	left join PU.TipoProgramacion K on A.IdTipoProgramacion = k.Id
	left join GL_MaestroPersonas L on A.IdUsuarioCreacion = L.Persona
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
		  and (A.IdTipoProgramacion = @TipoProgramacion or 0 = @TipoProgramacion)
		  and (A.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (A.FechaInicio between @FechaInicio and @FechaFin)
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and A.Activo = 1;
END
GO

IF object_id('PU.Usp_SaveProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionUnidad
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_SaveProgramacionUnidad]
	@TipoProgramacion int,
	@Proyecto varchar(15),
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@TipoUnidad int,
	@MetrajeTrailer int,
	@Especificacion varchar(50),
	@Almacen varchar(10),
	@Tonelada decimal(12,2),
	@Correo varchar(500),
	@FinAlquiler datetime,
	@FechaRevision datetime,
	@RevisionHoraInicio datetime,
	@RevisionHoraFin datetime,
	@Cotizacion varchar(50),	
	@NombreCliente varchar(100),
	@NombreProyecto varchar(100),
	@FechaLlegada datetime,
	@TurnoLlegada datetime,
	@TelefonoEncargado varchar(9),
	@NumeroContenedores varchar(30),
	@Contenedor varchar(30),
	@PackingList varchar(50),
	@TipoImportacion varchar(50),
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.ProgramacionUnidad(IdTipoProgramacion,
									  IdProyecto,
									  FechaInicio,
									  FechaFin,
									  IdEstado,
									  IdTipoUnidad,
									  IdMetrajeTrailer,
									  Especificacion,
									  IdAlmacen,
									  Tonelada,
									  Correo,
									  AlquilerFin,
									  FechaRevision,
									  RevisionHoraInicio,
									  RevisionHoraFin,
									  IdCotizacion,
									  Activo,
									  NombreCliente,
									  NombreProyecto,
									  FechaLlegada,
									  TurnoLlegada,
									  TelefonoEncargado,
									  NumeroContenedores,
									  Contenedor,
									  PackingList,
									  TipoImportacion,
									  IdUsuarioCreacion,
									  FechaCreacion) 
	OUTPUT inserted.ID AS IdProgramacionUnidad
	values(@TipoProgramacion,
		   @Proyecto,
		   @FechaInicio,
		   @FechaFin,
		   @Estado,
		   @TipoUnidad,
		   @MetrajeTrailer,
		   @Especificacion,
		   @Almacen,
		   @Tonelada,
		   @Correo,
		   @FinAlquiler,
		   @FechaRevision,
		   @RevisionHoraInicio,
		   @RevisionHoraFin,
		   @Cotizacion,
		   1,
		   @NombreCliente,
		   @NombreProyecto,
		   @FechaLlegada,
		   @TurnoLlegada,
		   @TelefonoEncargado,
		   @NumeroContenedores,
		   @Contenedor,
		   @PackingList,
		   @TipoImportacion,
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_EliminarPogramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_EliminarPogramacionUnidad
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarPogramacionUnidad]
	@Programacion int
AS
BEGIN
	DECLARE @Persona int
	Declare @Response bit 
	SET @Persona = (SELECT Id from PU.ProgramacionUnidad where Id = @Programacion);
	SET @Response = 0;
	IF @Persona <> 0
	  BEGIN
	      Delete from PU.ProgramacionUnidad where Id = @Programacion
		  SET @Response = 1;
	  END	  
	select	'Return' = @Response
END
GO

IF object_id('PU.Usp_InhabilitarHabilitarProgramacion') IS NOT NULL
     DROP PROC PU.Usp_InhabilitarHabilitarProgramacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_InhabilitarHabilitarProgramacion]
	@Programacion int,
	@Estado bit
AS
BEGIN
	DECLARE @EstadoActual char(1)
	Declare @Response bit 
	SET @EstadoActual = (SELECT activo from PU.ProgramacionUnidad where Id =  @Programacion);
	IF @EstadoActual <> @Estado
	  BEGIN

		  update PU.ProgramacionUnidad set Activo =  @Estado  where Id =  @Programacion;
		  SET @Response = 1;
	  END
	Else
		BEGIN
			SET @Response = 0;
		END	
	select	'Return' = @Response
END
GO

IF object_id('PU.Usp_SaveAdjuntoFormulario') IS NOT NULL
     DROP PROC PU.Usp_SaveAdjuntoFormulario
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_SaveAdjuntoFormulario]
	@Formulario int,
	@Adjunto int,
	@Nombre varchar(30),
	@Ruta	varchar(100),
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.AdjuntoFormulario(IdFormulario,
									  IdAdjunto,
									  Nombre,
									  Ruta,
									  IdUsuarioCreacion,
									  FechaCreacion) 
	OUTPUT inserted.ID AS IdAdjuntoFormulario
	values(@Formulario,
		   @Adjunto,
		   @Nombre,
		   @Ruta,
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_SaveFormularioConsideracion') IS NOT NULL
     DROP PROC PU.Usp_SaveFormularioConsideracion
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_SaveFormularioConsideracion]
	@Formulario int,
	@Consideracion int,
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.FormularioConsideracion(IdFormulario,
										   IdConsideracion,
										   IdUsuarioCreacion,
										   FechaCreacion) 
	OUTPUT inserted.IdFormulario
	values(@Formulario,
		   @Consideracion,
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_UpdateProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_UpdateProgramacionUnidad
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_UpdateProgramacionUnidad]
	@ProgramacionUnidad int,
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@TipoUnidad int,
	@MetrajeTrailer int,
	@Especificacion varchar(50),
	@Almacen varchar(10),
	@Tonelada decimal(12,2),
	@Correo varchar(500),
	@FinAlquiler datetime,
	@FechaRevision datetime,
	@RevisionHoraInicio datetime,
	@RevisionHoraFin datetime,
	@Cotizacion varchar(50),	
	@NombreCliente varchar(100),
	@NombreProyecto varchar(100),
	@FechaLlegada datetime,
	@TurnoLlegada datetime,
	@TelefonoEncargado varchar(9),
	@NumeroContenedores varchar(30),
	@Contenedor varchar(30),
	@PackingList varchar(50),
	@TipoImportacion varchar(50),
	@UsuarioEdicion int
AS
BEGIN
	Update PU.ProgramacionUnidad
	set FechaInicio = @FechaInicio,
		FechaFin = @FechaFin,
		IdEstado = @Estado,
		IdTipoUnidad = @TipoUnidad,
		IdMetrajeTrailer = @MetrajeTrailer,
		Especificacion = @Especificacion,
		IdAlmacen = @Almacen,
		Tonelada = @Tonelada,
		Correo = @Correo,
		AlquilerFin = @FinAlquiler,
		FechaRevision = @FechaRevision,
		RevisionHoraInicio = @RevisionHoraInicio,
		RevisionHoraFin = @RevisionHoraFin,
		IdCotizacion = @Cotizacion,
		NombreCliente = @NombreCliente,
		NombreProyecto = @NombreProyecto,
        FechaLlegada = @FechaLlegada,
        TurnoLlegada = @TurnoLlegada,
        TelefonoEncargado = @TelefonoEncargado,
		NumeroContenedores = @NumeroContenedores,
		Contenedor = @Contenedor,
		PackingList = @PackingList,
		TipoImportacion = @TipoImportacion,
		IdUsuarioEdicion = @UsuarioEdicion,
		FechaEdicion = GETDATE()
	where Id = @ProgramacionUnidad	
END
GO

IF object_id('PU.Usp_UpdateFormularioProgramacion') IS NOT NULL
     DROP PROC PU.Usp_UpdateFormularioProgramacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_UpdateFormularioProgramacion]
	@ProgramacionUnidad int,
	@Formulario int
AS
BEGIN
	DECLARE @Response bit
	IF EXISTS (SELECT Id from PU.Formulario where Id =  @Formulario)
	  BEGIN
		  Update PU.ProgramacionUnidad
		  set IdFormulario = @Formulario
		  where Id = @ProgramacionUnidad	
		  SET @Response = 1;
	  END
	Else
		BEGIN
			SET @Response = 0;
		END	
	select	'Return' = @Response
END
GO

IF object_id('PU.Usp_GetFormularioProgramacion') IS NOT NULL
     DROP PROC PU.Usp_GetFormularioProgramacion
GO

CREATE OR ALTER PROCEDURE PU.Usp_GetFormularioProgramacion (
@IdFormulario INT
)
AS
BEGIN
	SELECT * FROM PU.Formulario WHERE Id=@IdFormulario;
	SELECT * FROM PU.AdjuntoFormulario WHERE IdFormulario=@IdFormulario;
	SELECT * FROM PU.FormularioConsideracion WHERE IdFormulario=@IdFormulario;
END
GO

IF object_id('AUC.Usp_ResetearContrasena') IS NOT NULL
	DROP PROC AUC.Usp_ResetearContrasena
GO

CREATE PROCEDURE AUC.Usp_ResetearContrasena(
 @IdUsuario INT,
 @Clave varchar(20)
)
AS  
BEGIN  
 DECLARE @ExisteUsuario int  
 Declare @Exist bit   
 SET @ExisteUsuario = (SELECT count(*) from GL_MaestroPersonas where EsCliente='S' AND estado='A' AND Persona=@IdUsuario);  
 IF @ExisteUsuario = 1  
   BEGIN  
    update GL_MaestroPersonas set Clave =  SEG.EncriptaClave(@Clave)  where Persona =  @IdUsuario;  
    SET @Exist = 1;  
   END  
 Else  
  BEGIN  
   SET @Exist = 0;  
  END   
 select @Exist;
END  
GO


IF object_id('AUC.Usp_ObtenerUsuarioPorNroDocumento') IS NOT NULL
	DROP PROC AUC.Usp_ObtenerUsuarioPorNroDocumento
GO

CREATE PROCEDURE [AUC].[Usp_ObtenerUsuarioPorNroDocumento]
	@NroDocumento varchar(50)
AS
BEGIN
SELECT
	SEG.desencriptaClave(MP.Clave),
	MP.Persona as Id,
    MP.Documento as NroDocumento,
	MP.Nombres,
	MP.ApellidoPaterno,
	MP.ApellidoMaterno,
    RTRIM(LTRIM(MP.NombreCompleto)) AS NombreCompleto,
    MP.CorreoElectronico,
    '-' as Rol,
    CL.Persona  AS Cliente_Id, 
    RTRIM(LTRIM(CL.NombreCompleto)) AS Cliente_NombreCompleto
FROM GL_MaestroPersonas CL
    INNER JOIN SEG.EmpresaEmpleado EE ON EE.Empresa=CL.Persona
    INNER JOIN GL_MaestroPersonas MP ON MP.Persona = EE.Empleado
WHERE CL.Estado = 'A'
    AND CL.EsCliente = 'S'
    AND MP.Estado = 'A'
AND MP.Documento = @NroDocumento
END
GO