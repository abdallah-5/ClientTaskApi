using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTask.Core.Helpers.APIResponse
{
    public class PaginationResponse<T> : IResponse
    {
        public PaginationResponse(bool status, int statusCode, IReadOnlyList<T> data, int pageIndex, int pageSize, int count)
        {
            Status = status;
            StatusCode = statusCode;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
        public bool Status { get; set; }

        public int StatusCode { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public static PaginationResponse<T> Success(bool status, int statusCode, IReadOnlyList<T> data, int pageIndex, int pageSize, int count)
        {
            return new PaginationResponse<T>(status, statusCode, data, pageIndex, pageSize, count);
        }
    }
}