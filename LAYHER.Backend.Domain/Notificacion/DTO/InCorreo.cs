using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Notificacion.DTO
{
    public class InCorreo
    {
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public List<string> Destinatarios { get; set; }
    }

    public class InCorreoValidator : AbstractValidator<InCorreo>
    {
        public InCorreoValidator()
        {
            RuleFor(x => x.Asunto).NotNull().NotEmpty();
            RuleFor(x => x.Cuerpo).NotNull().NotEmpty();
            RuleFor(x => x.Destinatarios).NotNull().ForEach(item => item.EmailAddress());
        }
    }
}
