ALTER TABLE LG_MaestroAlmacen
ADD Latitud VARCHAR(50)
GO

ALTER TABLE LG_MaestroAlmacen
ADD Longitud VARCHAR(50)
GO

UPDATE LG_MaestroAlmacen SET Latitud='-12.29018309', Longitud='-76.84036446' WHERE AlmacenCodigo='A_LAYHER01';
UPDATE LG_MaestroAlmacen SET Latitud='-16.36939968', Longitud='-71.56880387' WHERE AlmacenCodigo='A_LAYAREQ1';
UPDATE LG_MaestroAlmacen SET Latitud='-3.72827895', Longitud='-73.24078238' WHERE AlmacenCodigo='A_LAYLORE1';
UPDATE LG_MaestroAlmacen SET Latitud='-17.19193555', Longitud='-70.9523044' WHERE AlmacenCodigo='A_LAYMOQU1';
UPDATE LG_MaestroAlmacen SET Latitud='-5.15725602', Longitud='-80.72503865' WHERE AlmacenCodigo='A_LAYPIUR1';
UPDATE LG_MaestroAlmacen SET Latitud='-8.120562', Longitud='-79.04609923' WHERE AlmacenCodigo='A_LAYTRUJ1';
GO