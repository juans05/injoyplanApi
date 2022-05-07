ALTER TABLE PU.ProgramacionUnidad ADD Observacion text null

update PU.TipoEstado set Nombre = 'Pte.Revision' where id = 1;

IF object_id('PU.ProgramacionTiempo') IS NOT NULL
     DROP table PU.ProgramacionTiempo
GO

CREATE TABLE PU.ProgramacionTiempo (
Id int NOT NULL IDENTITY (1, 1),
IdProgramacionUnidad int null,
IdTipoMontaCarga int null,
DescargaInicio Datetime2 null,
DescargaFin Datetime2 null,
FechaRevision Datetime2 null,
InicioRevision Datetime2 null,
FinRevision Datetime2 null,
FechaRevision2 Datetime2 null,
InicioRevision2 Datetime2 null,
FinRevision2 Datetime2 null,
IdUsuarioCreacion INT,
FechaCreacion DATETIME2,
IdUsuarioEdicion INT,
FechaEdicion DATETIME2
);
GO

IF object_id('AUC.ListaNegra') IS NOT NULL
     DROP table AUC.ListaNegra
GO

CREATE TABLE AUC.ListaNegra (
Id int NOT NULL IDENTITY (1, 1),
Token varchar(100) null,
IdUsuarioCreacion INT,
FechaCreacion DATETIME2
);
GO

ALTER TABLE PU.AdjuntoFormulario ALTER COLUMN Nombre varchar(200);
GO

ALTER TABLE PU.ProgramacionUnidad ALTER COLUMN Correo varchar(500);
GO

ALTER TABLE PU.ProgramacionUnidad ADD IdAlmacenDestino varchar(10);
GO

IF object_id('PU.ArchivoAdjunto') IS NOT NULL
     DROP table PU.ArchivoAdjunto
GO

CREATE TABLE [PU].[ArchivoAdjunto]
(
	[Id] int NOT NULL IDENTITY (1, 1),
	[IdFormulario] int NULL,
	[Nombre] varchar(100) NULL,
	[Ruta] varchar(200) NULL,
	[IdUsuarioCreacion] int NULL,
	[FechaCreacion] datetime NULL
)
GO