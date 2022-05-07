using System;
using System.Collections.Generic;

namespace LAYHER.Backend.Shared
{
    public class StatusResponse
    {
        public bool Success { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Dictionary<string, List<string>> Errors { set; get; }

        public StatusResponse() : this(true, "")
        {

        }

        public StatusResponse(bool success, string title)
        {
            this.Success = success;
            this.Title = title;
        }

        public StatusResponse(bool success, string title, string body)
        {
            this.Success = success;
            this.Title = title;
            this.Detail = body;
        }

        /// <summary>
        /// Mensaje con título de error y diccionario de errores
        /// </summary>
        /// <param name="title"></param>
        /// <param name="errors"></param>
        public StatusResponse(string title, Dictionary<string, List<string>> errors)
        {
            this.Success = false;
            this.Title = title;
            this.Detail = null;
            this.Errors = errors;
        }

        /// <summary>
        /// Mensaje con título de error, detalle de error y diccionario de errores
        /// </summary>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        /// <param name="errors"></param>
        public StatusResponse(string title, string detail, Dictionary<string, List<string>> errors)
        {
            this.Success = false;
            this.Title = title;
            this.Detail = detail;
            this.Errors = errors;
        }
    }

    public class StatusReponse<T> : StatusResponse
    {
        public StatusReponse() : this(true, "")
        {
        }

        public StatusReponse(bool success, string title) : base(success, title)
        {
        }

        public StatusReponse(bool success, string title, string body) : base(success, title, body)
        {
        }

        /// <summary>
        /// Mensaje con título de error y diccionario de errores
        /// </summary>
        /// <param name="title"></param>
        /// <param name="errors"></param>
        public StatusReponse(string title, Dictionary<string, List<string>> errors) : base(title, errors)
        {
        }

        /// <summary>
        /// Mensaje con título de error, detalle de error y diccionario de errores
        /// </summary>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        /// <param name="errors"></param>
        public StatusReponse(string title, string detail, Dictionary<string, List<string>> errors) : base(title, detail, errors)
        {
        }

        public T Data { get; set; }
    }
}
