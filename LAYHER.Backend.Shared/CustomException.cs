using System;
using System.Collections.Generic;
using System.Text;

namespace LAYHER.Backend.Shared
{
    public class CustomException : Exception
    {
        public CustomException(string title) : base(title)
        {
            this.Title = title;
        }

        public CustomException(string title, Exception ex) : base(ex.Message, ex)
        {
            this.Title = title;
        }

        public string Title { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
