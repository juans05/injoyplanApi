ALTER TABLE PU.ProgramacionUnidad ADD UnidadRecibida bit null
ALTER TABLE PU.ProgramacionUnidad ADD Conforme bit null

ALTER TABLE PU.Formulario ADD EsTransportistaEncargado bit null

--04062021
insert into PU.TipoEstado(Nombre,Activo,IdUsuarioCreacion,FechaCreacion) values('Descarga',1,1,GETDATE());
insert into PU.ProgramacionEstado(IdTipoProgramacion,IdTipoEstado,IdUsuarioCreacion,FechaCreacion) values(1,6,1,GETDATE());