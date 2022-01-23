using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.Mapper
{
    public static class GridReaderExtensions
    {
        public static IEnumerable<TFirst> Read<TFirst, TSecond>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return gridReader.Read(MappingCache<TFirst, TSecond>.Map, splitOn, buffered);
        }

        public static IEnumerable<TFirst> Read<TFirst, TSecond, TThird>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return gridReader.Read(MappingCache<TFirst, TSecond, TThird>.Map, splitOn, buffered);
        }

        public static IEnumerable<TFirst> Read<TFirst, TSecond, TThird, TFourth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth>.Map, splitOn, buffered);
        }

        public static IEnumerable<TFirst> Read<TFirst, TSecond, TThird, TFourth, TFifth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth, TFifth>.Map, splitOn, buffered);
        }

        public static IEnumerable<TFirst> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>.Map, splitOn, buffered);
        }

        // #if NETSTANDARD
        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond>.Map, splitOn, buffered));
        }

        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond, TThird>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond, TThird>.Map, splitOn, buffered));
        }

        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond, TThird, TFourth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth>.Map, splitOn, buffered));
        }

        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond, TThird, TFourth, TFifth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth, TFifth>.Map, splitOn, buffered));
        }

        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>.Map, splitOn, buffered));
        }

        public static Task<IEnumerable<TFirst>> ReadAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(this Dapper.SqlMapper.GridReader gridReader, string splitOn = "id", bool buffered = true)
        {
            return Task.Run(() => gridReader.Read(MappingCache<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>.Map, splitOn, buffered));
        }
        // #endif
    }
}
