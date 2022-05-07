using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO
{
    public class OutCheckList: CheckList
    {
        public string TipoAndamio_Nombre { get; set; }
        public string SubTipoAndamio_Nombre { get; set; }
    }
}
