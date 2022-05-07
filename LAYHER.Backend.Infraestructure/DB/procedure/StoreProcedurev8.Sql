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

IF object_id('PU.Usp_ListaProgramacionObservacionDetalle') IS NOT NULL
     DROP PROC PU.Usp_ListaProgramacionObservacionDetalle
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaProgramacionObservacionDetalle]
	@ProgramacionUnidad int
AS
BEGIN
	select 
			A.IdTipoProgramacion,F.Nombre as NombreTipoProgramacion,
			A.FechaInicio,A.FechaFin,E.NombreCompleto,B.EsTransporteNacional,B.EsTransportistaEncargado,B.RazonSocial,
			B.NombreTransportista,B.DocumentoTransportista,B.LicenciaTransportista,B.PlacaCarreta,B.PlacaTracto,
			B.NombreEncargado,B.DocumentoEncargado,
			A.Observacion
	from PU.ProgramacionUnidad A
	left join PU.Formulario B on A.IdFormulario = B.Id
	left join CB_MestroProyectos D on A.IdProyecto = D.afe
	left join GL_MaestroPersonas E on D.Client_ID = E.Persona
	left join PU.TipoProgramacion F on A.IdTipoProgramacion = F.Id
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad);
END
GO

--Se agrego el dia 01062021
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
			H.localname as NombreProyecto,A.Id as IdProgramacionUnidad,A.* 
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
		  and (A.FechaInicio between @FechaInicio and DATEADD(day,1,@FechaFin))
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and A.Activo = 1;
END
GO

--Se agrego el dia 02062021
IF object_id('AUC.Usp_ListaPerfilesxDocumento') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilesxDocumento
GO

CREATE OR ALTER PROCEDURE [AUC].[Usp_ListaPerfilesxDocumento]
	@Documento varchar(50)
AS
BEGIN
	Declare @Respuesta bit
	set @Respuesta = 0
	Declare @Persona int
	set @Persona = (select Persona from GL_MaestroPersonas where Documento = @Documento);
	Declare @PerfilArea int
	set @PerfilArea = (select IdPerfilArea from SEG.UsuarioPerfilArea where Usuario = @Persona)
	
	IF exists(select IdPerfil from SEG.PerfilArea where IdPerfilArea = @PerfilArea)
		BEGIN
			Select D.IdPerfil, D.Nombre 
			From GL_MaestroPersonas A 
			Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
			Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
			Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
			Where A.Documento = @Documento;

			set @Respuesta = 1
		END
	IF (@Respuesta = 0)
		BEGIN
			Declare @PersonaCliente int
			set @PersonaCliente = (select vc_UsuarioCreador from GL_MaestroPersonas where Documento = @Documento)
			Declare @PerfilAreaEmpresa int
			set @PerfilAreaEmpresa = (select IdPerfilArea from SEG.UsuarioPerfilArea where Usuario = @PersonaCliente)
			Declare @PerfilEmpresa int
			set @PerfilEmpresa = (select IdPerfil from SEG.PerfilArea where IdPerfilArea = @PerfilAreaEmpresa and IdPerfil = 91)
			Declare @PerfilAUC int
			set @PerfilAUC = (select IdPerfil from AUC.Perfil_Persona where IdPersona = @Persona and Activo = 1)
			
			IF (@PerfilEmpresa = 91 and @PerfilAUC = 5)
				BEGIN
					Select -91 as IdPerfil, D.Nombre 
					From GL_MaestroPersonas A 
					Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
					Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
					Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
					Where A.Persona = @PersonaCliente;
				END
			ELSE
				BEGIN
					Select D.IdPerfil, D.Nombre 
					From GL_MaestroPersonas A 
					Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
					Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
					Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
					Where A.Documento = @Documento;
				END
		END
END
GO

IF object_id('AUC.Usp_ValidaPersona') IS NOT NULL
     DROP PROC AUC.Usp_ValidaPersona
GO

CREATE OR ALTER PROCEDURE [AUC].[Usp_ValidaPersona]
		@Documento varchar(50),
		@Password varchar(100),
		@Perfil int
AS
BEGIN	
		Declare @Respuesta bit
		set @Respuesta = 0

		IF (@Perfil = -91)
			BEGIN
					IF EXISTS(select * from GL_MaestroPersonas where Documento = @Documento and Clave = SEG.EncriptaClave(@Password))
						BEGIN
							Declare @PersonaCliente int
							set @PersonaCliente = (select vc_UsuarioCreador from GL_MaestroPersonas where Documento = @Documento)
							Select A.Persona,A.NombreCompleto 
							From GL_MaestroPersonas A 
							Where A.Persona = @PersonaCliente

							set @Respuesta = 1
						END
			END
		IF (@Respuesta = 0)
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
END
GO

--Se agrego el dia 04062021
IF object_id('PU.Usp_ValidaCruceProgramacion') IS NOT NULL
     DROP PROC PU.Usp_ValidaCruceProgramacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ValidaCruceProgramacion] --1,'A_LAYHER01','2021-05-20 14:12:00'
	@TipoProgramacion int,
	@Almacen varchar(10),
	@Fecha Datetime
AS
BEGIN
		select Id as IdProgramacionUnidad,* from PU.ProgramacionUnidad 
			  where FechaInicio <= @Fecha and FechaFin >= @Fecha and 
				        IdAlmacen = @Almacen and IdTipoProgramacion = @TipoProgramacion
END
GO
