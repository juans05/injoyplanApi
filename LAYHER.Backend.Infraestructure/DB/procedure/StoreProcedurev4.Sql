IF object_id('PU.Usp_ListaCalendario') IS NOT NULL
     DROP PROC PU.Usp_ListaCalendario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaCalendario] --'2021-03-15','0'
	@Fecha datetime,
	@Sede varchar(10)
AS
BEGIN
	Declare @afterFecha datetime;
	Declare @beforeFecha datetime;

	set @afterFecha = DATEADD(MONTH,1,@Fecha);
	set @beforeFecha = DATEADD(MONTH,-1,@Fecha);

	select tab1.CantidadObservacion,
		   tab1.IdTipoProgramacion,
	       tab1.NombreTipoProgramacion,
		   tab1.Tonelada,
		   Convert(char(10),tab1.FechaInicio,126) as FechaInicio,
			Case
				WHEN tab1.IdTipoProgramacion = 1 THEN
												CASE WHEN tab1.Tonelada <= 20 THEN 'VERDE'
													 WHEN tab1.Tonelada > 20 and tab1.Tonelada <= 40 THEN 'AMARILLO'
													 ELSE 'ROJO' END
				ELSE
												CASE WHEN tab1.Tonelada <= 30 THEN 'VERDE'
													 WHEN tab1.Tonelada > 30 and tab1.Tonelada <= 60 THEN 'AMARILLO'
													 ELSE 'ROJO' END
				END as Color
	from (select 
	        Count(case when A.Observacion is not null then 1 end) as CantidadObservacion,
			A.IdTipoProgramacion, 
			C.Nombre as NombreTipoProgramacion,
			Convert(Date,A.FechaInicio) as FechaInicio,
			SUM(A.Tonelada) as Tonelada
	from PU.ProgramacionUnidad A
	left join PU.TipoProgramacion C on A.IdTipoProgramacion = C.Id
	where A.Activo = 1 and A.IdTipoProgramacion in (1,2)
			and (A.IdAlmacen = @Sede or'0' = @Sede)
			and FechaInicio between @beforeFecha and @afterFecha--(MONTH(FechaInicio) = MONTH(@Fecha) and YEAR(FechaInicio) = YEAR(@Fecha))
	Group by A.IdTipoProgramacion, C.Nombre,Convert(Date,A.FechaInicio)) tab1
	UNION ALL
	select tab2.CantidadObservacion,
		   tab2.IdTipoProgramacion,
	       tab2.NombreTipoProgramacion,
		   tab2.Tonelada, 
		   Convert(char(10),tab2.FechaInicio,126) as FechaInicio,
		   CASE 
				WHEN tab2.Tonelada <= 20 THEN 'VERDE'
			    WHEN tab2.Tonelada > 20 and tab2.Tonelada <= 40 THEN 'AMARILLO'
				ELSE 'ROJO' END
	from (select 
		    Count(case when A.Observacion is not null then 1 end) as CantidadObservacion,
			A.IdTipoProgramacion, 
			'Revisado' as NombreTipoProgramacion,
			Convert(Date,A.FechaInicio) as FechaInicio,
			SUM(A.Tonelada) as Tonelada
	from PU.ProgramacionUnidad A
	left join PU.TipoProgramacion C on A.IdTipoProgramacion = C.Id
	where A.Activo = 1 and A.IdTipoProgramacion = 1 and A.IdEstado = 2 
			and (A.IdAlmacen = @Sede or'0' = @Sede)
			and FechaInicio between @beforeFecha and @afterFecha--(MONTH(FechaInicio) = MONTH(@Fecha) and YEAR(FechaInicio) = YEAR(@Fecha))
	Group by A.IdTipoProgramacion,C.Nombre,Convert(Date,A.FechaInicio)) tab2
END
GO

IF object_id('PU.Usp_ListProgramacionObservacion') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionObservacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListProgramacionObservacion] --'2021-04-13','0'
	@Fecha Datetime,
	@Sede varchar(10)
AS
BEGIN
	select 
			A.Id as IdProgramacionUnidad,A.IdTipoProgramacion,F.Nombre as NombreTipoProgramacion,
			A.FechaInicio,A.FechaFin,E.NombreCompleto,B.RazonSocial,
			B.PlacaCarreta,B.PlacaTracto,A.IdEstado,C.Nombre as NombreEstado,
			B.Id as IdFormulario
	from PU.ProgramacionUnidad A
	left join PU.Formulario B on A.IdFormulario = B.Id
	left join PU.TipoEstado C on A.IdEstado = C.Id
	left join CB_MestroProyectos D on A.IdProyecto = D.afe
	left join GL_MaestroPersonas E on D.Client_ID = E.Persona
	left join PU.TipoProgramacion F on A.IdTipoProgramacion = F.Id
	where Convert(Date,A.FechaInicio) = Convert(Date,@Fecha)
		  and (A.IdAlmacen = @Sede or '0' = @Sede)
		  and A.Activo = 1;
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
			A.FechaInicio,A.FechaFin,E.NombreCompleto,B.EsTransporteNacional,B.RazonSocial,
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

IF object_id('PU.Usp_UpdateProgramacionEstado') IS NOT NULL
     DROP PROC PU.Usp_UpdateProgramacionEstado
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_UpdateProgramacionEstado]
	@ProgramacionUnidad int,
	@Estado int,
	@UsuarioEdicion int
AS
BEGIN
	Update PU.ProgramacionUnidad
	set 
		IdEstado = @Estado,
		IdUsuarioEdicion = @UsuarioEdicion,
		FechaEdicion = GETDATE()
	where Id = @ProgramacionUnidad	
END
GO

IF object_id('PU.Usp_SaveProgramacionTiempo') IS NOT NULL
     DROP PROC PU.Usp_SaveProgramacionTiempo
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_SaveProgramacionTiempo]
	@ProgramacionUnidad int,
	@ProgramacionTiempo int,
	@TipoMontacarga int,
	@DescargaInicio datetime,
	@DescargaFin datetime,
	@FechaRevision datetime,
	@InicioRevision datetime,
	@FinRevision datetime,
	@FechaRevision2 datetime,
	@InicioRevision2 datetime,
	@FinRevision2 datetime,
	@UsuarioCreacion int
AS
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
GO

IF object_id('PU.Usp_ListProgramacionTiempo') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionTiempo
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListProgramacionTiempo]
    @ProgramacionTiempo int,
	@ProgramacionUnidad int
AS
BEGIN
	select 
			A.Id as IdProgramacionTiempo,
			A.IdProgramacionUnidad,
			A.DescargaInicio,
			A.DescargaFin,
			A.IdTipoMontaCarga,
			A.FechaRevision,
			A.InicioRevision,
			A.FinRevision,
			A.FechaRevision2,
			A.InicioRevision2,
			A.FinRevision2
	from PU.ProgramacionTiempo A
	left join PU.ProgramacionUnidad B on A.IdProgramacionUnidad = B.Id
	where (A.Id = @ProgramacionTiempo or 0 = @ProgramacionTiempo) and
		  (B.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad);
END
GO

IF object_id('AUC.Usp_SaveListaNegra') IS NOT NULL
     DROP PROC AUC.Usp_SaveListaNegra
GO

CREATE OR ALTER   PROCEDURE [AUC].[Usp_SaveListaNegra]
		@Token varchar(100),
		@UsuarioCreacion int,
		@FechaCreacion datetime
	AS
	BEGIN
		insert into AUC.ListaNegra(Token,
								   IdUsuarioCreacion,
								   FechaCreacion) 
		OUTPUT inserted.ID AS Id
		values(@Token,
			   @UsuarioCreacion,
			   GETDATE());
	END
GO

IF object_id('AUC.Usp_ValidaListaNegra') IS NOT NULL
     DROP PROC AUC.Usp_ValidaListaNegra
GO

CREATE OR ALTER PROCEDURE [AUC].[Usp_ValidaListaNegra](
 @Token varchar(100)
)
AS  
BEGIN   
 Declare @Exist bit   
 IF Exists (SELECT id from AUC.ListaNegra where Token = @Token)  
   BEGIN  
    SET @Exist = 1;  
   END  
 Else  
  BEGIN  
   SET @Exist = 0;  
  END   
 select @Exist;
END  
GO

IF object_id('NOTIFICACION.usp_registrar_dispositivo') IS NOT NULL
     DROP PROCEDURE NOTIFICACION.usp_registrar_dispositivo
GO

CREATE PROC NOTIFICACION.usp_registrar_dispositivo(  
	
    @Usuario_Id INT,  
    @TokenDispositivo VARCHAR(255),  
    @OrigenDispositivo INT,  
    @Estado BIT,  
    @IdUsuarioCreacion INT,  
    @FechaCreacion DATETIME2,  
    @IdUsuarioEdicion INT,  
    @FechaEdicion DATETIME2  
)  
AS  
BEGIN  
    DECLARE @Id INT = NULL;  
    SELECT TOP 1 @Id=Id
    FROM NOTIFICACION.Dispositivo  
    WHERE Usuario_Id=@Usuario_Id AND TokenDispositivo=@TokenDispositivo;  

    IF(@Id IS NULL)  
    BEGIN  
		print('inserted');
        INSERT INTO NOTIFICACION.Dispositivo 
            (  
            Usuario_Id  
            ,TokenDispositivo  
            ,OrigenDispositivo  
            ,Estado  
            ,IdUsuarioCreacion  
            ,FechaCreacion  
            )  
        VALUES  
            (  
                @Usuario_Id  
        , @TokenDispositivo  
        , @OrigenDispositivo  
        , @Estado  
        , @IdUsuarioCreacion  
        , @FechaCreacion);  
    END  
ELSE  
BEGIN  
		print('updated');
        UPDATE NOTIFICACION.Dispositivo SET  
         Usuario_Id =@Usuario_Id,  
                 TokenDispositivo =@TokenDispositivo,  
                 OrigenDispositivo =@OrigenDispositivo,  
                 Estado =@Estado,  
                 IdUsuarioEdicion =@IdUsuarioEdicion,  
                 FechaEdicion =@FechaEdicion  
        WHERE Id=@Id;  
    END  
END  
GO

IF object_id('NOTIFICACION.usp_obtener_dispositivos_por_usuario') IS NOT NULL
     DROP PROCEDURE NOTIFICACION.usp_obtener_dispositivos_por_usuario
GO

CREATE PROC NOTIFICACION.usp_obtener_dispositivos_por_usuario(
    @Usuario_Id INT
)  
AS  
BEGIN  
    SELECT *
    FROM NOTIFICACION.Dispositivo  
    WHERE Estado=1 AND Usuario_Id=@Usuario_Id;
END  
GO

IF object_id('PU.Usp_UpdateProgramacionObservacion') IS NOT NULL
     DROP PROCEDURE PU.Usp_UpdateProgramacionObservacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_UpdateProgramacionObservacion]
	@ProgramacionUnidad int,
	@Observacion text,
	@UsuarioEdicion int
AS
BEGIN
	Update PU.ProgramacionUnidad
	set Observacion = @Observacion,
	    IdUsuarioEdicion = @UsuarioEdicion,
		FechaEdicion = GETDATE()
	where Id = @ProgramacionUnidad	
END
GO

IF object_id('AUC.Usp_ListaProyectoCliente') IS NOT NULL
     DROP PROC AUC.Usp_ListaProyectoCliente
GO

CREATE OR ALTER   PROCEDURE [AUC].[Usp_ListaProyectoCliente]
	@Proyecto varchar(15)
AS
BEGIN
	Select 
			A.afe,A.companyowner,
			A.localname,
			B.NombreCompleto as NombreCliente,
		    B.Persona as IdCliente,
			B.CorreoElectronico as CorreoCliente,
			A.id_ejecutivo as IdComercial,
			C.NombreCompleto as NombreComercial,
			D.vc_idContrato as IdContrato,
			D.vc_DescripContrato as NombreContrato
	From CB_MestroProyectos A 
	inner join GL_MaestroPersonas B on A.Client_ID = B.Persona
	inner join GL_MaestroPersonas C on A.id_ejecutivo = C.Persona
	inner join FC_MaestroContratos D on A.afe = D.vc_idProyecto
	Where (A.afe = @Proyecto or '0' = @Proyecto) 
	ORDER BY A.afe;
END
GO