using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Responses
{
    public class BaseResponse<TEntity>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public TEntity? Data { get; set; }
        public BaseResponse(IEnumerable<string> errors , int _statusCode = 400) : this(_statusCode)
        {
            Errors = new List<string>();
            Errors = errors;
        }

        public BaseResponse(int _statusCode, string? _massage = null)
        {
            StatusCode = _statusCode;
            Message = _massage ?? getMassage(_statusCode);
        }

        public BaseResponse(TEntity? data , int _statusCode = 200) : this(_statusCode)
        {
            Data = data;
        }

        private string? getMassage(int statusCode)
        {
            return statusCode switch
            {
                400 => "BadRequest",
                401 => "You Are Not Authrized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                200 => "OK",
                _ => null
            };
        }
    }
}
