IF object_id('AUC.USP_OBTENER_CLIENTE_POR_DOCUMENTO') IS NOT NULL
     DROP PROC AUC.USP_OBTENER_CLIENTE_POR_DOCUMENTO
GO

CREATE PROCEDURE [AUC].[USP_OBTENER_CLIENTE_POR_DOCUMENTO](
@NroDocumento VARCHAR(50)
)
AS
BEGIN
	SELECT CL.Persona as Id,
		CL.Documento,
		CL.NombreCompleto,
		ISNULL(CL.CorreoElectronicoFE, CL.CorreoElectronico) AS CorreoElectronico,
		Clave
	FROM GL_MaestroPersonas CL 
	WHERE Estado='A' AND EsCliente='S' AND Documento=@NroDocumento;
END
GO