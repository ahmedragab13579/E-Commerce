using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Results
{
    public class Result<T>
    {
        public bool Success {  get; set; }
        public T Data {  get; set; }
        public string Message {  get; set; }

         public string Code {  get; set; }
        public static  Result<T> Ok(T data, string message = null) => new Result<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Code= "OK"
        };

        public static Result<T> Fail(string code, string message) => new Result<T>
        {
            Success = false,
            Code = code,
            Message = message
        };
    }
}
