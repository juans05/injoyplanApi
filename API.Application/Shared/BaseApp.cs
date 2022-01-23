
using System;
using System.Threading.Tasks;

using API.Shared;
using Microsoft.Extensions.Logging;

namespace API.Application.Shared
{
    public class BaseApp<T>
    {

        public readonly ILogger<BaseApp<T>> _logger;
        public BaseApp(ILogger<BaseApp<T>> logger)
        {
            _logger = logger;
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
               
                response.Success = false;
                response.Title = cuEx.Title;
                response.Errors = cuEx.Errors;
            }
            catch (Exception ex)
            {
               
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
               
                response.Title = cuix.Title;
                response.Success = false;
            }
            catch (Exception ex)
            {
                
                response.Title = "Sucedió un error inesperado.";
                response.Success = false;
            }

            return response;
        }

    }
}
