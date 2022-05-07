UPDATE INSPECCION.EstadoInspeccionAndamio
SET Nombre = 'Borrador' WHERE EstadoInspeccionAndamio_id = 1;
GO

INSERT INTO INSPECCION.EstadoInspeccionAndamio (Nombre,IdUsuarioCreacion,FechaCreacion,Activo)
VALUES ('Histórico',1,GETDATE(),1);
GO