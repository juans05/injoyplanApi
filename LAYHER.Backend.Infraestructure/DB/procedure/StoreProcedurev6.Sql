IF object_id('PU.Usp_GetFormularioProgramacion') IS NOT NULL
     DROP PROC PU.Usp_GetFormularioProgramacion
GO

CREATE OR ALTER PROCEDURE PU.Usp_GetFormularioProgramacion (  
@IdFormulario INT  
)  
AS  
BEGIN  
 SELECT Id as IdFormulario,
EsTransporteNacional,
NumeroDocumento,
RazonSocial,
ModeloVehiculo,
PlacaTracto,
PlacaCarreta,
TelefonoContacto,
IdTipoDocumentoTransportista,
DocumentoTransportista,
NombreTransportista,
SctrTransportista,
TelefonoTransportista,
LicenciaTransportista,
IdTipoDocumentoEncargado,
DocumentoEncargado,
NombreEncargado,
SctrEncargado,
Consideracion,
IdUsuarioCreacion,
FechaCreacion,
IdUsuarioEdicion,
FechaEdicion FROM PU.Formulario WHERE Id=@IdFormulario;  
 SELECT * FROM PU.AdjuntoFormulario WHERE IdFormulario=@IdFormulario;  
 SELECT * FROM PU.FormularioConsideracion WHERE IdFormulario=@IdFormulario; 
 SELECT Id as IdArchivoAdjunto,
		IdFormulario,
		Nombre,
		Ruta
 FROM PU.ArchivoAdjunto WHERE IdFormulario = @IdFormulario;
END  
GO

IF object_id('COMUN.Usp_ListaPersonas') IS NOT NULL
     DROP PROC COMUN.Usp_ListaPersonas
GO

CREATE OR ALTER PROCEDURE COMUN.Usp_ListaPersonas
	@Persona int,
	@OffSet int,
	@Fetch int
AS
BEGIN
		Select Persona,Documento,TipoDocumento,Estado,NombreCompleto 
		from GL_MaestroPersonas 
		where (Persona = @Persona or 0 = @Persona) and Estado = 'A' 
		ORDER BY Persona 
		OFFSET @OffSet ROWS FETCH NEXT @Fetch ROWS ONLY
END
GO

IF object_id('PU.Usp_ListaAdjuntoFormulario') IS NOT NULL
     DROP PROC PU.Usp_ListaAdjuntoFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaAdjuntoFormulario]
	@ArchivoAdjunto int
AS
BEGIN
	select 
			Id as IdAdjuntoFormulario,
			IdFormulario,
			Nombre,
			Ruta
	from PU.ArchivoAdjunto
	where (Id = @ArchivoAdjunto or 0 = @ArchivoAdjunto)
END
GO

IF object_id('PU.Usp_SaveAdjuntoFormulario') IS NOT NULL
     DROP PROC PU.Usp_SaveAdjuntoFormulario
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_SaveAdjuntoFormulario]
	@Formulario int,
	@Adjunto int,
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.AdjuntoFormulario(IdFormulario,
									  IdAdjunto,
									  IdUsuarioCreacion,
									  FechaCreacion) 
	OUTPUT inserted.ID AS IdAdjuntoFormulario
	values(@Formulario,
		   @Adjunto,
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_ListBandejaCliente') IS NOT NULL
     DROP PROC PU.Usp_ListBandejaCliente
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListBandejaCliente]
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
			tab.* 
	from (select A.Id as IdProgramacionUnidad,A.*
				   from PU.ProgramacionUnidad A 
				   where A.IdTipoProgramacion in (1,2)) tab
	left join PU.Formulario J on tab.IdFormulario = J.Id
	left join PU.TipoEstado B on tab.IdEstado = B.Id
	left join PU.MetrajeTrailer C on tab.IdMetrajeTrailer = C.Id
	left join PU.TipoUnidad D on tab.IdTipoUnidad = D.Id
	left join LG_MaestroAlmacen F on tab.IdAlmacen = F.AlmacenCodigo 
	left join LG_MaestroAlmacen M on tab.IdAlmacenDestino = M.AlmacenCodigo 
	left join PU.Formulario G on tab.IdFormulario = G.Id 
	left join CB_MestroProyectos H on tab.IdProyecto = H.afe
	left join GL_MaestroPersonas I on H.Client_ID = I.Persona
	left join PU.TipoProgramacion K on tab.IdTipoProgramacion = k.Id
	left join GL_MaestroPersonas L on tab.IdUsuarioCreacion = L.Persona
	where (tab.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad) 
		  and (tab.IdTipoProgramacion = @TipoProgramacion or 0 = @TipoProgramacion)
		  and (tab.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (tab.FechaInicio between @FechaInicio and @FechaFin)
		  and (tab.IdEstado = @Estado or 0 = @Estado)
		  and (tab.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and tab.Activo = 1;
END
GO

IF object_id('PU.Usp_ListBandejaProveedor') IS NOT NULL
     DROP PROC PU.Usp_ListBandejaProveedor
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListBandejaProveedor]
	@ProgramacionUnidad int,
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
	left join LG_MaestroAlmacen M on A.IdAlmacenDestino = M.AlmacenCodigo
	left join PU.Formulario G on A.IdFormulario = G.Id
	left join CB_MestroProyectos H on A.IdProyecto = H.afe
	left join GL_MaestroPersonas I on H.Client_ID = I.Persona
	left join PU.TipoProgramacion K on A.IdTipoProgramacion = k.Id
	left join GL_MaestroPersonas L on A.IdUsuarioCreacion = L.Persona
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
		  and (A.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (A.FechaInicio between @FechaInicio and @FechaFin)
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and A.Activo = 1
		  and A.IdTipoProgramacion = 3;
END
GO

IF object_id('PU.Usp_SaveProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionUnidad
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_SaveProgramacionUnidad]
	@TipoProgramacion int,
	@Proyecto varchar(15),
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@TipoUnidad int,
	@MetrajeTrailer int,
	@Especificacion varchar(50),
	@Almacen varchar(10),
	@AlmacenDestino varchar(10),
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
									  IdAlmacenDestino,
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
		   @AlmacenDestino,
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
    left join LG_MaestroAlmacen M on A.IdAlmacenDestino = M.AlmacenCodigo
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

IF object_id('PU.Usp_SaveArchivoAdjunto') IS NOT NULL
     DROP PROC PU.Usp_SaveArchivoAdjunto
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_SaveArchivoAdjunto]
	@Formulario int,
	@Nombre varchar(100),
	@Ruta varchar(200),
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.ArchivoAdjunto(IdFormulario,
									  Nombre,
									  Ruta,
									  IdUsuarioCreacion,
									  FechaCreacion) 
	OUTPUT inserted.ID AS IdArchivoAdjunto
	values(@Formulario,
		   @Nombre,
		   @Ruta,
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_EliminarFormulario') IS NOT NULL
     DROP PROC PU.Usp_EliminarFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarFormulario]
	@Formulario int
AS
BEGIN
	Declare @Response bit 
	SET @Response = 0;
	--Se elimina la programacion si existe
	IF EXISTS (SELECT Id from PU.Formulario where Id = @Formulario)
	  BEGIN
	      DELETE FROM PU.Formulario WHERE Id = @Formulario
		  SET @Response = 1;
	  END	  
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_EliminarPogramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_EliminarPogramacionUnidad
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarPogramacionUnidad]
	@Programacion int
AS
BEGIN
	Declare @Response bit 
	SET @Response = 0;
	--Se elimina la programacion si existe
	IF EXISTS (SELECT Id from PU.ProgramacionUnidad where Id = @Programacion)--@Id <> 0
	  BEGIN
	      --Se elimina el formulario si existe.
		  IF EXISTS (SELECT IdFormulario FROM PU.ProgramacionUnidad WHERE Id = @Programacion)
		  BEGIN
			DECLARE @IdFormulario int
			SET @IdFormulario = (SELECT IdFormulario FROM PU.ProgramacionUnidad WHERE Id = @Programacion)
			DELETE FROM PU.AdjuntoFormulario Where IdFormulario = @IdFormulario
			DELETE FROM PU.FormularioConsideracion Where IdFormulario = @IdFormulario
			DELETE FROM PU.ArchivoAdjunto Where IdFormulario = @IdFormulario
			DELETE FROM PU.Formulario WHERE Id = @IdFormulario
		  END
		  --Se elimina la programacion de tiempo si existe.
		  IF EXISTS (SELECT Id FROM PU.ProgramacionTiempo WHERE IdProgramacionUnidad = @Programacion)
		  BEGIN
			DECLARE @IdProgramacionTiempo int
			SET @IdProgramacionTiempo = (SELECT Id FROM PU.ProgramacionTiempo WHERE IdProgramacionUnidad = @Programacion)
			DELETE FROM PU.ProgramacionTiempo WHERE Id = @IdProgramacionTiempo
		  END
	      DELETE FROM PU.ProgramacionUnidad WHERE Id = @Programacion
		  SET @Response = 1;
	  END	  
	SELECT	'Return' = @Response
END
GO