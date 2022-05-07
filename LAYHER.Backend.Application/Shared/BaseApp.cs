using LAYHER.Backend.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LAYHER.Backend.Application.Shared
{
    public class BaseApp<T>
    {
        public readonly ILogger<BaseApp<T>> _logger;
        public BaseApp()
        {
           
        }

        protected async Task<StatusResponse> MessageResponse(Func<Task> callback, string msgTitle)
        {
            var response = new StatusResponse();

            try
            {
                await callback();

                response.Success = true;
                response.Title = msgTitle;
            }
            catch (CustomException cuEx)
            {
                this._logger.LogError(1, cuEx, "Error personalizado");
                response.Success = false;
                response.Title = cuEx.Title;
                response.Errors = cuEx.Errors;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, "Error genérico");
                response.Success = false;
                response.Title = "Sucedió un error inesperado.";
            }

            return response;
        }


        protected async Task<StatusReponse<X>> ComplexResponse<X>(Func<Task<X>> callbackData, string title = "")
        {
            var response = new StatusReponse<X>();

            try
            {
                response.Data = await callbackData();
                response.Title = title;
                response.Success = true;
            }
            catch (CustomException cuix)
            {
                this._logger.LogError(1, cuix, "Error personalizado");
                response.Title = cuix.Title;
                response.Success = false;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, "Error generico");
                response.Title = "Sucedió un error inesperado.";
                response.Success = false;
            }

            return response;
        }

    }
}
