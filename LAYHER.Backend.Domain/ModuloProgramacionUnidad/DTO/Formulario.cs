using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class Formulario
    {
        public int IdFormulario { get; set; }
        public List<IFormFile> Archivo { get; set; }
        public bool EsTransporteNacional { get; set; }
        public bool EsTransportistaEncargado { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string ModeloVehiculo { get; set; }
        public string PlacaTracto { get; set; }
        public string PlacaCarreta { get; set; }
        public string TelefonoContacto { get; set; }
        public string IdTipoDocumentoTransportista { get; set; }
        public string NombreTipoDocumentoTransportista { get; set; }
        public string DocumentoTransportista { get; set; }
        public string NombreTransportista { get; set; }
        public string SctrTransportista { get; set; }
        public string TelefonoTransportista { get; set; }
        public string LicenciaTransportista { get; set; }
        public string IdTipoDocumentoEncargado { get; set; }
        public string NombreTipoDocumentoEncargado { get; set; }
        public string DocumentoEncargado { get; set; }
        public string NombreEncargado { get; set; }
        public string SctrEncargado { get; set; }
        public string Consideracion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
        public List<AdjuntoFormulario> ListaAdjunto { get; set; }
        public List<FormularioConsideracion> ListaConsideracion { get; set; }
        public List<ArchivoAdjunto> ListaArchivo { get; set; }
        public List<int> IdAdjuntoSeleccion { get; set; }
        public List<int> IdConsideracionSeleccion { get; set; }
        //Adicional
        public int ProgramacionUnidad { get; set; }
        //Consideracion
        public int IdConsideracion { get; set; }
        public string NombreConsideracion { get; set; }
        //Adjunto
        public int IdAdjunto { get; set; }
        public string NombreAdjunto { get; set; }
        public string NombreDocumento { get; set; }
        public string RutaAdjunto { get; set; }

    }
}
