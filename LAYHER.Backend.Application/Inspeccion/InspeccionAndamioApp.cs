using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using LAYHER.Backend.Domain.Inspeccion.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Logging;
using LAYHER.Backend.Application.Shared;
using LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO;
using MimeTypes;
using System.IO;

namespace LAYHER.Backend.Application.Inspeccion
{
    public class InspeccionAndamioApp : BaseApp<Object>
    {
        private IInspeccionAndamioRepository _InspeccionAndamioRepository;
        private ICheckListRepository _CheckListRepository;
        private ICumplimientoRepository _CumplimientoRepository;
        private IRegistroFotograficoRepository _RegistroFotograficoRepository;
        private IConfiguracionAppRepository _ConfiguracionAppRepository;

        public InspeccionAndamioApp(IInspeccionAndamioRepository inspeccionAndamioRepository,
            ICheckListRepository checkListRepository,
            ICumplimientoRepository cumplimientoRepository,
            IRegistroFotograficoRepository registroFotograficoRepository,
            IConfiguracionAppRepository configuracionAppRepository
           ) : base()
        {
            this._InspeccionAndamioRepository = inspeccionAndamioRepository;
            this._CheckListRepository = checkListRepository;
            this._CumplimientoRepository = cumplimientoRepository;
            this._RegistroFotograficoRepository = registroFotograficoRepository;
            this._ConfiguracionAppRepository = configuracionAppRepository;
        }
        public async Task<StatusReponse<List<InspeccionAndamio>>> ListarInspeccionAndamio(
            string nombreProyecto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int offset,
            int fetch,
            bool? verHistorial,
            bool modoHistorico = false,
            int cantidadAnios = 3)
        {
            return await this.ComplexResponse(() => _InspeccionAndamioRepository.ListarInspeccionAndamio(nombreProyecto, fechaInicio, fechaFin, offset, fetch, verHistorial, modoHistorico, cantidadAnios));
        }

        public async Task<StatusReponse<InspeccionAndamio>> Registrar(InspeccionAndamio inspeccionAndamio)
        {
            StatusReponse<InspeccionAndamio> respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>() { Success = false, Title = "" };
            StatusReponse<CheckList> respuestaCheckList = new StatusReponse<CheckList>() { Success = false, Title = "" };
            StatusReponse<Cumplimiento> respuestaCumplimiento = new StatusReponse<Cumplimiento>() { Success = false, Title = "" };
            StatusReponse<RegistroFotografico> respuestaRegistroFotografico = new StatusReponse<RegistroFotografico>() { Success = false, Title = "" };
            bool actualizar = false;
            Dictionary<string, List<string>> errores = new Dictionary<string, List<string>>();
            List<string> listaErroresInspeccion = new List<string>();
            List<string> listaErroresCheckList = new List<string>();
            List<string> listaErroresCumplimiento = new List<string>();
            bool validacion1 = false;
            bool validacion2 = false;
            InspeccionAndamio inspeccionAndamioActual = null;
            DateTime fechaAhora = DateTime.Now;

            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (inspeccionAndamio.interfaz == 0 || inspeccionAndamio.interfaz > 3)
                        listaErroresInspeccion.Add("El valor de interfaz no es válido");

                    if (inspeccionAndamio.InspeccionAndamio_id > 0 || inspeccionAndamio.interfaz == 3)
                    {
                        actualizar = true;
                        inspeccionAndamio.IdUsuarioEdicion = inspeccionAndamio.IdUsuario;
                        inspeccionAndamio.FechaEdicion = fechaAhora;
                        if (inspeccionAndamio.InspeccionAndamio_id == 0)
                        {
                            listaErroresInspeccion.Add("El valor InspeccionAndamio_id no es válido");
                        }
                        else
                        {
                            inspeccionAndamioActual = await _InspeccionAndamioRepository.Obtener(inspeccionAndamio.InspeccionAndamio_id, true);
                            if (inspeccionAndamioActual.InspeccionAndamio_id == 0)
                                listaErroresInspeccion.Add(string.Format("La inspección {0} no existe", inspeccionAndamio.InspeccionAndamio_id));
                        }
                    }
                    else
                    {
                        inspeccionAndamio.Activo = true;
                        inspeccionAndamio.IdUsuarioCreacion = inspeccionAndamio.IdUsuario;
                        inspeccionAndamio.FechaCreacion = fechaAhora;
                    }
                    if (inspeccionAndamio.interfaz == 1)
                    {
                        if (actualizar)
                            inspeccionAndamio.strMode = "U1";
                        else
                            inspeccionAndamio.strMode = "C1";

                        validacion1 = true;
                    }
                    if (inspeccionAndamio.interfaz == 2)
                    {
                        if (actualizar)
                        {
                            inspeccionAndamio.strMode = "U2";
                        }
                        else
                        {
                            inspeccionAndamio.strMode = "C2";
                            validacion1 = true;
                        }
                        validacion2 = true;
                    }
                    if (inspeccionAndamio.interfaz == 3)
                    {
                        inspeccionAndamio.strMode = "U3";
                        validacion1 = true;
                        validacion2 = true;
                    }
                    if (validacion1)
                    {
                        if (inspeccionAndamio.EstadoInspeccionAndamio_id != (int)EEstadoInspeccionAndamio.Borrador
                            && inspeccionAndamio.EstadoInspeccionAndamio_id != (int)EEstadoInspeccionAndamio.Completado)
                            listaErroresInspeccion.Add("El campo EstadoInspeccionAndamio_id no es válido");

                        if (string.IsNullOrEmpty(inspeccionAndamio.Proyecto))
                        {
                            listaErroresInspeccion.Add("El campo Proyecto no es válido");
                        }
         
                        if (string.IsNullOrEmpty(inspeccionAndamio.Direccion) || inspeccionAndamio.Direccion.Length > 400)
                            listaErroresInspeccion.Add("El campo Direccion no es válido");
                        if (string.IsNullOrEmpty(inspeccionAndamio.ZonaTrabajo) || inspeccionAndamio.ZonaTrabajo.Length > 400)
                            listaErroresInspeccion.Add("El campo ZonaTrabajo no es válido");
                        if (string.IsNullOrEmpty(inspeccionAndamio.Responsable) || inspeccionAndamio.Responsable.Length > 400)
                            listaErroresInspeccion.Add("El campo Responsable no es válido");
                        if (string.IsNullOrEmpty(inspeccionAndamio.Cargo) || inspeccionAndamio.Cargo.Length > 400)
                            listaErroresInspeccion.Add("El campo Cargo no es válido");
                        if (string.IsNullOrEmpty(inspeccionAndamio.MarcaAndamio) || inspeccionAndamio.MarcaAndamio.Length > 400)
                            listaErroresInspeccion.Add("El campo MarcaAndamio no es válido");
                    }
                    if (validacion2)
                    {
                        if (string.IsNullOrEmpty(inspeccionAndamio.SobreCargaUso))
                            listaErroresInspeccion.Add("El campos SobreCargaUso no es válido");

                        if (!string.IsNullOrEmpty(inspeccionAndamio.Observacion))
                        {
                            if (inspeccionAndamio.Observacion.Length > 400)
                                listaErroresInspeccion.Add("El campo Observacion no es válido");
                        }

                        CheckList checkListTemp = null;
                        CheckList CheckListActual = null;
                        Cumplimiento cumplimientoTemp = null;
                        SubTipoAndamio subTipo = null;
                        List<SubTipoAndamio> listaSubTipoAndamio = await _InspeccionAndamioRepository.ListarSubTipoAndamios();

                        if (inspeccionAndamio.ListaCheckList != null && inspeccionAndamio.ListaCheckList.Count > 0)
                        {
                            for (int i = 0; i < inspeccionAndamio.ListaCheckList.Count; i++)
                            {
                                checkListTemp = inspeccionAndamio.ListaCheckList[i];
                                if (checkListTemp.CheckList_id > 0)
                                {
                                    if (inspeccionAndamio.InspeccionAndamio_id > 0)
                                    {
                                        CheckListActual = null;
                                        CheckListActual = await _CheckListRepository.Obtener(checkListTemp.CheckList_id, inspeccionAndamio.InspeccionAndamio_id);
                                        if (CheckListActual == null || CheckListActual.CheckList_id == 0)
                                            listaErroresCheckList.Add(string.Format("CheckList[{0}] El checkList {1} no existe", i, checkListTemp.CheckList_id));
                                    }
                                    else
                                    {
                                        listaErroresInspeccion.Add(string.Format("El campo InspeccionAndamio_id debe ser incluido para procesar el checkList[{0}] CheckList_id:{1}", i, checkListTemp.CheckList_id));
                                    }
                                }
                                if (checkListTemp.TipoAndamio_id == 0)
                                {
                                    listaErroresCheckList.Add(string.Format("CheckList[{0}] Campo TipoAndamio_id no es válido", i));
                                }
                                else
                                {
                                    List<SubTipoAndamio> listaSubTipoAndamioActual = listaSubTipoAndamio.FindAll(s => s.TipoAndamio_id == checkListTemp.TipoAndamio_id);
                                    if (listaSubTipoAndamioActual.Count > 0)
                                    {
                                        if (checkListTemp.SubTipoAndamio_id == 0)
                                        {
                                            listaErroresCheckList.Add(string.Format("CheckList[{0}] Campo SubTipoAndamio_id no es válido", i));
                                        }
                                        else
                                        {
                                            subTipo = null;
                                            subTipo = listaSubTipoAndamioActual.Find(s => s.SubTipoAndamio_id == checkListTemp.SubTipoAndamio_id);
                                            if (subTipo == null)
                                                listaErroresCheckList.Add(string.Format("CheckList[{0}] Campo SubTipoAndamio_id {1} no es válido", i, checkListTemp.SubTipoAndamio_id));
                                        }
                                    }
                                    else
                                    {
                                        if (checkListTemp.SubTipoAndamio_id > 0)
                                            listaErroresCheckList.Add(string.Format("CheckList[{0}] Campo SubTipoAndamio_id {1} no es un sub tipo válido", i, checkListTemp.SubTipoAndamio_id));
                                    }
                                }
                                if (checkListTemp.ListaPreguntaInspeccion != null && checkListTemp.ListaPreguntaInspeccion.Count > 0)
                                {
                                    for (int j = 0; j < checkListTemp.ListaPreguntaInspeccion.Count; j++)
                                    {
                                        cumplimientoTemp = checkListTemp.ListaPreguntaInspeccion[j].Cumplimiento;
                                        if (cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.Si
                                            && cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.No
                                            && cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.NA)
                                            listaErroresCumplimiento.Add(string.Format("CheckList[{0}] Cumplimiento[{1}] Campo RespuestaCumplimiento_id no valido", i, j));

                                        if (cumplimientoTemp.PreguntaInspeccionAndamio_id == 0)
                                            listaErroresCumplimiento.Add(string.Format("CheckList[{0}] Cumplimiento[{1}] Campo PreguntaInspeccionAndamio_id no valido", i, j));
                                    }
                                }
                            }
                        }
                    }
                    if (listaErroresInspeccion.Count > 0)
                        errores.Add("InspeccionAndamio", listaErroresInspeccion);

                    if (listaErroresCheckList.Count > 0)
                        errores.Add("CheckList", listaErroresCheckList);

                    if (listaErroresCumplimiento.Count > 0)
                        errores.Add("Cumplimiento", listaErroresCumplimiento);

                    if (errores.Count == 0)
                    {
                        CheckList checkListTemp = null;
                        respuestaInspeccionAndamio = await _InspeccionAndamioRepository.Registrar(inspeccionAndamio);
                        if (respuestaInspeccionAndamio.Success || validacion2)
                        {
                            if (inspeccionAndamio.ListaCheckList != null && inspeccionAndamio.ListaCheckList.Count > 0)
                            {
                                if (actualizar)
                                {
                                    foreach (CheckList checkList in inspeccionAndamioActual.ListaCheckList)
                                    {
                                        checkListTemp = null;
                                        checkListTemp = inspeccionAndamio.ListaCheckList.Find(c => c.CheckList_id == checkList.CheckList_id);
                                        if (checkListTemp == null || checkListTemp.CheckList_id == 0)
                                        {
                                            //eliminar checklist y cumplimiento
                                            await _CheckListRepository.Eliminar(checkList.CheckList_id);
                                        }
                                    }
                                }

                                foreach (CheckList checkList in inspeccionAndamio.ListaCheckList)
                                {
                                    if (checkList.CheckList_id > 0)
                                    {
                                        checkList.IdUsuarioEdicion = inspeccionAndamio.IdUsuario;
                                        checkList.FechaEdicion = fechaAhora;
                                        checkList.strMode = "U";
                                    }
                                    else
                                    {
                                        checkList.IdUsuarioCreacion = inspeccionAndamio.IdUsuario;
                                        checkList.FechaCreacion = fechaAhora;
                                        checkList.strMode = "C";
                                        checkList.Activo = true;
                                    }
                                    if (actualizar)
                                        checkList.InspeccionAndamio_id = inspeccionAndamioActual.InspeccionAndamio_id;
                                    else
                                        checkList.InspeccionAndamio_id = respuestaInspeccionAndamio.Data.InspeccionAndamio_id;
                                    respuestaCheckList = await _CheckListRepository.Registrar(checkList);
                                    if (respuestaCheckList.Success)
                                    {
                                        if (checkList.ListaPreguntaInspeccion != null && checkList.ListaPreguntaInspeccion.Count > 0)
                                        {
                                            if (checkList.CheckList_id > 0)
                                                await _CumplimientoRepository.Eliminar(checkList.CheckList_id);

                                            foreach (PreguntaInspeccion preguntaInspeccion in checkList.ListaPreguntaInspeccion)
                                            {
                                                Cumplimiento cumplimiento = preguntaInspeccion.Cumplimiento;
                                                if (checkList.strMode == "U")
                                                    cumplimiento.CheckList_id = checkList.CheckList_id;
                                                else
                                                    cumplimiento.CheckList_id = respuestaCheckList.Data.CheckList_id;

                                                cumplimiento.IdUsuarioCreacion = inspeccionAndamio.IdUsuario;
                                                cumplimiento.FechaCreacion = fechaAhora;
                                                cumplimiento.Activo = true;
                                                respuestaCumplimiento = await _CumplimientoRepository.Registrar(cumplimiento);
                                                if (!respuestaCumplimiento.Success)
                                                    return respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>(respuestaCumplimiento.Detail, errores);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>(respuestaCheckList.Detail, errores);
                                    }
                                }
                            }
                            if (inspeccionAndamio.ListaRegistroFotografico != null && inspeccionAndamio.ListaRegistroFotografico.Count > 0)
                            {
                                foreach (RegistroFotografico foto in inspeccionAndamio.ListaRegistroFotografico)
                                {
                                    if (foto.RegistroFotografico_id > 0)
                                    {
                                        foto.strMode = "U";
                                        foto.FechaEdicion = fechaAhora;
                                        foto.IdUsuarioEdicion = inspeccionAndamio.IdUsuario;
                                    }
                                    else
                                    {
                                        foto.strMode = "C";
                                        foto.FechaCreacion = fechaAhora;
                                        foto.IdUsuarioCreacion = inspeccionAndamio.IdUsuario;
                                        foto.Activo = true;
                                    }
                                    if (foto.Nombre.StartsWith("/tmp/"))
                                    {
                                        foto.NombreArchivoTemporal = foto.Nombre;
                                        foto.Nombre = foto.Nombre.Substring(5);
                                    }
                                    if (actualizar)
                                        foto.InspeccionAndamio_id = inspeccionAndamioActual.InspeccionAndamio_id;
                                    else
                                        foto.InspeccionAndamio_id = respuestaInspeccionAndamio.Data.InspeccionAndamio_id;

                                    respuestaRegistroFotografico = await _RegistroFotograficoRepository.Registrar(foto);
                                    if (!respuestaRegistroFotografico.Success)
                                        return respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>(respuestaRegistroFotografico.Detail, errores);
                                }

                            }
                            if (inspeccionAndamio.ListaRegistroFotograficoEliminado != null && inspeccionAndamio.ListaRegistroFotograficoEliminado.Count > 0)
                            {
                                foreach (RegistroFotografico foto in inspeccionAndamio.ListaRegistroFotograficoEliminado)
                                {
                                    if (!foto.Nombre.StartsWith("/tmp/"))
                                    {
                                        await _RegistroFotograficoRepository.Eliminar(foto.RegistroFotografico_id);
                                    }
                                }
                            }
                        }
                        tran.Complete();
                    }
                    else
                    {
                        respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>("Errores de validación en registro de inspección", errores);
                    }
                }
                catch (Exception ex)
                {
                    respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>("Error inesperado en registro de inspección", errores);
                    this._logger.LogError(1, ex, "Error en registro de inspección", inspeccionAndamio);
                }
            }
            return respuestaInspeccionAndamio;
        }

        public async Task<StatusReponse<List<PreguntaInspeccion>>> ListarPreguntaInspeccionAndamio(int tipoAndamio_id, int? subTipoAndamio_id)
        {
            StatusReponse<List<PreguntaInspeccion>> status = null;
            if (tipoAndamio_id == 0)
            {
                status = new StatusReponse<List<PreguntaInspeccion>>(false, "Debe especificar un tipo de andamio");
                return status;
            }

            status = await this.ComplexResponse(() => _InspeccionAndamioRepository.ListarPreguntaInspeccionAndamio(tipoAndamio_id, subTipoAndamio_id));
            return status;
        }

        public async Task<StatusReponse<List<SubTipoAndamio>>> ListarSubTipoAndamios()
        {
            return await this.ComplexResponse(() => _InspeccionAndamioRepository.ListarSubTipoAndamios());
        }

        public async Task<StatusReponse<List<TipoAndamio>>> ListarTipoAndamio()
        {
            return await this.ComplexResponse(() => _InspeccionAndamioRepository.ListarTipoAndamio());
        }

        public async Task<StatusReponse<InspeccionAndamio>> Obtener(int inspeccionAndamio_id)
        {
            return await this.ComplexResponse(() => _InspeccionAndamioRepository.Obtener(inspeccionAndamio_id, true, true));
        }

        public async Task<StatusReponse<ReporteInspeccion>> ObtenerParaReporte(int inspeccionAndamio_id, string folderRegistroFotografico)
        {
            StatusReponse<ReporteInspeccion> status = await this.ComplexResponse(() => _InspeccionAndamioRepository.ObtenerParaReporte(inspeccionAndamio_id));
            if (status.Data != null)
            {
                if (status.Data.ListaRegistroFotografico != null && status.Data.ListaRegistroFotografico.Count > 0)
                {
                    List<OutRegistroFotografico> lista = new List<OutRegistroFotografico>();
                    int total = status.Data.ListaRegistroFotografico.Count;
                    int esImpar = total % 2;
                    for (int i = 0; i < total; i++)
                    {
                        OutRegistroFotografico item = new OutRegistroFotografico();
                        lista.Add(item);

                        item.PathImagen1 = "file:///" + folderRegistroFotografico + status.Data.ListaRegistroFotografico[i].Nombre;
                        string extension = Path.GetExtension(item.PathImagen1);
                        item.MimeTypeImage1 = MimeTypeMap.GetMimeType(extension);
                        item.Descripcion1 = status.Data.ListaRegistroFotografico[i].Descripcion;
                        i++;

                        if (i == total && esImpar > 0)
                        {
                            item.PathImagen2 = "";
                            item.Descripcion2 = "";
                            break;
                        }

                        item.PathImagen2 = status.Data.ListaRegistroFotografico[i].NombreOriginal;
                        item.Descripcion2 = status.Data.ListaRegistroFotografico[i].Descripcion;
                    }

                    status.Data.Fotos = lista;
                }

                if(status.Data.Fotos == null)
                    status.Data.Fotos = new List<OutRegistroFotografico>();
            }
            return status;
        }

        public async Task<StatusReponse<List<Cliente>>> ListarCliente(string documentoUsuario)
        {
            StatusReponse<List<Cliente>> status = null;
            if (string.IsNullOrEmpty(documentoUsuario))
            {
                status = new StatusReponse<List<Cliente>>(false, "Debe especificar el nro. de documento del usuario.");
                return status;
            }
            status = await this.ComplexResponse(() => _InspeccionAndamioRepository.ListarCliente(documentoUsuario));
            return status;
        }

        public async Task<StatusReponse<OutCheckList>> ObtenerCheckList(int checkList_id)
        {
            return await this.ComplexResponse(() => _CheckListRepository.Obtener(checkList_id, null, true));
        }

        public async Task<StatusReponse<CheckList>> RegistrarCheckList(CheckList checkList)
        {
            StatusReponse<CheckList> respuestaCheckList = new StatusReponse<CheckList>() { Success = false, Title = "" };
            StatusReponse<Cumplimiento> respuestaCumplimiento = new StatusReponse<Cumplimiento>() { Success = false, Title = "" };
            Dictionary<string, List<string>> errores = new Dictionary<string, List<string>>();
            List<string> listaErroresCheckList = new List<string>();
            List<string> listaErroresCumplimiento = new List<string>();
            InspeccionAndamio inspeccionAndamioActual = null;
            CheckList CheckListActual = null;
            Cumplimiento cumplimientoTemp = null;
            DateTime fechaAhora = DateTime.Now;
            bool actualizar = false;
            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (checkList.InspeccionAndamio_id == 0)
                    {
                        listaErroresCheckList.Add("Campo InspeccionAndamio_id no valido");
                    }
                    else
                    {
                        inspeccionAndamioActual = await _InspeccionAndamioRepository.Obtener(checkList.InspeccionAndamio_id, true);
                        if (inspeccionAndamioActual == null || inspeccionAndamioActual.InspeccionAndamio_id == 0)
                        {
                            listaErroresCheckList.Add(string.Format("La inspeccion {0} no existe", checkList.InspeccionAndamio_id));
                        }
                        else
                        {
                            if (checkList.CheckList_id > 0)
                            {
                                actualizar = true;
                                CheckListActual = await _CheckListRepository.Obtener(checkList.CheckList_id, checkList.InspeccionAndamio_id);
                                if (CheckListActual == null || CheckListActual.CheckList_id == 0)
                                    listaErroresCheckList.Add(string.Format("El checkList {0} no existe", checkList.CheckList_id));
                            }
                            if (checkList.TipoAndamio_id == 0)
                            {
                                listaErroresCheckList.Add("Campo TipoAndamio_id no es válido");
                            }
                            else
                            {
                                List<SubTipoAndamio> listaSubTipoAndamio = await _InspeccionAndamioRepository.ListarSubTipoAndamios();
                                List<SubTipoAndamio> listaSubTipoAndamioActual = listaSubTipoAndamio.FindAll(s => s.TipoAndamio_id == checkList.TipoAndamio_id);
                                SubTipoAndamio subTipo = null;
                                if (listaSubTipoAndamioActual.Count > 0)
                                {
                                    if (checkList.SubTipoAndamio_id == 0)
                                    {
                                        listaErroresCheckList.Add("Campo SubTipoAndamio_id no es válido");
                                    }
                                    else
                                    {
                                        subTipo = null;
                                        subTipo = listaSubTipoAndamioActual.Find(s => s.SubTipoAndamio_id == checkList.SubTipoAndamio_id);
                                        if (subTipo == null)
                                            listaErroresCheckList.Add(string.Format("Campo SubTipoAndamio_id {0} no es válido", checkList.SubTipoAndamio_id));
                                    }
                                }
                                else
                                {
                                    if (checkList.SubTipoAndamio_id > 0)
                                        listaErroresCheckList.Add(string.Format("Campo SubTipoAndamio_id {0} no es un sub tipo válido", checkList.SubTipoAndamio_id));
                                }
                            }
                            if (checkList.ListaPreguntaInspeccion != null && checkList.ListaPreguntaInspeccion.Count > 0)
                            {
                                for (int i = 0; i < checkList.ListaPreguntaInspeccion.Count; i++)
                                {
                                    cumplimientoTemp = checkList.ListaPreguntaInspeccion[i].Cumplimiento;
                                    if (cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.Si
                                        && cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.No
                                        && cumplimientoTemp.RespuestaCumplimiento_id != (int)ERespuestaCumplimiento.NA)
                                        listaErroresCumplimiento.Add(string.Format("Cumplimiento[{0}] Campo RespuestaCumplimiento_id no valido", i));

                                    if (cumplimientoTemp.PreguntaInspeccionAndamio_id == 0)
                                        listaErroresCumplimiento.Add(string.Format("Cumplimiento[{0}] Campo PreguntaInspeccionAndamio_id no valido", i));
                                }
                            }
                        }
                    }
                    if (listaErroresCheckList.Count > 0)
                        errores.Add("CheckList", listaErroresCheckList);

                    if (listaErroresCumplimiento.Count > 0)
                        errores.Add("Cumplimiento", listaErroresCumplimiento);

                    if (errores.Count == 0)
                    {
                        if (actualizar)
                        {
                            checkList.IdUsuarioEdicion = checkList.IdUsuario;
                            checkList.FechaEdicion = fechaAhora;
                            checkList.strMode = "U";
                        }
                        else
                        {
                            checkList.IdUsuarioCreacion = checkList.IdUsuario;
                            checkList.FechaCreacion = fechaAhora;
                            checkList.strMode = "C";
                            checkList.Activo = true;
                        }

                        respuestaCheckList = await _CheckListRepository.Registrar(checkList);
                        if (respuestaCheckList.Success)
                        {
                            if (checkList.ListaPreguntaInspeccion != null && checkList.ListaPreguntaInspeccion.Count > 0)
                            {
                                if (checkList.CheckList_id > 0)
                                    await _CumplimientoRepository.Eliminar(checkList.CheckList_id);

                                foreach (PreguntaInspeccion preguntaInspeccion in checkList.ListaPreguntaInspeccion)
                                {
                                    Cumplimiento cumplimiento = preguntaInspeccion.Cumplimiento;
                                    if (checkList.strMode == "U")
                                        cumplimiento.CheckList_id = checkList.CheckList_id;
                                    else
                                        cumplimiento.CheckList_id = respuestaCheckList.Data.CheckList_id;

                                    cumplimiento.IdUsuarioCreacion = checkList.IdUsuario;
                                    cumplimiento.FechaCreacion = fechaAhora;
                                    cumplimiento.Activo = true;
                                    respuestaCumplimiento = await _CumplimientoRepository.Registrar(cumplimiento);
                                    if (!respuestaCumplimiento.Success)
                                        return respuestaCheckList = new StatusReponse<CheckList>(respuestaCumplimiento.Detail, errores);
                                }
                            }
                        }
                        else
                        {
                            return respuestaCheckList;
                        }
                        tran.Complete();
                    }
                    else
                    {
                        respuestaCheckList = new StatusReponse<CheckList>("Errores de validación en registro de checkList", errores);
                    }
                }
                catch (Exception ex)
                {
                    respuestaCheckList = new StatusReponse<CheckList>("Error inesperado en registro de inspección", errores);
                    this._logger.LogError(1, ex, "Error en registro de inspección", checkList);
                }
            }
            return respuestaCheckList;
        }

        public async Task<StatusReponse<RegistroFotografico>> Obtener(string nombre)
        {
            return await this.ComplexResponse(() => _RegistroFotograficoRepository.Obtener(nombre));
        }

        public async Task<StatusReponse<InspeccionAndamio>> ActualizarHistorico(List<int> listaId, int idUsuario)
        {
            StatusReponse<InspeccionAndamio> respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>() { Success = false, Title = "" };
            Dictionary<string, List<string>> errores = new Dictionary<string, List<string>>();
            InspeccionAndamio inspeccion = null;
            DateTime fechaAhora = DateTime.Now;
            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (int id in listaId)
                    {
                        inspeccion = new InspeccionAndamio();
                        inspeccion.InspeccionAndamio_id = id;
                        inspeccion.EstadoInspeccionAndamio_id = (int)EEstadoInspeccionAndamio.Historico;
                        inspeccion.FechaEdicion = fechaAhora;
                        inspeccion.IdUsuarioEdicion = idUsuario;
                        respuestaInspeccionAndamio = await _InspeccionAndamioRepository.ActualizarHistorico(inspeccion);
                        if (!respuestaInspeccionAndamio.Success)
                            return respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>(respuestaInspeccionAndamio.Detail, errores);
                    }
                }
                catch (Exception ex)
                {
                    respuestaInspeccionAndamio = new StatusReponse<InspeccionAndamio>("Error inesperado en ActualizarHistorico", errores);
                    this._logger.LogError(1, ex, "Error en ActualizarHistorico", listaId);
                }
                tran.Complete();
            }
            return respuestaInspeccionAndamio;
        }
        public async Task<StatusReponse<OutConfiguracionApp>> ObtenerConfiguracionApp(int usuario_id)
        {
            return await this.ComplexResponse(() => _ConfiguracionAppRepository.Obtener(usuario_id));
        }
        public async Task<StatusResponse> CreateUpdateConfiguracionApp(ConfiguracionApp configuracion, int usuario_id)
        {
            configuracion.Fecha = DateTime.Now;
            configuracion.IdUsuario = usuario_id;
            configuracion.Activo = true;
            return await this.ComplexResponse(() => _ConfiguracionAppRepository.CreateUpdate(configuracion));
        }
    }
}
