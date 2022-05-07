using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloWebShop.DTO
{
    public class ControlCantidadesShop
    {
        public string jsonResult { get; set; }
        public string nroPedido { get; set; }
        public string vc_idCompany { get; set; }
        public string vc_idProducto { get; set; }
        public string vc_Producto { get; set; }
        public int dbl_cantidad { get; set; }

        public double dbl_Peso { get; set; }
        public DateTime dt_fechaAprobacion { get; set; }
        public int dbl_CantidadRecibida { get; set; }
        public string estado_pre { get; set; }
        public string nroGuia { get; set; }
        public string NroSerie { get; set; }
        public string vc_idListaPrecio { get; set; }
        public string AlmacenCodigo { get; set; }
        public string PreGuiaNumero { get; set; }
        public string LiberaStockPreGuia { get; set; }

        public string rucCliente { get; set; }

        public double dlb_precio { get; set; }
    }
}
