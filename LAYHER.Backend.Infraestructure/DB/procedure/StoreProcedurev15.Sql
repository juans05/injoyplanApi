IF object_id('PU.Usp_GetFormularioProgramacion') IS NOT NULL
     DROP PROC PU.Usp_GetFormularioProgramacion
GO

CREATE OR ALTER PROCEDURE PU.Usp_GetFormularioProgramacion (  
@IdFormulario INT  
)  
AS  
BEGIN  
 SELECT Id as IdFormulario,
EsTransporteNacional,
EsTransportistaEncargado,
NumeroDocumento,
RazonSocial,
ModeloVehiculo,
PlacaTracto,
PlacaCarreta,
TelefonoContacto,
IdTipoDocumentoTransportista,
DocumentoTransportista,
NombreTransportista,
SctrTransportista,
TelefonoTransportista,
LicenciaTransportista,
IdTipoDocumentoEncargado,
DocumentoEncargado,
NombreEncargado,
SctrEncargado,
Consideracion,
IdUsuarioCreacion,
FechaCreacion,
IdUsuarioEdicion,
FechaEdicion FROM PU.Formulario WHERE Id=@IdFormulario;  
 SELECT * FROM PU.AdjuntoFormulario WHERE IdFormulario=@IdFormulario;  
 SELECT * FROM PU.FormularioConsideracion WHERE IdFormulario=@IdFormulario; 
 SELECT Id as IdArchivoAdjunto,
		IdFormulario,
		Nombre,
		Ruta
 FROM PU.ArchivoAdjunto WHERE IdFormulario = @IdFormulario;
END  
GO

--07132021
IF object_id('PU.Usp_ProgramacionUnidadPorId') IS NOT NULL
     DROP PROC PU.Usp_ProgramacionUnidadPorId
GO

CREATE OR ALTER PROCEDURE [PU].[Usp_ProgramacionUnidadPorId]
	@ProgramacionUnidad int
AS
BEGIN
	select 
			B.Nombre as NombreEstado,C.Metro as Longitud,D.Nombre as NombreUnidad,F.DescripcionLocal,I.NombreCompleto,
			K.Nombre as NombreTipoProgramacion,L.NombreCompleto as NombreUsuarioCreacion,
			J.Id as IdFormulario, J.NumeroDocumento, J.RazonSocial,J.ModeloVehiculo,J.PlacaTracto,J.PlacaCarreta,
			H.localname as NombreProyecto,A.Id as IdProgramacionUnidad,A.*,
			H.id_ejecutivo as IdComercial, N.NombreCompleto as NombreComercial,
			P.vc_idContrato as IdContrato, P.vc_DescripContrato as NombreContrato
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
	left join GL_MaestroPersonas N on H.id_ejecutivo = N.Persona
	left join FC_MaestroContratos P on H.afe = P.vc_idProyecto
	left join PU.TipoProgramacion K on A.IdTipoProgramacion = k.Id
	left join GL_MaestroPersonas L on A.IdUsuarioCreacion = L.Persona
	where (A.Id = @ProgramacionUnidad or 0 = @ProgramacionUnidad)
		  and A.Activo = 1;
END
GO