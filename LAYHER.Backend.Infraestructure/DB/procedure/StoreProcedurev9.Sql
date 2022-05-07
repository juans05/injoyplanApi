IF object_id('AUC.Usp_ObtenerUsuarioInternoPorId') IS NOT NULL
     DROP PROC AUC.Usp_ObtenerUsuarioInternoPorId
GO
CREATE PROCEDURE [AUC].[Usp_ObtenerUsuarioInternoPorId]  
 @Persona INT
AS  
BEGIN  
SELECT  
     MP.Persona,  
     MP.Documento,  
	 MP.Nombres,  
	 MP.ApellidoPaterno,  
	 MP.ApellidoMaterno,  
	 RTRIM(LTRIM(MP.NombreCompleto)) AS NombreCompleto,  
     MP.CorreoElectronico
FROM GL_MaestroPersonas MP
WHERE TipoDocumento='D' AND EsEmpleado='S' AND MP.Persona > 0 AND MP.Persona = @Persona
END  
GO

IF object_id('AUC.Usp_ListaPerfilesxDocumento') IS NOT NULL
     DROP PROC AUC.Usp_ListaPerfilesxDocumento
GO

CREATE OR ALTER PROCEDURE [AUC].[Usp_ListaPerfilesxDocumento]
	@Documento varchar(50)
AS
BEGIN
	Declare @Respuesta bit
	set @Respuesta = 0
	Declare @Persona int
	set @Persona = (select Persona from GL_MaestroPersonas where Documento = @Documento);

	IF exists(select IdPerfil from SEG.PerfilArea 
						where IdPerfilArea in (select IdPerfilArea from SEG.UsuarioPerfilArea where Usuario = @Persona))
		BEGIN
			Select D.IdPerfil, D.Nombre 
			From GL_MaestroPersonas A 
			Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
			Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
			Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
			Where A.Documento = @Documento;

			set @Respuesta = 1
		END
	IF (@Respuesta = 0)
		BEGIN
			Declare @PersonaCliente int
			set @PersonaCliente = (select vc_UsuarioCreador from GL_MaestroPersonas where Documento = @Documento)
			Declare @PerfilAreaEmpresa int
			set @PerfilAreaEmpresa = (select IdPerfilArea from SEG.UsuarioPerfilArea where Usuario = @PersonaCliente)
			Declare @PerfilEmpresa int
			set @PerfilEmpresa = (select IdPerfil from SEG.PerfilArea where IdPerfilArea = @PerfilAreaEmpresa and IdPerfil = 91)
			Declare @PerfilAUC int
			set @PerfilAUC = (select IdPerfil from AUC.Perfil_Persona where IdPersona = @Persona and Activo = 1)
			
			IF (@PerfilEmpresa = 91 and @PerfilAUC = 5)
				BEGIN
					Select -91 as IdPerfil, D.Nombre 
					From GL_MaestroPersonas A 
					Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
					Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
					Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
					Where A.Persona = @PersonaCliente;
				END
			ELSE
				BEGIN
					Select D.IdPerfil, D.Nombre 
					From GL_MaestroPersonas A 
					Join SEG.UsuarioPerfilArea B on A.Persona = B.Usuario 
					Join SEG.PerfilArea C on B.IdPerfilArea = C.IdPerfilArea 
					Join SEG.Perfil D on C.IdPerfil = D.IdPerfil 
					Where A.Documento = @Documento;
				END
		END
END
GO

IF object_id('SEG.USP_PERSONAALMACEN_LISTAR_X_PERSONA') IS NOT NULL
     DROP PROC SEG.USP_PERSONAALMACEN_LISTAR_X_PERSONA
GO

CREATE   PROCEDURE [SEG].[USP_PERSONAALMACEN_LISTAR_X_PERSONA]  
(  
@IdPersona int  
)  
AS  
BEGIN  
 SELECT  
  PA.IdPersonaAlmacen,  
  PA.IdPersona,  
  PA.AlmacenCodigo,  
  RTRIM(LTRIM(REPLACE(MA.DescripcionLocal, 'LAYHER', ''))) AlmacenNombre,
  MA.Latitud,
  MA.Longitud
 FROM SEG.PersonaAlmacen PA  
 INNER JOIN LG_MaestroAlmacen MA ON PA.AlmacenCodigo = MA.AlmacenCodigo  
 WHERE PA.IdPersona = @IdPersona  
END  
GO

IF object_id('COMUN.USP_LISTARALMACENES') IS NOT NULL
     DROP PROC COMUN.USP_LISTARALMACENES
GO

CREATE   PROCEDURE [COMUN].[USP_LISTARALMACENES]  
AS  
BEGIN  
 SELECT Distinct  
  PA.AlmacenCodigo,  
  RTRIM(LTRIM(REPLACE(MA.DescripcionLocal, 'LAYHER', ''))) AlmacenNombre,
  MA.Latitud,
  MA.Longitud
 FROM SEG.PersonaAlmacen PA  
 INNER JOIN LG_MaestroAlmacen MA ON PA.AlmacenCodigo = MA.AlmacenCodigo  
END  