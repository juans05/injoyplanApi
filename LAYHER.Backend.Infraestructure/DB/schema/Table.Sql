IF NOT EXISTS ( SELECT * FROM    sys.schemas WHERE   name = N'AUC' )
    EXEC('CREATE SCHEMA [AUC]');
GO

IF NOT EXISTS ( SELECT * FROM    sys.schemas WHERE   name = N'PU' )
    EXEC('CREATE SCHEMA [PU]');
GO

--09032021
CREATE TABLE [AUC].[Perfil]
(
	[IdPerfil] int NOT NULL,
	[Nombre] varchar(50) NOT NULL,
	[Activo] bit NOT NULL,
	[IdUsuarioCreacion] int NOT NULL,
	[FechaCreacion] datetime NOT NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(1,'Responsable de Proyectos',1,1,GETDATE());
insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(2,'Contable',1,1,GETDATE());
insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(3,'Ingenieria',1,1,GETDATE());
insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(4,'Operaciones',1,1,GETDATE());
insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(5,'Administrador',1,1,GETDATE());
insert into AUC.Perfil(IdPerfil,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(6,'Otros',1,1,GETDATE());
GO

CREATE TABLE [AUC].[Permiso]
(
	[IdPermiso] int NOT NULL,
	[Nombre] varchar(50) NOT NULL,
	[Activo] bit NOT NULL,
	[IdUsuarioCreacion] int NOT NULL,
	[FechaCreacion] datetime NOT NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(1,'Consultar Guias',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(2,'Consultar Informes de Devolucion',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(3,'Consultar Informes de Despacho',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(4,'Ver planos',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(5,'Ver metrados',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(6,'Consultar constancias',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(7,'Consultar EECC',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(8,'Ver facturas',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(9,'Ver valorizaciones',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values(10,'Ver contratos',1,1,GETDATE());
GO

CREATE TABLE [AUC].[Perfil_Permiso]
(
	[IdPerfil] int NOT NULL,
	[IdPermiso] int NOT NULL,
	[Activo] bit NOT NULL,
	[IdUsuarioCreacion] int NOT NULL,
	[FechaCreacion] datetime NOT NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO
--Operaciones
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(4,1,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(4,2,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(4,3,1,1,GETDATE());
--Ingenieria
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(3,4,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(3,5,1,1,GETDATE());
--Contable
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(2,8,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(2,9,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(2,10,1,1,GETDATE());
--Administrador
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,1,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,2,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,3,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,4,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,5,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,6,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,7,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,8,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,9,1,1,GETDATE());
insert into AUC.Perfil_Permiso(IdPerfil,IdPermiso,Activo,IdUsuarioCreacion,FechaCreacion) values(5,10,1,1,GETDATE());
GO

--11032021
CREATE TABLE [PU].[TipoUnidad]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Nombre] varchar(50) NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into PU.TipoUnidad(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('I',1,1,GETDATE());
insert into PU.TipoUnidad(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('II',1,1,GETDATE());
insert into PU.TipoUnidad(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('III',1,1,GETDATE());
GO

CREATE TABLE [PU].[MetrajeTrailer]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Metro] int NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(3,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(4,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(5,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(6,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(8,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(9,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(10,1,1,GETDATE());
insert into PU.MetrajeTrailer(Metro,Activo,IdUsuarioCreacion,FechaCreacion) values(12,1,1,GETDATE());
GO

CREATE TABLE [PU].[UnidadMetraje]
(
	[IdTipoUnidad] int NOT NULL,
	[IdMetrajeTrailer] int NOT NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
)
GO
-- Tipo I tiene 3,4
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(1,1,1,GETDATE());
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(1,2,1,GETDATE());
-- Tipo II tiene 5,6,8
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(2,3,1,GETDATE());
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(2,4,1,GETDATE());
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(2,5,1,GETDATE());
-- Tipo III tiene 9,10,12
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(3,6,1,GETDATE());
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(3,7,1,GETDATE());
insert into PU.UnidadMetraje(IdTipoUnidad,IdMetrajeTrailer,IdUsuarioCreacion,FechaCreacion) values(3,8,1,GETDATE());
GO

CREATE TABLE [PU].[TipoProgramacion]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Nombre] varchar(50) NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into PU.TipoProgramacion(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Recepciï¿½n',1,1,GETDATE());
insert into PU.TipoProgramacion(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Despacho',1,1,GETDATE());
insert into PU.TipoProgramacion(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Traslado',1,1,GETDATE());
insert into PU.TipoProgramacion(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Importaciï¿½n',1,1,GETDATE());
insert into PU.TipoProgramacion(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Otros',1,1,GETDATE());
GO

CREATE TABLE [PU].[TipoEstado]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Nombre] varchar(50) NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('En revision',1,1,GETDATE());
insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Revisado',1,1,GETDATE());
insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Cancelado',1,1,GETDATE());
insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Confirmado',1,1,GETDATE());
insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Por confirmar',1,1,GETDATE());
GO

CREATE TABLE [PU].[ProgramacionEstado]
(
	[IdTipoProgramacion] int NOT NULL,
	[IdTipoEstado] int NOT NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
)
GO
--Recepcion
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(1,1,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(1,2,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(1,3,1,GETDATE());
--Despacho
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(2,4,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(2,5,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(2,3,1,GETDATE());
--Traslado
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(3,4,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(3,5,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(3,3,1,GETDATE());
--Importacion
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(4,4,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(4,5,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(4,3,1,GETDATE());
--Otros
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(5,4,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(5,5,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(5,3,1,GETDATE());
GO

--15032021
CREATE TABLE [AUC].[Perfil_Persona]
(
	[IdPerfil_Persona] int NOT NULL,
	[IdPerfil] int NOT NULL,
	[IdPersona] int NOT NULL,
	[Activo] bit NOT NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioCreacion] int NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

CREATE TABLE [AUC].[Permiso_Perfil_Persona]
(
	[IdPermiso_Perfil_Persona] int NOT NULL,
	[IdPerfil_Persona] int NOT NULL,
	[IdPermiso] int NOT NULL,
	[IdUsuarioCreacion] int NOT NULL,
	[FechaCreacion] datetime NOT NULL
)
GO

CREATE TABLE [AUC].[Proyecto_Perfil_Persona]
(
	[IdProyecto_Perfil_Persona] int NOT NULL,
	[IdPerfil_Persona] int NOT NULL,
	[afe] char(15) NOT NULL,
	[IdUsuarioCreacion] int NOT NULL,
	[FechaCreacion] datetime NOT NULL
)
GO



/**
			MODULO PROGRMACION DE UNIDADES
**/
/****** CREACIÓN DE TABLA: PersonaAlmacen ******/
CREATE TABLE [SEG].[PersonaAlmacen](
	[IdPersonaAlmacen] [int] IDENTITY(1,1) NOT NULL,
	[IdPersona] [int] NOT NULL,
	[AlmacenCodigo] [char](10) NOT NULL,
	[IdUsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[IdUsuarioActualizacion] [int] NULL,
	[FechaActualizacion] [datetime] NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_PersonaAlmacen] PRIMARY KEY CLUSTERED 
(
	[IdPersonaAlmacen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- VALORES POR DEFECTO
ALTER TABLE [SEG].[PersonaAlmacen] ADD CONSTRAINT [DF_PersonaAlmacen_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [SEG].[PersonaAlmacen] ADD CONSTRAINT [DF_PersonaAlmacen_Activo]  DEFAULT (1) FOR [Activo]
GO

-- DESCRIPCIÓN DE CAMPOS
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador de la relación entre persona y almacén' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'IdPersonaAlmacen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador del persona' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'IdPersona'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del almacén' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'AlmacenCodigo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador del usuario que creó el registro' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'IdUsuarioCreacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de creación del registro' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'FechaCreacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador del último usuario que actualizó el registro' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'IdUsuarioActualizacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de última actualización del registro' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'FechaActualizacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el registro está activo' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen', @level2type=N'COLUMN',@level2name=N'Activo'
GO

-- DESCRIPCIÓN DE TABLA
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que contiene información sobre la relación entre persona y almacén.' , @level0type=N'SCHEMA',@level0name=N'SEG', @level1type=N'TABLE',@level1name=N'PersonaAlmacen'
GO

-- Solicitud Evaluación Cliente
ALTER TABLE [SEG].[PersonaAlmacen] ADD CONSTRAINT FK_PersonaAlmacen_Persona FOREIGN KEY (IdPersona) REFERENCES [dbo].[GL_MaestroPersonas](Persona)
GO

-- IdTipoRegistroPersona
ALTER TABLE [SEG].[PersonaAlmacen] ADD CONSTRAINT FK_PersonaAlmacen_MaestroAlmacen FOREIGN KEY (AlmacenCodigo) REFERENCES [dbo].[LG_MaestroAlmacen](AlmacenCodigo)
GO

IF object_id('PU.ProgramacionUnidad') IS NOT NULL
     DROP table PU.ProgramacionUnidad
GO

CREATE TABLE [PU].[ProgramacionUnidad]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[NumeroProgramacion] varchar(10) NULL,
	[FechaInicio] datetime NULL,
	[FechaFin] datetime NULL,
	[IdEstado] int NULL,
	[Correo] varchar(30) NULL,
	[Tonelada] decimal(7,3) NULL,
	[Especificacion] varchar(50) null,
	[FechaRevision] datetime NULL,
	[RevisionHoraInicio] datetime NULL,
	[RevisionHoraFin] datetime NULL,
	[AlquilerFin] datetime NULL,
	[IdMetrajeTrailer] int NULL,
	[IdProyecto] char(15) NULL,
	[IdTipoProgramacion] int NULL,
	[IdCotizacion] varchar(50) NULL,
	[IdTipoUnidad] int NULL,
	[IdFormulario] int NULL,
	[IdAlmacen] varchar(10) null,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

IF object_id('PU.Formulario') IS NOT NULL
     DROP table PU.Formulario
GO

CREATE TABLE [PU].[Formulario]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[EsTransporteNacional] bit NULL,
	[NumeroDocumento] varchar(20) NULL,
	[RazonSocial] varchar(50) NULL,
	[ModeloVehiculo] varchar(30) NULL,
	[PlacaTracto] varchar(20) NULL,
	[PlacaCarreta] varchar(20) NULL,
	[TelefonoContacto] varchar(9) NULL,
	[IdTipoDocumentoTransportista] char(1) NULL,
	[DocumentoTransportista] varchar(50) NULL,
	[NombreTransportista] varchar(30) NULL,
	[SctrTransportista] varchar(20) NULL,
	[TelefonoTransportista] varchar(9) NULL,
	[LicenciaTransportista] varchar(20) NULL,
	[IdTipoDocumentoEncargado] char(1) NULL,
	[DocumentoEncargado] varchar(50) NULL,
	[NombreEncargado] varchar(30) NULL,
	[SctrEncargado] varchar(20) NULL,
	[Consideracion] text NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

IF object_id('PU.FormularioConsideracion') IS NOT NULL
     DROP table PU.FormularioConsideracion
GO

CREATE TABLE [PU].[FormularioConsideracion]
(
	[IdFormulario] int NULL,
	[IdConsideracion] int NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO

IF object_id('PU.Adjunto') IS NOT NULL
     DROP table PU.Adjunto
GO

CREATE TABLE [PU].[Adjunto]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Nombre] varchar(200) NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('SCTR Transportista',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('SCTR Encargado',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Tengo Guia Fisica',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('SOAT',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Brevete Conductor',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Guia de Remision Electronica',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Declaracion Jurada de alcohol y drogas',1,1,GETDATE());
insert into PU.Adjunto(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Evaluacion de riesgo',1,1,GETDATE());

IF object_id('PU.AdjuntoFormulario') IS NOT NULL
     DROP table PU.AdjuntoFormulario
GO

CREATE TABLE [PU].[AdjuntoFormulario]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[IdFormulario] int NULL,
	[IdAdjunto] int NULL,
	[Nombre] varchar(30) NULL,
	[Ruta] varchar(100) NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL
)
GO

IF object_id('PU.Consideracion') IS NOT NULL
     DROP table PU.Consideracion
GO

CREATE TABLE [PU].[Consideracion]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[Nombre] varchar(100) NULL,
	[Activo] bit NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL,
	[IdUsuarioEdicion] int NULL,
	[FechaEdicion] datetime NULL
)
GO
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Casco',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Guantes',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Tacos',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Unidad abierta',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Zapatos',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Polo/Camisa manga larga',1,1,GETDATE());
insert into PU.Consideracion(nombre,activo,IdUsuarioCreacion,FechaCreacion) values('Fajas',1,1,GETDATE());
GO

ALTER TABLE PU.ProgramacionUnidad ADD Activo bit null;
GO

update PU.ProgramacionUnidad set Activo = 1;
GO