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
	@Almacen varchar(100),
	@Persona int
AS
BEGIN
	select 
			B.Nombre as NombreEstado,C.Metro as Longitud,D.Nombre as NombreUnidad,F.DescripcionLocal,
			M.DescripcionLocal as DescripcionLocalDestino,I.NombreCompleto,
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
		  --and (A.IdAlmacen = @Almacen or '0' = @Almacen)
		  and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Almacen, ',')))
		  and (I.Persona = @Persona or 0 = @Persona)
		  and A.Activo = 1;
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

	--Recepcion e Importacion
	select
		   tab10.CantidadObservacion,
	       tab10.NombreTipoProgramacion,
		   1 as idTipoEvento,
		   tab10.Tonelada,
		   Convert(char(10),tab10.FechaInicio,126) as FechaInicio,
			Case
				WHEN tab10.Tonelada <= 20 THEN 'VERDE'
				WHEN tab10.Tonelada > 20 and tab10.Tonelada <= 40 THEN 'AMARILLO'								
				ELSE 'ROJO'
				END as Color
	from
		(select 
				SUM(tab1.CantidadObservacion) as CantidadObservacion,
				'Recepcion' as NombreTipoProgramacion,
				tab1.FechaInicio as FechaInicio, 
				SUM(tab1.Tonelada) as Tonelada
		from 
		   (select 
					Count(case when A.Observacion is not null then 1 end) as CantidadObservacion,
					A.IdTipoProgramacion, 
					C.Nombre as NombreTipoProgramacion,
					Convert(Date,A.FechaInicio) as FechaInicio,
					SUM(A.Tonelada) as Tonelada
			from PU.ProgramacionUnidad A
			left join PU.TipoProgramacion C on A.IdTipoProgramacion = C.Id
			where A.Activo = 1 and A.IdTipoProgramacion in (1,4)
					and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
					and FechaInicio between @beforeFecha and @afterFecha
			Group by A.IdTipoProgramacion, C.Nombre,Convert(Date,A.FechaInicio)) tab1
			Group by FechaInicio) tab10
	UNION ALL
	--Despacho y Traslado
	select
		   tab20.CantidadObservacion,
	       tab20.NombreTipoProgramacion,
		   2 as idTipoEvento,
		   tab20.Tonelada,
		   Convert(char(10),tab20.FechaInicio,126) as FechaInicio,
			Case
				WHEN tab20.Tonelada <= 30 THEN 'VERDE'
				WHEN tab20.Tonelada > 30 and tab20.Tonelada <= 60 THEN 'AMARILLO'								
				ELSE 'ROJO'
				END as Color
	from
		(select 
				SUM(tab2.CantidadObservacion) as CantidadObservacion,
				'Despacho' as NombreTipoProgramacion,
				tab2.FechaInicio as FechaInicio, 
				SUM(tab2.Tonelada) as Tonelada
		from 
		   (select 
					Count(case when A.Observacion is not null then 1 end) as CantidadObservacion,
					A.IdTipoProgramacion, 
					C.Nombre as NombreTipoProgramacion,
					Convert(Date,A.FechaInicio) as FechaInicio,
					SUM(A.Tonelada) as Tonelada
			from PU.ProgramacionUnidad A
			left join PU.TipoProgramacion C on A.IdTipoProgramacion = C.Id
			where A.Activo = 1 and A.IdTipoProgramacion in (2,3)
					and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
					and FechaInicio between @beforeFecha and @afterFecha
			Group by A.IdTipoProgramacion, C.Nombre,Convert(Date,A.FechaInicio)) tab2
			Group by FechaInicio) tab20
	UNION ALL
	--Pdte Revision
	select 
		   tab3.CantidadObservacion,
	       tab3.NombreTipoProgramacion,
		   3 as idTipoEvento,
		   tab3.Tonelada, 
		   Convert(char(10),tab3.FechaInicio,126) as FechaInicio,
		   CASE 
				WHEN tab3.Tonelada <= 20 THEN 'VERDE'
			    WHEN tab3.Tonelada > 20 and tab3.Tonelada <= 40 THEN 'AMARILLO'
				ELSE 'ROJO' END as Color
	from 
		(select 
				Count(case when A.Observacion is not null then 1 end) as CantidadObservacion,
				A.IdTipoProgramacion, 
				'Revisado' as NombreTipoProgramacion,
				Convert(Date,A.FechaRevision) as FechaInicio,
				SUM(A.Tonelada) as Tonelada
		from PU.ProgramacionUnidad A
		left join PU.TipoProgramacion C on A.IdTipoProgramacion = C.Id
		where A.Activo = 1 and A.IdTipoProgramacion = 1 and A.IdEstado = 2 
				and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
				and FechaRevision between @beforeFecha and @afterFecha
		Group by A.IdTipoProgramacion,C.Nombre,Convert(Date,A.FechaRevision)) tab3
END
GO