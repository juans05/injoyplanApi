IF object_id('PU.Usp_CalendarioAgente') IS NOT NULL
     DROP PROC PU.Usp_CalendarioAgente
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_CalendarioAgente] --'2021-07-07','A_LAYMOQU1'
	@Fecha	datetime,
	@Sede varchar(100)
AS
BEGIN
	
	Declare @afterFecha datetime;
	Declare @beforeFecha datetime;

	set @afterFecha = DATEADD(MONTH,2,@Fecha);
	set @beforeFecha = DATEADD(MONTH,-1,@Fecha);

	select
			--Count(Id) as CantidadProgramacion,
			FechaInicio
	from 
			(select 
					A.Id,
					Convert(char(10),A.FechaInicio,126) as FechaInicio
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
			where FechaInicio between @beforeFecha and @afterFecha
				  and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
				  and A.Activo = 1
			Union
			select 
					A.Id,
					Convert(char(10),A.FechaInicio,126) as FechaInicio
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
			where FechaInicio between @beforeFecha and @afterFecha
				  and (A.IdAlmacenDestino in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
				  and A.Activo = 1
			) tab
			group by FechaInicio
			order by FechaInicio
END
GO

IF object_id('PU.Usp_ListProgramacionObservacionAgente') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionObservacionAgente
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListProgramacionObservacionAgente]
	@Fecha Datetime,
	@Sede varchar(100)
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
		  --and (A.IdAlmacen = @Sede or '0' = @Sede)
		  and (A.IdAlmacen in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
		  and A.Activo = 1
	UNION
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
		  --and (A.IdAlmacen = @Sede or '0' = @Sede)
		  and (A.IdAlmacenDestino in (SELECT Value FROM AUC.fn_Split(@Sede, ',')))
		  and A.Activo = 1
END
GO