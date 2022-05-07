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
														 and FechaInicio > @Fecha)
	  BEGIN
		  SET @Response = 1;
	  END	  
	SELECT	'Return' = @Response
END
GO

IF object_id('PU.Usp_ListaCalendario') IS NOT NULL
     DROP PROC PU.Usp_ListaCalendario
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListaCalendario] --'2021-03-15','0'
	@Fecha datetime,
	@Sede varchar(100)
AS
BEGIN
	Declare @afterFecha datetime;
	Declare @beforeFecha datetime;

	set @afterFecha = DATEADD(MONTH,2,@Fecha);
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
			--and (A.IdAlmacen = @Sede or'0' = @Sede)
			and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
			and FechaInicio between @beforeFecha and @afterFecha
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
			--and (A.IdAlmacen = @Sede or'0' = @Sede)
			and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
			and FechaInicio between @beforeFecha and @afterFecha
	Group by A.IdTipoProgramacion,C.Nombre,Convert(Date,A.FechaInicio)) tab2
END
GO

IF object_id('PU.Usp_ListBandejaCliente') IS NOT NULL
     DROP PROC PU.Usp_ListBandejaCliente
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListBandejaCliente]
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
			H.localname as NombreProyecto,J.Id as IdFormulario, J.NumeroDocumento, J.RazonSocial,J.ModeloVehiculo,J.PlacaTracto,J.PlacaCarreta,
			tab.* 
	from (select A.Id as IdProgramacionUnidad,A.*
				   from PU.ProgramacionUnidad A 
				   where A.IdTipoProgramacion in (1,2)) tab
	left join PU.Formulario J on tab.IdFormulario = J.Id
	left join PU.TipoEstado B on tab.IdEstado = B.Id
	left join PU.MetrajeTrailer C on tab.IdMetrajeTrailer = C.Id
	left join PU.TipoUnidad D on tab.IdTipoUnidad = D.Id
	left join LG_MaestroAlmacen F on tab.IdAlmacen = F.AlmacenCodigo 
	left join LG_MaestroAlmacen M on tab.IdAlmacenDestino = M.AlmacenCodigo 
	left join PU.Formulario G on tab.IdFormulario = G.Id 
	left join CB_MestroProyectos H on tab.IdProyecto = H.afe
	left join GL_MaestroPersonas I on H.Client_ID = I.Persona
	left join PU.TipoProgramacion K on tab.IdTipoProgramacion = k.Id
	left join GL_MaestroPersonas L on tab.IdUsuarioCreacion = L.Persona
	where (tab.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad) 
		  and (tab.IdTipoProgramacion = @TipoProgramacion or 0 = @TipoProgramacion)
		  and (tab.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (tab.FechaInicio between @FechaInicio and DATEADD(day,1,@FechaFin))
		  and (tab.IdEstado = @Estado or 0 = @Estado)
		  and (tab.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and tab.Activo = 1;
END
GO

IF object_id('PU.Usp_ListBandejaProveedor') IS NOT NULL
     DROP PROC PU.Usp_ListBandejaProveedor
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListBandejaProveedor]
	@ProgramacionUnidad int,
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
		  and (A.IdProyecto = @Proyecto or '0' = @Proyecto)
		  and (A.FechaInicio between @FechaInicio and DATEADD(day,1,@FechaFin))
		  and (A.IdEstado = @Estado or 0 = @Estado)
		  and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (I.Persona = @Persona or 0 = @Persona)
		  and A.Activo = 1
		  and A.IdTipoProgramacion = 3;
END
GO