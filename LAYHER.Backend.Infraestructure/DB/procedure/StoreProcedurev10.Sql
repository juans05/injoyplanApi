IF object_id('PU.Usp_ObtenerAlmacenPorIdProgramacionUnidad') IS NOT NULL
     DROP PROC PU.Usp_ObtenerAlmacenPorIdProgramacionUnidad
GO

CREATE PROCEDURE [PU].[Usp_ObtenerAlmacenPorIdProgramacionUnidad]
 @IdProgramacionUnidad INT
AS  
BEGIN  
	SELECT MA.* FROM LG_MaestroAlmacen MA
	INNER JOIN PU.ProgramacionUnidad PU ON PU.IdAlmacen = MA.AlmacenCodigo
	WHERE Id=@IdProgramacionUnidad
END  
GO

IF object_id('PU.Usp_UpdateFormulario') IS NOT NULL
     DROP PROC PU.Usp_UpdateFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_UpdateFormulario]
	@Formulario int,
	@EsTransporteNacional bit,
	@EsTransportistaEncargado bit,
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
	@UsuarioEdicion int
AS
BEGIN
		IF EXISTS(Select * from PU.Formulario where Id = @Formulario)
		BEGIN
			update PU.Formulario 
						set EsTransporteNacional = @EsTransporteNacional,
							EsTransportistaEncargado = @EsTransportistaEncargado,
											  NumeroDocumento = @NumeroDocumento,
											  RazonSocial = @RazonSocial,
											  ModeloVehiculo = @ModeloVehiculo,
											  PlacaTracto = @PlacaTracto,
											  PlacaCarreta = @PlacaCarreta,
											  TelefonoContacto = @TelefonoContacto,
											  IdTipoDocumentoTransportista = @IdTipoDocumentoTransportista,
											  DocumentoTransportista = @DocumentoTransportista,
											  NombreTransportista = @NombreTransportista,
											  SctrTransportista = @SctrTransportista,
											  TelefonoTransportista = @TelefonoTransportista,
											  LicenciaTransportista = @LicenciaTransportista,
											  IdTipoDocumentoEncargado = @IdTipoDocumentoEncargado,
											  DocumentoEncargado = @DocumentoEncargado,
											  NombreEncargado = @NombreEncargado,
											  SctrEncargado = @SctrEncargado,
											  Consideracion = @Consideracion,
											  IdUsuarioCreacion = @UsuarioEdicion,
											  FechaCreacion = GETDATE() 
		END
END
GO

IF object_id('PU.Usp_EliminarAdjuntoFormulario') IS NOT NULL
     DROP PROC PU.Usp_EliminarAdjuntoFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarAdjuntoFormulario]
	@Formulario int
AS
BEGIN
	Declare @Response bit 
	SET @Response = 1;
	--Se elimina
    DELETE FROM PU.AdjuntoFormulario WHERE IdFormulario = @Formulario
	IF EXISTS (SELECT * from PU.AdjuntoFormulario where IdFormulario = @Formulario)
	  BEGIN
		  SET @Response = 0;
	  END	  
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_EliminarArchivoAdjunto') IS NOT NULL
     DROP PROC PU.Usp_EliminarArchivoAdjunto
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarArchivoAdjunto]
	@Formulario int
AS
BEGIN
	Declare @Response bit 
	SET @Response = 1;
	--Se elimina
    DELETE FROM PU.ArchivoAdjunto WHERE IdFormulario = @Formulario
	IF EXISTS (SELECT * from PU.ArchivoAdjunto where IdFormulario = @Formulario)
	  BEGIN
		  SET @Response = 0;
	  END	  
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_EliminarFormularioConsideracion') IS NOT NULL
     DROP PROC PU.Usp_EliminarFormularioConsideracion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_EliminarFormularioConsideracion]
	@Formulario int
AS
BEGIN
	Declare @Response bit 
	SET @Response = 1;
	--Se elimina
    DELETE FROM PU.FormularioConsideracion WHERE IdFormulario = @Formulario
	--Validamos si aun existe
	IF EXISTS (SELECT * from PU.FormularioConsideracion where IdFormulario = @Formulario)
	  BEGIN
	      DELETE FROM PU.FormularioConsideracion WHERE IdFormulario = @Formulario
		  SET @Response = 0;
	  END	
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_ValidaFormularioEdicion') IS NOT NULL
     DROP PROC PU.Usp_ValidaFormularioEdicion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ValidaFormularioEdicion]
	@Formulario int
AS
BEGIN
	Declare @Fecha datetime
	SET @Fecha = GETDATE();
	Declare @Response bit 
	SET @Response = 0;

	IF EXISTS (SELECT * from PU.ProgramacionUnidad where IdFormulario = @Formulario 
														 and FechaFin > @Fecha)
	  BEGIN
		  SET @Response = 1;
	  END	  
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_ListaArchivoAdjunto') IS NOT NULL
     DROP PROC PU.Usp_ListaArchivoAdjunto
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaArchivoAdjunto] 
	@Formulario int
AS
BEGIN
	select * from PU.ArchivoAdjunto where IdFormulario = @Formulario
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
 SELECT Id as IdFormulario,
EsTransporteNacional,
EsTransportistaEncargado,
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