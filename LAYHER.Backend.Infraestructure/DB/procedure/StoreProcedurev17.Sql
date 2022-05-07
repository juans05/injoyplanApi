IF object_id('PU.Usp_SaveFormulario') IS NOT NULL
     DROP PROC PU.Usp_SaveFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_SaveFormulario]
	@ProgramacionUnidad int,
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
	@NombreTransportista varchar(50),
	@SctrTransportista varchar(20),
	@TelefonoTransportista varchar(9),
	@LicenciaTransportista varchar(20),
	@IdTipoDocumentoEncargado char(1),
	@DocumentoEncargado varchar(50),
	@NombreEncargado varchar(50),
	@SctrEncargado varchar(20),
	@Consideracion text,
	@UsuarioCreacion int
AS
BEGIN
		IF EXISTS(Select * from PU.ProgramacionUnidad where Id = @ProgramacionUnidad)
		BEGIN
			insert into PU.Formulario(EsTransporteNacional,
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
											  FechaCreacion) 
			OUTPUT inserted.ID AS IdFormulario
			values(@EsTransporteNacional,
				   @EsTransportistaEncargado,
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

IF object_id('AUC.Usp_ListaPermisos') IS NOT NULL
     DROP PROC AUC.Usp_ListaPermisos
GO

CREATE OR ALTER PROCEDURE AUC.Usp_ListaPermisos
	@Permiso int
AS
BEGIN
	select IdPermiso, Nombre, Activo 
	From AUC.Permiso 
	Where (IdPermiso = @Permiso or 0 = @Permiso) and Activo = 1;
END
GO