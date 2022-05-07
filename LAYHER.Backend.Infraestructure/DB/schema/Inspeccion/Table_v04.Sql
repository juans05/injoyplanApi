CREATE TABLE INSPECCION.ConfiguracionApp(
    ConfiguracionApp_id INT IDENTITY(1,1) NOT NULL,
    Usuario_id INT NOT NULL,
    ModoHistorico BIT NULL,
    CantidadAnios INT NULL,
    IdUsuarioCreacion INT NOT NULL,
    IdUsuarioEdicion INT NULL,
    FechaCreacion DATETIME NOT NULL,
    FechaEdicion DATETIME NULL,
    Activo BIT NOT NULL,
 CONSTRAINT PK_ConfiguracionApp PRIMARY KEY CLUSTERED 
(
	ConfiguracionApp_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM SEG.TipoRelacionEmpresaEmpleado WHERE Nombre='INSPECTOR')
BEGIN
	INSERT INTO SEG.TipoRelacionEmpresaEmpleado (
	Nombre,
	IdUsuarioCreacion,
	FechaCreacion,
	Activo
	) 
	VALUES
	(
	'INSPECTOR',
	1,
	GETDATE(),
	1
	);
END
GO

DECLARE @IdEmpresa INT, @IdEmpleado INT, @IdTipoRelacionEmpresaEmpleado INT;
SET @IdEmpresa = (SELECT Persona FROM GL_MaestroPersonas WHERE NombreCompleto='SISTEMAS TECNICOS AUXILIARES S.A.C.');
SET @IdEmpleado = (SELECT Persona FROM GL_MaestroPersonas WHERE Documento='42120505');
SET @IdTipoRelacionEmpresaEmpleado = (SELECT IdTipoRelacionEmpresaEmpleado FROM SEG.TipoRelacionEmpresaEmpleado WHERE Nombre='INSPECTOR');


IF NOT EXISTS(SELECT * FROM SEG.EmpresaEmpleado WHERE Empresa=@IdEmpresa AND Empleado=@IdEmpleado AND IdTipoRelacionEmpresaEmpleado=@IdTipoRelacionEmpresaEmpleado)
BEGIN
	INSERT INTO SEG.EmpresaEmpleado (
Empresa,
Empleado,
Activo,
IdUsuarioCreacion,
FechaCreacion,
IdTipoRelacionEmpresaEmpleado
	) 
	VALUES
	(
	@IdEmpresa,
	@IdEmpleado,
	1,
	1,
	GETDATE(),
	@IdTipoRelacionEmpresaEmpleado
	);
END
GO