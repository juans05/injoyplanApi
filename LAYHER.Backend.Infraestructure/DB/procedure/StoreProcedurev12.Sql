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

IF object_id('PU.Usp_ListProgramacionObservacion') IS NOT NULL
     DROP PROC PU.Usp_ListProgramacionObservacion
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ListProgramacionObservacion] --'2021-04-13','0'
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
		  and A.Activo = 1;
END
GO