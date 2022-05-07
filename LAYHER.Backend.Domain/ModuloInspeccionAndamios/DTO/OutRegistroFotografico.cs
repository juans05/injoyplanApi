using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO
{
    public class OutRegistroFotografico
    {
        public string PathImagen1 { get; set; }
        public string MimeTypeImage1 { get; set; }
        public string Descripcion1 { get; set; }

        public string PathImagen2 { get; set; }
        public string MimeTypeImage2 { get; set; }
        public string Descripcion2 { get; set; }
    }
}
