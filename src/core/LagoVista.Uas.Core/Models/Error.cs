using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public enum ErrorLevel
    {
        Fatal,
        Error,
        Warning
    }

    public class Error
    {
        public Error(string err, ErrorLevel level)
        {
            Message = err;
            ErrorLevel = level;
        }

        public string Message { get; }
        public  ErrorLevel ErrorLevel { get; }
    }
}
