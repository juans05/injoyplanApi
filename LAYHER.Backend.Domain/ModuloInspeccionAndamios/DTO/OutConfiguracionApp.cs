using LAYHER.Backend.Domain.ModuloInspeccionAndamios.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.ModuloInspeccionAndamios.DTO
{
    public class OutConfiguracionApp
    {
        public int Usuario_id { get; set; }
        public bool ModoHistorico { get; set; }
        public int CantidadAnios  { get; set; }
    }
}
