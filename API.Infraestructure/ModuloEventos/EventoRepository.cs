using API.Domain.ModuloEventos.DTO;
using API.Domain.ModuloEventos.Interfaces;
using API.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infraestructure.ModuloEventos
{
    public class EventoRepository : IEventosRepository
    {
        protected readonly ICustomConnection mConnection;

        public EventoRepository(ICustomConnection _connection)
        {
            this.mConnection = _connection;
        }



        public async Task<List<Domain.ModuloEventos.DTO.Eventos>> listarEventos(Domain.ModuloEventos.DTO.Eventos entity)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusReponse<Domain.ModuloEventos.DTO.Eventos>> Save(Domain.ModuloEventos.DTO.Eventos entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos>("SP_GuardarEventos",
                    new
                    {
                        @titulo = entity.titulo,
                        @TipoEvento = entity.TipoEvento,
                        @referencia = entity.referencia,
                        @latitud_longitud = entity.latitud_longitud,
                        @tipoMoneda = entity.tipoMoneda,
                        @descripcionEvento = entity.descripcionEvento,
                        @ubigeo = entity.ubigeo,
                        @Departamento = entity.Departamento,
                        @Distrito = entity.Distrito,
                        @provincia = entity.Provincia,
                        @TituloLocal = entity.TituloLocal,
                        @DescripcionLocal = entity.DescripcionLocal,
                        @direccion = entity.direccion,
                        @Numero = entity.Numero,
                        @Destacado = (entity.Destacado) ? 1 : 0,
                        @urlFuente = entity.urlFuente

                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Domain.ModuloEventos.DTO.Eventos)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo Save");
                }
            }
            return status;
        }



        public async Task<StatusReponse<Eventos_Categoria>> SaveEventoCategoria(Eventos_Categoria entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos_Categoria> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos_Categoria>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Categoria>("SP_GuardarEventos_Categoria",
                    new
                    {
                        @ideventos = entity.id_Eventos,
                        @Idcategoria = entity.id_categoria



                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Domain.ModuloEventos.DTO.Eventos_Categoria)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo SaveEventoCategoria");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Evento_Entradas>> SaveEventoEntradas(Evento_Entradas entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Evento_Entradas> status = new StatusReponse<Domain.ModuloEventos.DTO.Evento_Entradas>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_Entradas>("SP_GuardarEventos_entradas",
                    new
                    {
                        @idEvento = entity.idEvento,
                        @idPlataforma = entity.idEntrada,
                        @url = entity.url,
                        @UrlWeb = entity.UrlWeb,

                    }, commandType: CommandType.StoredProcedure); ;
                    status.Data = (Domain.ModuloEventos.DTO.Evento_Entradas)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo SaveEventoEntradas");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Eventos_Fecha>> SaveEventoFecha(Eventos_Fecha entity, bool tipo)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos_Fecha> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos_Fecha>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var nuevaFecha = "";
                    if (tipo)
                    {
                        var FechasArray = entity.Fecha.Split('/');
                        var mes = FechasArray[0];
                        var dia = FechasArray[1];
                        var ano = FechasArray[2].Split(' ')[0];

                        nuevaFecha = ano + "-" + mes + "-" + dia;
                    }
                    else
                    {

                        nuevaFecha = entity.Fecha;
                    }


                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Fecha>("SP_GuardarEventos_fechas",
                    new
                    {
                        @idEvento = entity.idEvento,
                        @fecha = Convert.ToDateTime(nuevaFecha).ToString("yyyy-MM-dd"),
                        @horaInicio = entity.HoraInicio,
                        @HoraFinal = entity.HoraFin,
                        @precio = entity.monto,
                        @EventoLineaoPresencial = entity.EventoLineaoPresencial,
                        @cantvisita = entity.cantvisita
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Domain.ModuloEventos.DTO.Eventos_Fecha)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo SaveEventoFecha");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Evento_plataforma>> SaveEventoPlataforma(Evento_plataforma entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Evento_plataforma> status = new StatusReponse<Domain.ModuloEventos.DTO.Evento_plataforma>() { Success = false, Title = "" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_plataforma>("SP_GuardarEventos_platataforma",
                    new
                    {
                        @idEvento = entity.idEvento,
                        @idPlataforma = entity.idPlataforma,
                        @url = entity.url
                    }, commandType: CommandType.StoredProcedure);
                    status.Data = (Domain.ModuloEventos.DTO.Evento_plataforma)items.FirstOrDefault();
                    status.Success = true;
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo SaveEventoPlataforma");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Domain.ModuloEventos.DTO.Eventos>> update(Domain.ModuloEventos.DTO.Eventos entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos>() { Success = false, Title = "Error en la Actualizacón" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos>("SP_ActualizarEventos",
                    new
                    {
                        @idEvento = entity.id,
                        @titulo = entity.titulo,
                        @TipoEvento = entity.TipoEvento,
                        @referencia = entity.referencia,
                        @latitud_longitud = entity.latitud_longitud,
                        @tipoMoneda = entity.tipoMoneda,
                        @descripcionEvento = entity.descripcionEvento,
                        @ubigeo = entity.ubigeo,
                        @Departamento = entity.Departamento,
                        @Distrito = entity.Distrito,
                        @provincia = entity.Provincia,
                        @TituloLocal = entity.TituloLocal,
                        @DescripcionLocal = entity.DescripcionLocal,
                        @direccion = entity.direccion,
                        @Numero = entity.Numero,
                        @Destacado = (entity.Destacado) ? 1 : 0,
                        @urlFuente = entity.urlFuente
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se actualizo la cabecera del evento";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo Save");
                }
            }
            return status;
        }
        public async Task<StatusReponse<Domain.ModuloEventos.DTO.EventoImagen>> SaveArchivoAdjunto(Domain.ModuloEventos.DTO.EventoImagen entity)
        {
            StatusReponse<Domain.ModuloEventos.DTO.EventoImagen> status = new StatusReponse<Domain.ModuloEventos.DTO.EventoImagen>() { Success = false, Title = "Error al registrar" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.EventoImagen>("SP_GuardarAdjunto",
                    new
                    {
                        @idEvento = entity.idEvento,
                        @url = entity.url,
                        @tipo = entity.tipo,
                        @principal = entity.principal,
                        @consecutivoImg = entity.consecutivoImg

                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se actualizo la Archivo Adjunto con evento";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo Save");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Eventos_Categoria>> DeleteEventoCategoria(int id)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos_Categoria> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos_Categoria>() { Success = false, Title = "Error al Eliminar Categoria del evento" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Categoria>("SP_EliminarEvento_categoria",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se elimino el evento de la categoria";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo update Categoria");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Evento_Entradas>> DeleteEventoEntradas(int id)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Evento_Entradas> status = new StatusReponse<Domain.ModuloEventos.DTO.Evento_Entradas>() { Success = false, Title = "Error al Eliminar Entradas del evento" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_Entradas>("SP_EliminarEvento_Entradas",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se elimino el evento de la Entradas";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo update Entradas");
                }
            }
            return status;
        }




        public async Task<StatusReponse<Eventos_Fecha>> DeleteEventoFecha(int id)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos_Fecha> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos_Fecha>() { Success = false, Title = "Error al Eliminar Categoria del evento" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Fecha>("SP_DeleteEvento_Fecha",
                    new
                    {
                        @idEvento = id

                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se elimino el evento de la categoria";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo update Categoria");
                }
            }
            return status;
        }






        public async Task<StatusReponse<Domain.ModuloEventos.DTO.Eventos>> delete(int id)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Eventos> status = new StatusReponse<Domain.ModuloEventos.DTO.Eventos>() { Success = false, Title = "Error al Eliminar  evento" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos>("SP_Delete_Evento",
                    new
                    {

                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se elimino el evento";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo eliminar evento");
                }
            }
            return status;
        }

        public async Task<StatusReponse<Evento_plataforma>> DeleteEventoPlataforma(int id)
        {
            StatusReponse<Domain.ModuloEventos.DTO.Evento_plataforma> status = new StatusReponse<Domain.ModuloEventos.DTO.Evento_plataforma>() { Success = false, Title = "Error al Eliminar Plataforma del evento" };
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_plataforma>("SP_EliminarEvento_plataforma",
                    new
                    {

                        @idEvento = id

                    }, commandType: CommandType.StoredProcedure);
                    status.Success = true;
                    status.Title = "Se elimino el evento de la Plataforma";
                }
                catch (Exception e)
                {
                    throw new CustomException("Sucedió un error al realizar la operación en el metodo delete Plataforma");
                }
            }
            return status;
        }

        public async Task<Domain.ModuloEventos.DTO.Eventos> EditarEvento(int id)
        {
            Domain.ModuloEventos.DTO.Eventos entity = new Domain.ModuloEventos.DTO.Eventos();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {

                    //var items = await scope.QueryMultipleAsync("SP_EditarEvento",
                    //new
                    //{
                    //    @idEvento = id
                    //}, commandType: CommandType.StoredProcedure);
                    //entity = await items.ReadSingleAsync<Domain.ModuloEventos.DTO.Eventos>();
                    //if (items.Read<Domain.ModuloEventos.DTO.Eventos_Categoria>().ToList().Count>0)
                    //{
                    //    entity.EventoCategoria = items.Read<Domain.ModuloEventos.DTO.Eventos_Categoria>().ToList();

                    //}
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos>("SP_EditarEvento",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (Domain.ModuloEventos.DTO.Eventos)items.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - AdminListarEventos");
                }
            }
            return entity;
        }

        public async Task<List<Eventos_Categoria>> ConsultarEventoCategoria(int id)
        {

            List<Domain.ModuloEventos.DTO.Eventos_Categoria> entity = new List<Domain.ModuloEventos.DTO.Eventos_Categoria>();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Categoria>("Sp_ConsultarEventoCategoria",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (List<Domain.ModuloEventos.DTO.Eventos_Categoria>)items;

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - ConsultarEventoCategoria");
                }
            }
            return entity;
        }

        public async Task<List<Evento_plataforma>> ConsultarEventoPlataforma(int id)
        {
            List<Domain.ModuloEventos.DTO.Evento_plataforma> entity = new List<Domain.ModuloEventos.DTO.Evento_plataforma>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_plataforma>("SP_ConsultarEventoPlataforma",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (List<Domain.ModuloEventos.DTO.Evento_plataforma>)items;

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - ConsultarEventoPlataforma");
                }
            }
            return entity;
        }

        public async Task<List<Evento_Entradas>> ConsultarEventoEntradas(int id)
        {

            List<Domain.ModuloEventos.DTO.Evento_Entradas> entity = new List<Domain.ModuloEventos.DTO.Evento_Entradas>();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Evento_Entradas>("SP_ConsultarEventoEntradas",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (List<Domain.ModuloEventos.DTO.Evento_Entradas>)items;

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - ConsultarEventoEntradas");
                }
            }
            return entity;
        }

        public async Task<List<Eventos_Fecha>> ConsultarEventoFecha(int id)
        {
            List<Domain.ModuloEventos.DTO.Eventos_Fecha> entity = new List<Domain.ModuloEventos.DTO.Eventos_Fecha>();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.Eventos_Fecha>("SP_ConsultarEventoFecha",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (List<Eventos_Fecha>)items;

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - ConsultarEventoFecha");
                }
            }
            return entity;
        }


        public async Task<List<EventoImagen>> ConsultarEventoImagen(int id)
        {
            List<Domain.ModuloEventos.DTO.EventoImagen> entity = new List<Domain.ModuloEventos.DTO.EventoImagen>();
            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<Domain.ModuloEventos.DTO.EventoImagen>("SP_ConsultarEventoImagen",
                    new
                    {
                        @idEvento = id
                    }, commandType: CommandType.StoredProcedure);

                    entity = (List<EventoImagen>)items;

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - ConsultarEventoFecha");
                }
            }
            return entity;
        }

        public async Task<List<ListarEventosAdmin>> AdminListarEventos(int perfil, int usuario)
        {
            List<ListarEventosAdmin> entity = new List<ListarEventosAdmin>();

            using (var scope = await mConnection.BeginConnection())
            {
                try
                {
                    var items = await scope.QueryAsync<ListarEventosAdmin>("SP_ListarEventoAdmin",
                    new
                    {
                        @Perfil = perfil,
                        @usuario = usuario
                    }, commandType: CommandType.StoredProcedure);
                    entity = (List<ListarEventosAdmin>)items;
                }
                catch (Exception)
                {
                    throw new CustomException("Sucedió un error al realizar la operación - AdminListarEventos");
                }
            }
            return entity;
        }
    }
}
