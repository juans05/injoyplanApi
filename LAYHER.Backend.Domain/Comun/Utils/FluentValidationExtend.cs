using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Domain.Comun.Utils
{
    public static class FluentValidationExtend
    {
        public static Dictionary<string, List<string>> GetErrors(IList<ValidationFailure> listOfErrors)
        {
            Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

            if (listOfErrors != null)
            {
                foreach (ValidationFailure item in listOfErrors)
                {
                    List<string> errorsFromProperties = null;
                    if (!_errors.TryGetValue(item.PropertyName, out errorsFromProperties))
                    {
                        errorsFromProperties = new List<string>();
                        _errors.Add(item.PropertyName, errorsFromProperties);
                    }

                    errorsFromProperties.Add(item.ErrorMessage);
                }
            }
            return _errors;
        }
    }
}
