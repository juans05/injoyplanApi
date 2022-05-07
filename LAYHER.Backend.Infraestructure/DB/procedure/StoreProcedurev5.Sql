IF object_id('AUC.Usp_ProgramacionClienteDatos') IS NOT NULL
     DROP PROC AUC.Usp_ProgramacionClienteDatos
GO

CREATE OR ALTER PROCEDURE [AUC].[Usp_ProgramacionClienteDatos]
	@ProgramacionUnidad int
AS
BEGIN
		select
				D.Persona as Persona,
				D.CorreoElectronico as Correo,
				D.NombreCompleto as NombreCompleto
		from PU.ProgramacionUnidad A
		join CB_MestroProyectos C on A.IdProyecto = C.afe
		join GL_MaestroPersonas D on C.Client_ID = D.Persona
		where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
END

IF object_id('PU.Usp_SaveProgramacionTiempo') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionTiempo
GO

IF object_id('AUC.Usp_SaveProgramacionTiempo') IS NOT NULL
     DROP PROC AUC.Usp_SaveProgramacionTiempo
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_SaveProgramacionTiempo]
	@ProgramacionUnidad int,
	@TipoMontacarga int,
	@DescargaInicio datetime,
	@DescargaFin datetime,
	@FechaRevision datetime,
	@InicioRevision datetime,
	@FinRevision datetime,
	@FechaRevision2 datetime,
	@InicioRevision2 datetime,
	@FinRevision2 datetime,
	@RevisionMayorUno bit,
	@UsuarioCreacion int
AS
BEGIN
	IF (@RevisionMayorUno = 1)
		BEGIN 
			insert into PU.ProgramacionTiempo(IdProgramacionUnidad,
											  IdTipoMontaCarga,
											  DescargaInicio,
											  DescargaFin,
											  FechaRevision,
											  InicioRevision,
											  FinRevision,
											  FechaRevision2,
											  InicioRevision2,
											  FinRevision2,
											  IdUsuarioCreacion,
											  FechaCreacion) 
			OUTPUT inserted.ID AS IdProgramacionTiempo
			values(@ProgramacionUnidad,
				   @TipoMontacarga,
				   @DescargaInicio,
				   @DescargaFin,
				   @FechaRevision,
				   @InicioRevision,
				   @FinRevision,
				   @FechaRevision2,
				   @InicioRevision2,
				   @FinRevision2,
				   @UsuarioCreacion,
				   GETDATE());
		END
	ELSE
		BEGIN
			insert into PU.ProgramacionTiempo(IdProgramacionUnidad,
											  IdTipoMontaCarga,
											  DescargaInicio,
											  DescargaFin,
											  FechaRevision,
											  InicioRevision,
											  FinRevision,
											  IdUsuarioCreacion,
											  FechaCreacion)
			OUTPUT inserted.ID AS IdProgramacionTiempo								  
			values(@ProgramacionUnidad,
				   @TipoMontacarga,
				   @DescargaInicio,
				   @DescargaFin,
				   @FechaRevision,
				   @InicioRevision,
				   @FinRevision,
				   @UsuarioCreacion,
				   GETDATE());
		END
END
GO

IF object_id('AUC.USP_PERSONAALMACEN_LISTAR_X_PERSONA') IS NOT NULL
     DROP PROC AUC.USP_PERSONAALMACEN_LISTAR_X_PERSONA
GO

CREATE OR ALTER PROCEDURE [SEG].[USP_PERSONAALMACEN_LISTAR_X_PERSONA]
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
	WHERE PA.IdPersona = @IdPersona
END
GO

IF object_id('AUC.USP_LISTARALMACENES') IS NOT NULL
     DROP PROC AUC.USP_LISTARALMACENES
GO

CREATE OR ALTER PROCEDURE [COMUN].[USP_LISTARALMACENES]
AS
BEGIN
	SELECT Distinct
		PA.AlmacenCodigo,
		RTRIM(LTRIM(REPLACE(MA.DescripcionLocal, 'LAYHER', ''))) AlmacenNombre
	FROM SEG.PersonaAlmacen PA
	INNER JOIN LG_MaestroAlmacen MA ON PA.AlmacenCodigo = MA.AlmacenCodigo
END
GO

IF object_id('PU.Usp_ListaFormulario') IS NOT NULL
     DROP PROC PU.Usp_ListaFormulario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaFormulario]
	@Formulario int
AS
BEGIN
	select 
			A.Id as IdFormulario,
			A.EsTransporteNacional,
			A.NumeroDocumento,
			A.RazonSocial,
			A.ModeloVehiculo,
			A.PlacaTracto,
			A.PlacaCarreta,
			A.TelefonoContacto,
			A.IdTipoDocumentoTransportista,
			D.Descripcion as NombreTipoDocumentoTransportista,
			A.DocumentoTransportista,
			A.NombreTransportista,
			A.SctrTransportista,
			A.TelefonoTransportista,
			A.LicenciaTransportista,
			A.IdTipoDocumentoEncargado,
			E.Descripcion as NombreTipoDocumentoEncargado,
			A.DocumentoEncargado,
			A.NombreEncargado,
			A.SctrEncargado,
			A.Consideracion,
			B.IdConsideracion,
			F.Nombre as NombreConsideracion,
			C.IdAdjunto,
			G.Nombre as NombreAdjunto,
			C.Nombre as NombreDocumento,
			C.Ruta as RutaAdjunto
	from PU.Formulario A
	left join PU.FormularioConsideracion B on A.Id = B.IdFormulario
	left join PU.Consideracion F on B.IdConsideracion = F.Id
	left join PU.AdjuntoFormulario C on A.Id = C.IdFormulario
	left join PU.Adjunto G on C.IdAdjunto = G.Id
	left join Comun.TipoDocumentoIdentidad D on A.IdTipoDocumentoTransportista = D.TipoDocumento
	left join Comun.TipoDocumentoIdentidad E on A.IdTipoDocumentoEncargado = E.TipoDocumento
	where (A.Id = @Formulario or 0 = @Formulario)
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

IF object_id('PU.Usp_ProgramacionUnidadPorId') IS NOT NULL
     DROP PROC PU.Usp_ProgramacionUnidadPorId
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ProgramacionUnidadPorId]
	@ProgramacionUnidad int
AS
BEGIN
	SELECT 
			A.Id as IdProgramacionUnidad,
			A.IdFormulario as IdFormulario,
			B.Id as IdProgramacionTiempo
	FROM PU.ProgramacionUnidad A
	left join PU.ProgramacionTiempo B On A.Id = B.IdProgramacionUnidad
	WHERE (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
END
GO