IF NOT EXISTS ( SELECT * FROM    sys.schemas WHERE   name = N'INSPECCION' )
    EXEC('CREATE SCHEMA [INSPECCION]');
GO

CREATE TABLE INSPECCION.InspeccionAndamio(
	InspeccionAndamio_id INT IDENTITY(1,1) NOT NULL,
    EstadoInspeccionAndamio_id INT NOT NULL,
	Cliente_id INT NOT NULL,
    Proyecto VARCHAR(250) NOT NULL,
    Direccion VARCHAR(400) NULL,
    ZonaTrabajo VARCHAR(400) NULL,
    Responsable VARCHAR(400) NULL,
    Cargo VARCHAR(400) NULL,
    MarcaAndamio VARCHAR(400) NOT NULL,
    SobreCargaUso VARCHAR(50) NULL,
    Observacion VARCHAR(400) NULL,
	IdUsuarioCreacion INT NOT NULL,
	IdUsuarioEdicion INT NULL,
	FechaCreacion DATETIME NOT NULL,
	FechaEdicion DATETIME NULL,
	Activo BIT NOT NULL,
 CONSTRAINT PK_InspeccionAndamio PRIMARY KEY CLUSTERED 
(
	InspeccionAndamio_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE INSPECCION.CheckList(
	CheckList_id INT IDENTITY(1,1) NOT NULL,
	InspeccionAndamio_id INT NOT NULL,
	TipoAndamio_id INT NOT NULL,
	SubTipoAndamio_id INT NULL,
	IdUsuarioCreacion INT NOT NULL,
	IdUsuarioEdicion INT NULL,
	FechaCreacion DATETIME NOT NULL,
	FechaEdicion DATETIME NULL,
	Activo BIT NOT NULL,
 CONSTRAINT PK_CheckList PRIMARY KEY CLUSTERED 
(
	CheckList_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE INSPECCION.CheckList  
ADD  CONSTRAINT FK_CheckList_InspeccionAndamio 
FOREIGN KEY(InspeccionAndamio_id)
REFERENCES INSPECCION.InspeccionAndamio (InspeccionAndamio_id)
GO

ALTER TABLE INSPECCION.CheckList
ADD  CONSTRAINT FK_CheckList_TipoAndamio
FOREIGN KEY(TipoAndamio_id)
REFERENCES COMERCIAL.TipoAndamio (tipo_sol_id)
GO

CREATE TABLE INSPECCION.Cumplimiento(
	Cumplimiento_id INT IDENTITY(1,1) NOT NULL,
	CheckList_id INT NOT NULL,
	PreguntaInspeccionAndamio_id INT NULL,
	RespuestaCumplimiento_id INT NULL,
	IdUsuarioCreacion INT NOT NULL,
	IdUsuarioEdicion INT NULL,
	FechaCreacion DATETIME NOT NULL,
	FechaEdicion DATETIME NULL,
	Activo BIT NOT NULL,
 CONSTRAINT PK_Cumplimiento PRIMARY KEY CLUSTERED 
(
	Cumplimiento_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE INSPECCION.Cumplimiento
ADD  CONSTRAINT FK_Cumplimiento_CheckList 
FOREIGN KEY(CheckList_id)
REFERENCES INSPECCION.CheckList (CheckList_id)
GO

ALTER TABLE INSPECCION.Cumplimiento
ADD  CONSTRAINT FK_Cumplimiento_PreguntaInspeccionAndamio 
FOREIGN KEY(PreguntaInspeccionAndamio_id)
REFERENCES ASESORIA.PreguntaInspeccionAndamio (PreguntaInspeccionAndamio_id)
GO

ALTER TABLE INSPECCION.Cumplimiento
ADD  CONSTRAINT FK_Cumplimiento_RespuestaCumplimiento 
FOREIGN KEY(RespuestaCumplimiento_id)
REFERENCES ASESORIA.RespuestaCumplimiento (RespuestaCumplimiento_id)
GO

CREATE TABLE INSPECCION.RegistroFotografico(
	RegistroFotografico_id INT IDENTITY(1,1) NOT NULL,
    InspeccionAndamio_id INT NOT NULL,
    Descripcion VARCHAR(400) NULL,
	Nombre VARCHAR(400) NULL,
	NombreOriginal VARCHAR(400) NULL,
	EsFoto BIT NULL,
	IdUsuarioCreacion INT NOT NULL,
	IdUsuarioEdicion INT NULL,
	FechaCreacion DATETIME NOT NULL,
	FechaEdicion DATETIME NULL,
	Activo BIT NOT NULL,
 CONSTRAINT PK_RegistroFotografico PRIMARY KEY CLUSTERED 
(
	RegistroFotografico_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE INSPECCION.RegistroFotografico  
ADD  CONSTRAINT FK_RegistroFotografico_InspeccionAndamio 
FOREIGN KEY(InspeccionAndamio_id)
REFERENCES INSPECCION.InspeccionAndamio (InspeccionAndamio_id)
GO


CREATE TABLE INSPECCION.EstadoInspeccionAndamio(
	EstadoInspeccionAndamio_id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(400) NULL,
	IdUsuarioCreacion INT NOT NULL,
	IdUsuarioEdicion INT NULL,
	FechaCreacion DATETIME NOT NULL,
	FechaEdicion DATETIME NULL,
	Activo BIT NOT NULL,
 CONSTRAINT PK_EstadoInspeccionAndamio PRIMARY KEY CLUSTERED 
(
	EstadoInspeccionAndamio_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO INSPECCION.EstadoInspeccionAndamio (Nombre,IdUsuarioCreacion,FechaCreacion,Activo)
VALUES
('En Proceso',1,GETDATE(),1),
('Completado',1,GETDATE(),1);
GO

ALTER TABLE INSPECCION.InspeccionAndamio  
ADD  CONSTRAINT FK_InspeccionAndamio_EstadoInspeccionAndamio
FOREIGN KEY(EstadoInspeccionAndamio_id)
REFERENCES INSPECCION.EstadoInspeccionAndamio (EstadoInspeccionAndamio_id)
GO

-- RollBack
 --ALTER TABLE INSPECCION.InspeccionAndamio   
 --DROP CONSTRAINT FK_InspeccionAndamio_EstadoInspeccionAndamio;
 --DROP TABLE INSPECCION.EstadoInspeccionAndamio;
 --DROP TABLE INSPECCION.RegistroFotografico;
 --DROP TABLE INSPECCION.Cumplimiento;
 --DROP TABLE INSPECCION.CheckList;
 --DROP TABLE INSPECCION.InspeccionAndamio;
 --DROP SCHEMA INSPECCION;
