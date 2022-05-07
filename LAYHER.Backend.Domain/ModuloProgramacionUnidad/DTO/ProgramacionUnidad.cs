using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProgramacionUnidad.DTO
{
    public class ProgramacionUnidad
    {
        public int IdProgramacionUnidad { get; set; }
        public string NumeroProgramacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Correo { get; set; }
        public decimal Tonelada { get; set; }
        public string Especificacion { get; set; }
        public DateTime FechaRevision { get; set; }
        public DateTime RevisionHoraInicio { get; set; }
        public DateTime RevisionHoraFin { get; set; }
        public DateTime AlquilerFin { get; set; }
        public int IdMetrajeTrailer { get; set; }
        public string IdProyecto { get; set; }
        public int IdTipoProgramacion { get; set; }
        public string IdCotizacion { get; set; }
        public int IdTipoUnidad { get; set; }
        public string IdAlmacen { get; set; }
        public string IdAlmacenDestino { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public DateTime FechaEdicion { get; set; }
        public string Observacion { get; set; }
        public bool UnidadRecibida { get; set; }
        public bool Conforme { get; set; }
        public bool Activo { get; set; }
        //adicionales
        public int IdPersona { get; set; }
        public int IdCliente { get; set; }
        public string CorreoCliente { get; set; }
        public int OffSet { get; set; }
        public int Fetch { get; set; }
        public string NombreEstado { get; set; }
        public int Longitud { get; set; }
        public string NombreUnidad { get; set; }
        public string DescripcionLocal { get; set; }
        public string DescripcionLocalDestino { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreTipoProgramacion { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public string NombreCliente { get; set; }
        public string NombreProyecto { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime TurnoLlegada { get; set; }
        public string TelefonoEncargado { get; set; }
        public string NumeroContenedores { get; set; }
        public string TipoImportacion { get; set; }
        public string Contenedor { get; set; }
        public string PackingList { get; set; }
        public string NombreEspecificacion { get; set; }
        
        //Datos del formulario
        public int IdFormulario { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string ModeloVehiculo { get; set; }
        public string PlacaTracto { get; set; }
        public string PlacaCarreta { get; set; }
        public int IdProgramacionTiempo { get; set; }
        public bool EsTransporteNacional { get; set; }
        public bool EsTransportistaEncargado { get; set; }
        public string NombreTransportista { get; set; }
        public string DocumentoTransportista { get; set; }
        public string LicenciaTransportista { get; set; }
        public string NombreEncargado { get; set; }
        public string DocumentoEncargado { get; set; }
        public int IdComercial { get; set; }
        public string NombreComercial { get; set; }
        public string IdContratro { get; set; }
        public string NombreContrato { get; set; }
    }

    public class Calendario
    {
        public int CantidadObservacion { get; set; }
        public int IdTipoProgramacion { get; set; }
        public string NombreTipoProgramacion { get; set; }
        public decimal Tonelada { get; set; }
        public string FechaInicio { get; set; }
        public string Color { get; set; }
        public string idTipoEvento { get; set; }
    }
}
