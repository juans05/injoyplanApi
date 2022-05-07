update AUC.Permiso
	set Activo = 0 where IdPermiso in (1,2,3,4,5,6,7,8,9,10)

update AUC.Perfil_Permiso
	set Activo = 0

insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) 
values(11,'Programación de unidades',1,1,GETDATE());
insert into AUC.Permiso(IdPermiso,Nombre,Activo,IdUsuarioCreacion,FechaCreacion) 
values(12,'Formato de evaluación',1,1,GETDATE());