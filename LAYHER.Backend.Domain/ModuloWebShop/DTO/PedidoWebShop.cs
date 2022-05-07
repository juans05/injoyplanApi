using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.DTO;
using LAYHER.Backend.Domain.ModuloProductos.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloWebShop.DTO
{
    public class PedidoWebShop
    {

        /// <summary>
        /// FE: Factura  BE: Boleta
        /// </summary>
        public string TipoDocumento { get; set; }///

        public string NumeroDocumento { get; set; }
        /// <summary>
        /// LO: Soles  EX: Dolares
        /// </summary>
        public string MonedaDocumento { get; set; }
        public double montoAfecto { get; set; }
        public double montoNoAfecto { get; set; }
        public double montoImpuesto { get; set; }
        public double montoDescuento { get; set; }
        public double montoTotal { get; set; }

        public string FormadePago { get; set; }

        public string nroPedido { get; set; }

        //public string Correo { get; set; }

        //public string tipoDocumentoPersonaEmpresa { get; set; }
        //public string nroDocumentoPersonaEmpresa { get; set; }
        //public string nroGuiaShop { get; set; }

        public MaestroPersonas persona { get; set; }

        public List<Producto> Productos
        {
            get; set;
        }
    }
}
