using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloProductos.DTO
{
    public class Eventos { 
        public int diaNum { get; set; }
        public int dia { get; set; }
        public int mes { get; set; }
    }
    public class Producto
    {
        public string compañiaSocio { get; set; }
        public string itemCodigo { get; set; }
        public string idCodigoProducto
        {
            get; set;
        }
        public string Secuencia { get; set; }
        public string Descripcion { get; set; }

        public double precioUnitario { get; set; }

        public double precioProductoTotal { get; set; }
        public string condicion { get; set; }
        public int cantidad { get; set; }
        public bool esServicio { get; set; }
        public int stockActual { get; set; }
        public string nombreImagen { get; set; }
        public string vc_rutaImagenLarga { get; set; }
        public string nroPedido { get; set; }
        public string PreGuiaNumero { get; set; }
        public string ListsPrecio { get; set; }
    }
}
