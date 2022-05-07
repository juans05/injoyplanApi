IF NOT EXISTS ( SELECT * FROM    sys.schemas WHERE   name = N'NOTIFICACION' )
    EXEC('CREATE SCHEMA [NOTIFICACION]');
GO

IF object_id('NOTIFICACION.Dispositivo') IS NOT NULL
     DROP table NOTIFICACION.Dispositivo
GO

CREATE TABLE NOTIFICACION.Dispositivo (
Id INT PRIMARY KEY IDENTITY(1,1),
Usuario_Id INT,
TokenDispositivo VARCHAR(255),
OrigenDispositivo INT,
Estado BIT,
IdUsuarioCreacion INT,
FechaCreacion DATETIME2,
IdUsuarioEdicion INT,
FechaEdicion DATETIME2
);
GO

/**
			MODULO PROGRMACION DE UNIDADES
**/
/****** CREACIÓN DE TABLA: PersonaAlmacen ******/


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

ALTER TABLE PU.ProgramacionUnidad ADD Activo bit null
GO

update PU.ProgramacionUnidad set Activo = 1;
GO

ALTER TABLE PU.ProgramacionUnidad DROP COLUMN NumeroProgramacion;
GO

ALTER TABLE PU.ProgramacionUnidad ADD NombreCliente varchar(100);
ALTER TABLE PU.ProgramacionUnidad ADD NombreProyecto varchar(100);
ALTER TABLE PU.ProgramacionUnidad ADD FechaLlegada datetime;
ALTER TABLE PU.ProgramacionUnidad ADD TurnoLlegada datetime;
ALTER TABLE PU.ProgramacionUnidad ADD TelefonoEncargado varchar(9);
ALTER TABLE PU.ProgramacionUnidad ADD NumeroContenedores varchar(30);
ALTER TABLE PU.ProgramacionUnidad ADD Contenedor varchar(30);
ALTER TABLE PU.ProgramacionUnidad ADD PackingList varchar(50);
GO

ALTER TABLE PU.ProgramacionUnidad ADD TipoImportacion varchar(50) null