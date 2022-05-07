/**

	MODULO PROGRAMACION DE UNIDADES

**/
IF object_id('PU.Usp_ListaCotizacion') IS NOT NULL
     DROP PROC PU.Usp_ListaCotizacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaCotizacion]
	@Proyecto varchar(15)
AS
BEGIN
	SELECT DISTINCT RTRIM(LTRIM(vc_idcotizacion)) as IdCotizacion 
	FROM CT_Cotizacion_Control_Cantidades 
	WHERE (vc_idproyecto = @Proyecto or '0' = @Proyecto) 
	AND dbl_CantidadDespachada<dbl_CantidadPedida 
	ORDER BY IdCotizacion 
END

IF object_id('SEG.Usp_ListaProyectoCliente') IS NOT NULL
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

CREATE OR ALTER PROCEDURE [AUC].[Usp_ListaProyectoCliente]
	@Proyecto varchar(15),
	@OffSet int,
	@Fetch int
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
	ORDER BY A.afe 
	OFFSET @OffSet ROWS 
	FETCH NEXT @Fetch ROWS ONLY;
END
GO

IF object_id('PU.Usp_SaveProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionUnidad
GO

CREATE OR ALTER PROCEDURE PU.Usp_SaveProgramacionUnidad
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
	@FinAlquiler datetime,
	@FechaRevision datetime,
	@RevisionHoraInicio datetime,
	@RevisionHoraFin datetime,
	@Cotizacion varchar(50),
	@UsuarioCreacion int
AS
BEGIN
	insert into PU.ProgramacionUnidad(NumeroProgramacion,
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
									  IdUsuarioCreacion,
									  IdCotizacion,
									  FechaCreacion) 
	OUTPUT inserted.ID AS IdProgramacionUnidad
	values(@NumeroProgramacion,
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
		   @UsuarioCreacion,
		   GETDATE());
END
GO

IF object_id('PU.Usp_ListProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionUnidad
GO

CREATE OR ALTER   PROCEDURE [PU].[Usp_ListProgramacionUnidad]
	@ProgramacionUnidad int,
	@NumeroProgramacion varchar(10),
	@Proyecto varchar(15),
	@FechaInicio datetime,
	@FechaFin	datetime,
	@Estado int,
	@Almacen varchar(10),
	@Persona int
AS
BEGIN
	select 
			B.Nombre as nombreEstado,C.Metro as Longitud,D.Nombre as nombreUnidad,F.DescripcionLocal,I.NombreCompleto,A.* 
	from PU.ProgramacionUnidad A
	left join PU.Formulario J on A.IdFormulario = J.Id
	left join PU.TipoEstado B on A.IdEstado = B.Id
	left join PU.MetrajeTrailer C on A.IdMetrajeTrailer = C.Id 
	left join PU.TipoUnidad D on A.IdTipoUnidad = D.Id
	left join LG_MaestroAlmacen F on A.IdAlmacen = F.AlmacenCodigo
	left join PU.Formulario G on A.IdFormulario = G.Id
	left join CB_MestroProyectos H on A.IdProyecto = H.afe
	left join GL_MaestroPersonas I on H.Client_ID = I.Persona 
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
		  and (A.NumeroProgramacion = @NumeroProgramacion or '0' = @NumeroProgramacion)
		  and (A.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (A.FechaInicio between @FechaInicio and @FechaFin)
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
END
GO

IF object_id('PU.Usp_SaveFormulario') IS NOT NULL
     DROP PROC PU.Usp_SaveFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_SaveFormulario]
	--[Id] int NOT NULL IDENTITY (1, 1),
	@ProgramacionUnidad int,
	@EsTransporteNacional bit,
	@NumeroDocumento varchar(20),
	@RazonSocial varchar(50),
	@ModeloVehiculo varchar(30),
	@PlacaTracto varchar(20),
	@PlacaCarreta varchar(20),
	@TelefonoContacto varchar(9),
	@IdTipoDocumentoTransportista char(1),
	@DocumentoTransportista varchar(50),
	@NombreTransportista varchar(30),
	@SctrTransportista varchar(20),
	@TelefonoTransportista varchar(9),
	@LicenciaTransportista varchar(20),
	@IdTipoDocumentoEncargado char(1),
	@DocumentoEncargado varchar(50),
	@NombreEncargado varchar(30),
	@SctrEncargado varchar(20),
	@Consideracion text,
	@UsuarioCreacion int
AS
BEGIN
		IF EXISTS(Select * from PU.ProgramacionUnidad where Id = @ProgramacionUnidad)
		BEGIN
			insert into PU.Formulario(EsTransporteNacional,
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
											  FechaCreacion) 
			OUTPUT inserted.ID AS IdFormulario
			values(@EsTransporteNacional,
				   @NumeroDocumento,
				   @RazonSocial,
				   @ModeloVehiculo,
				   @PlacaTracto,
				   @PlacaCarreta,
				   @TelefonoContacto,
				   @IdTipoDocumentoTransportista,
				   @DocumentoTransportista,
				   @NombreTransportista,
				   @SctrTransportista,
				   @TelefonoTransportista,
				   @LicenciaTransportista,
				   @IdTipoDocumentoEncargado,
				   @DocumentoEncargado,
				   @NombreEncargado,
				   @SctrEncargado,
				   @Consideracion,
				   @UsuarioCreacion,
				   GETDATE());
		END
END
GO

IF object_id('PU.Usp_ListaDocumentosxConsiderar') IS NOT NULL
     DROP PROC PU.Usp_ListaDocumentosxConsiderar
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaDocumentosxConsiderar]
	@Consideracion int
AS
BEGIN
	select 
			A.Id as IdConsideracion,
			A.Nombre as NombreConsideracion
	from PU.Consideracion A
	where (A.id = @Consideracion or 0 = @Consideracion)
		  and A.activo = 1
END
GO

IF object_id('PU.Usp_ListaDocumentosxAdjuntar') IS NOT NULL
     DROP PROC PU.Usp_ListaDocumentosxAdjuntar
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaDocumentosxAdjuntar]
	@Adjunto int
AS
BEGIN
	select 
			A.Id as IdAdjunto,
			A.Nombre as NombreAdjunto
	from PU.Adjunto A
	where (A.id = @Adjunto or 0 = @Adjunto)
		  and A.activo = 1
END
GO