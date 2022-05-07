/**
CREACION DE FUNCIONES
**/
IF object_id('AUC.fn_Split') IS NOT NULL
     DROP FUNCTION AUC.fn_Split
GO

CREATE FUNCTION [AUC].[fn_Split](@text varchar(8000), @delimiter varchar(20) = ' ')
RETURNS @Strings TABLE
(   
  position int IDENTITY PRIMARY KEY,
  value varchar(8000)  
)
AS
BEGIN

DECLARE @index int
SET @index = -1

WHILE (LEN(@text) > 0)
  BEGIN 
    SET @index = CHARINDEX(@delimiter , @text) 
    IF (@index = 0) AND (LEN(@text) > 0) 
      BEGIN  
        INSERT INTO @Strings VALUES (@text)
          BREAK 
      END 
    IF (@index > 1) 
      BEGIN  
        INSERT INTO @Strings VALUES (LEFT(@text, @index - 1))  
        SET @text = RIGHT(@text, (LEN(@text) - @index)) 
      END 
    ELSE
      SET @text = RIGHT(@text, (LEN(@text) - @index))
    END
  RETURN
END
GO

/**
CREACION DE PROCEDIMIENTOS ALMACENADOS
**/
IF object_id('AUC.Usp_CambioContrasena') IS NOT NULL
     DROP PROC AUC.Usp_CambioContrasena
GO

CREATE PROCEDURE AUC.Usp_CambioContrasena
	@Documento VARCHAR(12), 
	@NuevaClave varchar(20),
	@AntiguaClave varchar(20)
AS
BEGIN
	DECLARE @ExisteClave int
	Declare @Exist bit 
	SET @ExisteClave = (SELECT count(*) from GL_MaestroPersonas where Documento =  @Documento  and Clave =  SEG.EncriptaClave(@AntiguaClave) and estado =  'A');
	IF @ExisteClave = 1
	  BEGIN

		  update GL_MaestroPersonas set Clave =  SEG.EncriptaClave(@NuevaClave)  where Documento =  @Documento;
		  SET @Exist = 1;
	  END
	Else
		BEGIN
			SET @Exist = 0;
		END	
	select	'Return' = @Exist
END
GO

IF object_id('AUC.Usp_InhabilitarHabilitarUsuario') IS NOT NULL
     DROP PROC AUC.Usp_InhabilitarHabilitarUsuario
GO

CREATE PROCEDURE AUC.Usp_InhabilitarHabilitarUsuario
	@Documento VARCHAR(50),
	@Estado char(1)
AS
BEGIN
	DECLARE @EstadoActual char(1)
	Declare @Response bit 
	SET @EstadoActual = (SELECT estado from GL_MaestroPersonas where Documento =  @Documento);
	IF @EstadoActual <> @Estado
	  BEGIN

		  update GL_MaestroPersonas set Estado =  @Estado  where Documento =  @Documento;
		  SET @Response = 1;
	  END
	Else
		BEGIN
			SET @Response = 0;
		END	
	select	'Return' = @Response
END
GO

IF object_id('AUC.Usp_EliminarUsuario') IS NOT NULL
     DROP PROC AUC.Usp_EliminarUsuario
GO

CREATE PROCEDURE AUC.Usp_EliminarUsuario
	@Documento VARCHAR(50)
AS
BEGIN
	DECLARE @Persona int
	DECLARE @PerfilPersona int
	Declare @Response bit 
	SET @Persona = (SELECT Persona from GL_MaestroPersonas where Documento =  @Documento);
	SET @PerfilPersona = (SELECT IdPerfil_Persona from AUC.Perfil_Persona where IdPersona = @Persona);
	SET @Response = 0;
	IF @Persona <> 0
	  BEGIN
	      -- Se elimina la relacion del Usuario con su padre
	      Delete from SEG.EmpresaEmpleado where Empleado = @Persona
		  -- Se elimina la relacion de los permisos con el perfilpersona
		  Delete from AUC.Permiso_Perfil_Persona where IdPerfil_Persona = @PerfilPersona
		  -- Se elimina la relacion de los proyectos con el perfilpersona
		  Delete from AUC.Proyecto_Perfil_Persona where IdPerfil_Persona = @PerfilPersona
		  -- Se elimina el PerfilPersona del Usuario
		  Delete from AUC.Perfil_Persona where IdPersona = @Persona
		  -- Se elimina el Usuario
		  Delete from GL_MaestroPersonas where Persona = @Persona

		  SET @Response = 1;
	  END
	  
	select	'Return' = @Response
END
GO

IF object_id('PU.Usp_ListaSede') IS NOT NULL
     DROP PROC PU.Usp_ListaSede
GO

CREATE PROCEDURE PU.Usp_ListaSede
	@AlmacenRegion int
AS
BEGIN
	select * from LG_RegionAlmacen where (AlmacenRegion = @AlmacenRegion or 0 = @AlmacenRegion) and Estado = 'A'
END
GO

IF object_id('PU.Usp_ListaAlmacen') IS NOT NULL
     DROP PROC PU.Usp_ListaAlmacen
GO

CREATE PROCEDURE PU.Usp_ListaAlmacen
	@Responsable int
AS
BEGIN
	select A.*, B.DescripcionLocal 
	from SEG.AreaAlmacen A 
	join LG_RegionAlmacen B on A.IdAlmacen = B.AlmacenRegion 
	where (A.IdResponsable = @Responsable or 0 = @Responsable) and B.Estado = 'A' and A.Activo = 1
END
GO

IF object_id('PU.Usp_ListaCotizacion') IS NOT NULL
     DROP PROC PU.Usp_ListaCotizacion
GO

CREATE PROCEDURE PU.Usp_ListaCotizacion
	@Proyecto varchar(15),
	@OffSet int,
	@Fetch int
AS
BEGIN
	SELECT DISTINCT RTRIM(LTRIM(vc_idcotizacion)) as IdCotizacion 
	FROM CT_Cotizacion_Control_Cantidades 
	WHERE (vc_idproyecto = @Proyecto or '0' = @Proyecto) 
	AND dbl_CantidadDespachada<dbl_CantidadPedida 
	ORDER BY IdCotizacion 
	OFFSET @OffSet ROWS FETCH NEXT @Fetch ROWS ONLY
END
GO

IF object_id('COMUN.Usp_ListaPersonas') IS NOT NULL
     DROP PROC COMUN.Usp_ListaPersonas
GO

CREATE PROCEDURE COMUN.Usp_ListaPersonas
	@Persona int,
	@OffSet int,
	@Fetch int
AS
BEGIN
		Select Persona,Documento,TipoDocumento,Estado 
		from GL_MaestroPersonas 
		where (Persona = @Persona or 0 = @Persona) and Estado = 'A' 
		ORDER BY Persona 
		OFFSET @OffSet ROWS FETCH NEXT @Fetch ROWS ONLY
END
GO

IF object_id('AUC.Usp_ListaClientes') IS NOT NULL
     DROP PROC AUC.Usp_ListaClientes
GO

CREATE PROCEDURE AUC.Usp_ListaClientes
AS
BEGIN
		Select A.Persona,A.NombreCompleto 
		From GL_MaestroPersonas A 
		Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
		Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
		Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
		Where C.IdPerfil = 91 and C.Activo = 1 and D.Activo = 1
END
GO

IF object_id('PU.Usp_ListaMetrajes') IS NOT NULL
     DROP PROC PU.Usp_ListaMetrajes
GO

CREATE PROCEDURE PU.Usp_ListaMetrajes
		@Id int
AS
BEGIN
		Select Id as IdMetrajeTrailer,Metro 
		from PU.MetrajeTrailer 
		where (Id = @Id or 0 = @Id) and Activo = 1;
END
GO

IF object_id('AUC.Usp_ListaTipoDocumento') IS NOT NULL
     DROP PROC AUC.Usp_ListaTipoDocumento
GO

CREATE OR ALTER PROCEDURE AUC.Usp_ListaTipoDocumento
		@Id int,
		@Estado int
AS
BEGIN
		Select A.IdTipoDocumento, A.TipoDocumento as Documento,A.Descripcion 
		from COMUN.TipoDocumentoIdentidad A 
		INNER JOIN COMUN.TipoPersonaTipoDocumentoIdentidad TPTDI ON A.TipoDocumento = TPTDI.TipoDocumento 
		WHERE (IdTipoDocumento = @Id or 0 = @Id) and (estado = @Estado or 0 = @Estado) and TPTDI.TipoPersona = 'N';
END
GO

IF object_id('AUC.Usp_ValidaPersona') IS NOT NULL
     DROP PROC AUC.Usp_ValidaPersona
GO

CREATE PROCEDURE AUC.Usp_ValidaPersona
		@Documento varchar(50),
		@Password varchar(100),
		@Perfil int
AS
BEGIN
		Select A.Persona,A.NombreCompleto 
		From GL_MaestroPersonas A 
		Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
		Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
		Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
		Where A.Documento = @Documento 
			and A.clave = SEG.EncriptaClave(@Password) 
			and C.IdPerfil = @Perfil 
			and C.Activo = 1 
			and D.Activo = 1 
			and A.Estado = 'A';
END
GO

IF object_id('PU.Usp_ListaTipoEstado') IS NOT NULL
     DROP PROC PU.Usp_ListaTipoEstado
GO

CREATE PROCEDURE PU.Usp_ListaTipoEstado
		@Id int
AS
BEGIN
		Select Id as IdTipoEstado, Nombre 
		From PU.TipoEstado 
		Where (ID = @Id or 0 = @Id) and Activo = 1;
END
GO

IF object_id('PU.Usp_ListaTipoProgramacion') IS NOT NULL
     DROP PROC PU.Usp_ListaTipoProgramacion
GO

CREATE PROCEDURE PU.Usp_ListaTipoProgramacion
		@Id int
AS
BEGIN
		Select Id as IdTipoProgramacion,Nombre 
		From PU.TipoProgramacion 
		Where (Id = @Id or 0 = @Id) and Activo = 1;
END
GO

IF object_id('PU.Usp_ListaTipoProgramacionxEstado') IS NOT NULL
     DROP PROC PU.Usp_ListaTipoProgramacionxEstado
GO

CREATE PROCEDURE PU.Usp_ListaTipoProgramacionxEstado
		@Id int
AS
BEGIN
		Select A.IdTipoEstado, C.Nombre 
		From PU.ProgramacionEstado A 
		Join PU.TipoProgramacion B on A.IdTipoProgramacion = B.Id 
		Join PU.TipoEstado C on A.IdTipoEstado = C.Id 
		Where (A.IdTipoProgramacion = @Id or 0 = @Id) and B.Activo = 1 and C.Activo = 1;
END
GO

IF object_id('COMUN.Usp_ListaZonas') IS NOT NULL
     DROP PROC COMUN.Usp_ListaZonas
GO

CREATE PROCEDURE COMUN.Usp_ListaZonas
		@Id int
AS
BEGIN
		Select IdZona,Nombre,Activo 
		From SEG.Zona 
		Where (IdZona = @Id or 0 = @Id) and Activo = 1;
END
GO

IF object_id('PU.Usp_ListaTipoUnidad') IS NOT NULL
     DROP PROC PU.Usp_ListaTipoUnidad
GO

CREATE PROCEDURE PU.Usp_ListaTipoUnidad
		@Id int
AS
BEGIN
		Select Id as IdTipoUnidad, Nombre 
		From PU.TipoUnidad 
		Where (Id = @id or 0 = @id) and Activo = 1;
END
GO

IF object_id('PU.Usp_ListaTipoUnidadxMetraje') IS NOT NULL
     DROP PROC PU.Usp_ListaTipoUnidadxMetraje
GO

CREATE PROCEDURE PU.Usp_ListaTipoUnidadxMetraje
		@Id int
AS
BEGIN
		Select A.IdMetrajeTrailer, B.Metro 
		From PU.UnidadMetraje A 
		Join PU.MetrajeTrailer B on A.IdMetrajeTrailer = B.Id 
		Join PU.TipoUnidad C on A.IdTipoUnidad = C.id 
		Where (A.IdTipoUnidad = @Id or 0 = @Id) and B.Activo = 1 and C.Activo = 1;
END
GO

IF object_id('AUC.Usp_ListaProyectos') IS NOT NULL
     DROP PROC AUC.Usp_ListaProyectos
GO

CREATE PROCEDURE AUC.Usp_ListaProyectos
	@Proyecto varchar(15),
	@OffSet int,
	@Fetch int
AS
BEGIN
	Select afe,companyowner,localname 
	From CB_MestroProyectos 
	Where (afe = @Proyecto or '0' = @Proyecto) 
	ORDER BY afe 
	OFFSET @OffSet ROWS 
	FETCH NEXT @Fetch ROWS ONLY;
END
GO

IF object_id('AUC.Usp_ListaProyectosxPersona') IS NOT NULL
     DROP PROC AUC.Usp_ListaProyectosxPersona
GO

CREATE PROCEDURE AUC.Usp_ListaProyectosxPersona
	@Persona int,
	@Proyecto varchar(15),
	@ListaProyectos varchar(200),
	@LocalName varchar(200),
	@Zona int,
	@OffSet int,
	@Fetch int
AS
BEGIN
	IF @LocalName is NULL
	BEGIN
		SET @LocalName = '';
	END
	Select Z.IdZona,Z.Nombre as NombreZona,A.afe,A.companyowner,A.localname 
	From CB_MestroProyectos A 
	LEFT JOIN LY_MDepartamento DEP ON A.departamento = DEP.vc_CodDepartamento 
	LEFT JOIN SEG.Zona Z ON DEP.in_IdTerritorio = Z.IdZona 
	Where (A.Client_Id = @Persona or 0 = @Persona) 
	and (A.afe = @Proyecto or '0' = @Proyecto) 
	and A.localname like ('%'+@localname+'%') 
	and (Z.IdZona = @Zona or 0 = @Zona) 
	and A.status = 'AP' 
	and A.afe not in (SELECT Value FROM fn_Split(@ListaProyectos, ','))
	ORDER BY afe
	OFFSET @OffSet ROWS 
	FETCH NEXT @Fetch ROWS ONLY;
END
GO

IF object_id('AUC.Usp_ListaPerfilesUsuario') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilesUsuario
GO

CREATE PROCEDURE AUC.Usp_ListaPerfilesUsuario
	@Perfil int
AS
BEGIN
	select IdPerfil, Nombre, Activo 
	from AUC.Perfil 
	where (IdPerfil = @Perfil or 0 = @Perfil) and Activo = 1;
END
GO

IF object_id('AUC.Usp_ListaPerfilesxDocumento') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilesxDocumento
GO

CREATE PROCEDURE AUC.Usp_ListaPerfilesxDocumento
	@Documento varchar(50)
AS
BEGIN
	Select D.IdPerfil, D.Nombre 
	From GL_MaestroPersonas A 
	Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
	Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
	Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
	Where A.Documento = @Documento;
END
GO

IF object_id('AUC.Usp_ListaPerfilPersona') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilPersona
GO

CREATE PROCEDURE AUC.Usp_ListaPerfilPersona
	@PerfilPersona int
AS
BEGIN
	Select 
		A.IdPerfil_Persona as IdPerfilPersona, 
		A.IdPerfil, C.Nombre as NombrePerfil, 
		A.IdPersona, B.NombreCompleto as NombrePersona 
		From AUC.Perfil_Persona A 
		Join GL_MaestroPersonas B on A.IdPersona = B.Persona 
		Join AUC.Perfil C on A.IdPerfil = C.IdPerfil  
		Where (A.IdPerfil_Persona = @PerfilPersona or 0 = @PerfilPersona);
END
GO

IF object_id('AUC.Usp_ListaPerfilPermiso') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilPermiso
GO

CREATE PROCEDURE AUC.Usp_ListaPerfilPermiso
	@Perfil int,
	@Permiso int
AS
BEGIN
	Select 
		A.IdPerfil, C.Nombre as NombrePerfil, 
		A.IdPermiso, B.Nombre as NombrePermiso 
	From AUC.Perfil_Permiso A 
	Join AUC.Permiso B on A.IdPermiso = B.IdPermiso 
	Join AUC.Perfil C on A.IdPerfil = C.IdPerfil  
	Where (A.IdPerfil = @Perfil or 0 = @Perfil) 
		and (A.IdPermiso = @Permiso or 0 = @Permiso) and A.Activo = 1;
END
GO

IF object_id('AUC.Usp_ListaPermisos') IS NOT NULL
     DROP PROC AUC.Usp_ListaPermisos
GO

CREATE PROCEDURE AUC.Usp_ListaPermisos
	@Permiso int
AS
BEGIN
	select IdPermiso, Nombre, Activo 
	From AUC.Permiso 
	Where (IdPermiso = @Permiso or 0 = @Permiso);
END
GO

IF object_id('AUC.Usp_ListaPermisoPerfilPersona') IS NOT NULL
     DROP PROC AUC.Usp_ListaPermisoPerfilPersona
GO

CREATE PROCEDURE AUC.Usp_ListaPermisoPerfilPersona
	@PermisoPerfilPersona int
AS
BEGIN
	Select 
				* 
	From AUC.Permiso_Perfil_Persona A 
	Join AUC.Perfil_Persona B on A.IdPerfil_Persona = B.IdPerfil_Persona 
	Join AUC.Perfil C on B.IdPerfil = C.IdPerfil 
	Join AUC.Permiso D on A.IdPermiso = D.IdPermiso 
	Where (IdPermiso_Perfil_Persona = @PermisoPerfilPersona or 0 = @PermisoPerfilPersona);
END
GO

IF object_id('AUC.Usp_ListaPermisoProyectoPerfilPersona') IS NOT NULL
     DROP PROC AUC.Usp_ListaPermisoProyectoPerfilPersona
GO

CREATE PROCEDURE AUC.Usp_ListaPermisoProyectoPerfilPersona
	@Persona int,
	@Perfil int,
	@Proyecto varchar(15),
	@Documento varchar(50),
	@Nombre varchar(200)
AS
BEGIN
	IF @Nombre is NULL
	BEGIN
		SET @Nombre = '';
	END

	Select 
		A.IdPermiso_Perfil_Persona as IdPermisoPerfilPersona,
		A.IdPermiso,
		A.FechaCreacion,
		D.Nombre as NombrePermiso,
		B.IdPerfil_Persona as IdPerfilPersona,
		B.IdPerfil,
		E.Nombre as NombrePerfil,
		B.IdPersona,
		F.NombreCompleto as NombrePersona,
		F.Documento,
		F.TipoDocumento,
		SEG.DesencriptaClave(F.Clave) as clave,
		C.IdProyecto_Perfil_Persona as IdProyectoPerfilPersona,
		C.afe,
		G.companyowner,
		G.localname,
		H.Empresa,
		F.Estado
	From AUC.Permiso_Perfil_Persona A 
	Left join AUC.Perfil_Persona B on A.IdPerfil_Persona = B.IdPerfil_Persona 
	Left join AUC.Proyecto_Perfil_Persona C on B.IdPerfil_Persona = C.IdPerfil_Persona 
	Left join CB_MestroProyectos G on C.afe = G.afe 
	Left join AUC.Permiso D on A.IdPermiso = D.IdPermiso 
	Left join AUC.Perfil E on B.IdPerfil = E.IdPerfil 
	Left join GL_MaestroPersonas F on B.IdPersona = F.Persona 
	Join SEG.EmpresaEmpleado H on F.Persona = H.Empleado 
	Where H.Empresa = @Persona 
		and H.Activo = 1 
		and (F.Documento = @Documento or '0' = @Documento) 
		and (F.NombreCompleto like ('%'+@Nombre+'%')) 
		and (G.Afe = @Proyecto or '0' = @Proyecto) 
		and (B.IdPerfil = @Perfil or 0 = @Perfil);
END
GO

IF object_id('AUC.Usp_SaveUsuario') IS NOT NULL
     DROP PROC AUC.Usp_SaveUsuario
GO

CREATE PROCEDURE AUC.Usp_SaveUsuario
	@Documento varchar(50),
	@TipoDocumento char(1),
	@Nombre varchar(200),
	@UsuarioCreacion int,
	@Clave varchar(100)
AS
BEGIN
	DECLARE @MaxPersona int = (select MAX(Persona) + 1 from GL_MaestroPersonas);
    insert into GL_MaestroPersonas(Persona,Documento,TipoDocumento,Estado,NombreCompleto,vc_CodEmpresa,Busqueda,vc_UsuarioCreador,dt_FechaCreacion,Clave) 
	OUTPUT INSERTED.Persona 
	values(@MaxPersona,@Documento,@TipoDocumento,'A',@Nombre,'04000000',@Nombre,@UsuarioCreacion,GETDATE(),SEG.EncriptaClave(@Clave));

END
GO

IF object_id('AUC.Usp_SaveEmpresaEmpleado') IS NOT NULL
     DROP PROC AUC.Usp_SaveEmpresaEmpleado
GO

CREATE PROCEDURE AUC.Usp_SaveEmpresaEmpleado
	@Empresa int,
	@Empleado int,
	@UsuarioCreacion int
AS
BEGIN
	insert into SEG.EmpresaEmpleado(Empresa,Empleado,Activo,IdUsuarioCreacion,FechaCreacion,IdTipoRelacionEmpresaEmpleado) 
	OUTPUT INSERTED.IdEmpresaEmpleado 
	values(@Empresa,@Empleado,1,@UsuarioCreacion,GETDATE(),1);

END
GO

IF object_id('AUC.Usp_SavePerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_SavePerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_SavePerfilUsuario
	@Perfil int,
	@Persona int,
	@UsuarioCreacion int
AS
BEGIN
	DECLARE @MaxPerfilPersona int = (select COALESCE(MAX(IdPerfil_Persona),0) + 1 from AUC.Perfil_Persona);
	insert into AUC.Perfil_Persona(IdPerfil_Persona,IdPerfil,IdPersona,Activo,IdUsuarioCreacion,FechaCreacion) 
	OUTPUT INSERTED.IdPerfil_Persona as IdPerfilPersona 
	values(@MaxPerfilPersona,@Perfil,@Persona,1,@UsuarioCreacion,GETDATE());
END
GO

IF object_id('AUC.Usp_SavePermisoPerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_SavePermisoPerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_SavePermisoPerfilUsuario
	@PerfilPersona int,
	@Permiso int,
	@UsuarioCreacion int
AS
BEGIN
	DECLARE @MaxPermisoPerfilPersona int = (select COALESCE(MAX(IdPermiso_Perfil_Persona),0) + 1 from AUC.Permiso_Perfil_Persona);
	insert into AUC.Permiso_Perfil_Persona(IdPermiso_Perfil_Persona,IdPerfil_Persona,IdPermiso,IdUsuarioCreacion,FechaCreacion) 
	OUTPUT INSERTED.IdPermiso_Perfil_Persona as IdPermisoPerfilPersona 
	values(@MaxPermisoPerfilPersona,@PerfilPersona,@Permiso,@UsuarioCreacion,GETDATE());
END
GO

IF object_id('AUC.Usp_SaveProyectoPerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_SaveProyectoPerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_SaveProyectoPerfilUsuario
	@PerfilPersona int,
	@Proyecto varchar(15),
	@UsuarioCreacion int
AS
BEGIN
	DECLARE @MaxProyectoPerfilPersona int = (select COALESCE(MAX(IdProyecto_Perfil_Persona),0) + 1 from AUC.Proyecto_Perfil_Persona);
	insert into AUC.Proyecto_Perfil_Persona(IdProyecto_Perfil_Persona,IdPerfil_Persona,afe,IdUsuarioCreacion,FechaCreacion) 
	OUTPUT INSERTED.IdProyecto_Perfil_Persona as IdProyectoPerfilPersona 
	values(@MaxProyectoPerfilPersona,@PerfilPersona,@Proyecto,@UsuarioCreacion,GETDATE());
END
GO

IF object_id('AUC.Usp_UpdateUsuario') IS NOT NULL
     DROP PROC AUC.Usp_UpdateUsuario
GO

CREATE PROCEDURE AUC.Usp_UpdateUsuario 
	@Persona int,
	@Documento varchar(50),
	@TipoDocumento char(1),
	@Nombre varchar(200),
	@UsuarioEdicion int,
	@Clave varchar(100)
AS
BEGIN
	Update GL_MaestroPersonas 
	Set Documento = @Documento,
	TipoDocumento = @TipoDocumento,
	NombreCompleto = @Nombre,
	Busqueda = @Nombre,
	Estado = 'A',
	Clave = SEG.EncriptaClave(@Clave),
	vc_UltimoUsuario = @UsuarioEdicion,
	dt_FechaUltimaModif = GETDATE() 
	Where Persona = @Persona;
END
GO

IF object_id('AUC.Usp_UpdatePerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_UpdatePerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_UpdatePerfilUsuario
	@PerfilPersona int,
	@Perfil int,
	@Persona int,
	@UsuarioEdicion int
AS
BEGIN
	Update AUC.Perfil_Persona 
	set IdPerfil = @Perfil, 
	IdPersona = @Persona, 
	Activo = 1, 
	IdUsuarioEdicion = @UsuarioEdicion, 
	FechaEdicion = GETDATE() 
	Where IdPerfil_Persona = @PerfilPersona;
END
GO

IF object_id('AUC.Usp_DeleteProyectoPerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_DeleteProyectoPerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_DeleteProyectoPerfilUsuario
	@PerfilPersona int
AS
BEGIN
	Delete from AUC.Proyecto_Perfil_Persona 
	Where IdPerfil_Persona = @PerfilPersona;
END
GO

IF object_id('AUC.Usp_DeletePermisoPerfilUsuario') IS NOT NULL
     DROP PROC AUC.Usp_DeletePermisoPerfilUsuario
GO

CREATE PROCEDURE AUC.Usp_DeletePermisoPerfilUsuario
	@PerfilPersona int
AS
BEGIN
	 Delete from AUC.Permiso_Perfil_Persona 
	 Where IdPerfil_Persona = @PerfilPersona;
END
GO

/**

	MODULO PROGRAMACION DE UNIDADES

**/

IF object_id('SEG.USP_PERSONAALMACEN_LISTAR_X_PERSONA') IS NOT NULL
     DROP PROC SEG.USP_PERSONAALMACEN_LISTAR_X_PERSONA
GO

CREATE OR ALTER PROCEDURE SEG.USP_PERSONAALMACEN_LISTAR_X_PERSONA
(
	@IdPersona int
)
AS
BEGIN
	SELECT
		PA.IdPersonaAlmacen,
		PA.IdPersona,
		PA.AlmacenCodigo,
		RTRIM(LTRIM(REPLACE(MA.DescripcionLocal, 'LAYHER', ''))) AlmacenNombre
	FROM SEG.PersonaAlmacen PA
	INNER JOIN LG_MaestroAlmacen MA ON PA.AlmacenCodigo = MA.AlmacenCodigo
	WHERE (PA.IdPersona = @IdPersona or 0 = @IdPersona)
END
GO

IF object_id('AUC.Usp_ListaProyectoCliente') IS NOT NULL
     DROP PROC AUC.Usp_ListaProyectoCliente
GO

CREATE PROCEDURE [AUC].[Usp_ListaProyectoCliente]
	@Proyecto varchar(15),
	@OffSet int,
	@Fetch int
AS
BEGIN
	Select A.afe,A.companyowner,A.localname,B.NombreCompleto as NombreCliente
	From CB_MestroProyectos A 
	inner join GL_MaestroPersonas B on A.Client_ID = B.Persona
	Where (A.afe = @Proyecto or '0' = @Proyecto) 
	ORDER BY A.afe 
	OFFSET @OffSet ROWS 
	FETCH NEXT @Fetch ROWS ONLY;
END
GO

IF object_id('PU.Usp_SaveProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionUnidad
GO

CREATE PROCEDURE PU.Usp_SaveProgramacionUnidad
	@NumeroProgramacion varchar(10),
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
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.ProgramacionUnidad(NumeroProgramacion,IdProyecto,FechaInicio,FechaFin,IdEstado,IdTipoUnidad,IdMetrajeTrailer,Especificacion,IdAlmacen,Tonelada,Correo,IdUsuarioCreacion,FechaCreacion) 
	OUTPUT inserted.ID AS IdProgramacionUnidad
	values(@NumeroProgramacion,@Proyecto,@FechaInicio,@FechaFin,@Estado,@TipoUnidad,@MetrajeTrailer,@Especificacion,@Almacen,@Tonelada,@Correo,@UsuarioCreacion,GETDATE());
END
GO

IF object_id('PU.Usp_ListProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionUnidad
GO

CREATE PROCEDURE PU.Usp_ListProgramacionUnidad
	@ProgramacionUnidad int,
	@NumeroProgramacion varchar(10),
	@Proyecto varchar(15),
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@Almacen varchar(10)
AS
BEGIN
	select 
			B.Nombre as nombreEstado,C.Metro as Longitud,D.Nombre as nombreUnidad,F.DescripcionLocal,A.* 
	from PU.ProgramacionUnidad A 
	inner join PU.TipoEstado B on A.IdEstado = B.Id
	inner join PU.MetrajeTrailer C on A.IdMetrajeTrailer = C.Id 
	inner join PU.TipoUnidad D on A.IdTipoUnidad = D.Id
	inner join LG_MaestroAlmacen F on A.IdAlmacen = F.AlmacenCodigo
	left join PU.Formulario G on A.IdFormulario = G.Id
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
		  and (A.NumeroProgramacion = @NumeroProgramacion or '0' = @NumeroProgramacion)
		  and (A.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (A.FechaInicio between @FechaInicio and @FechaFin)
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)		  
END
GO