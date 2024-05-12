using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.Helpers.APIResponse
{
    public class SingleResponse<T> : IResponse
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }

        public T Data { get; set; }


        public static SingleResponse<T> Success(T data, int statusCode)
        {
            return new SingleResponse<T> { StatusCode = statusCode, Status = true, Data = data };
        }

    }
}
