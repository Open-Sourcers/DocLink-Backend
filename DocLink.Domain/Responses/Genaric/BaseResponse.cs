using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Responses.Genaric
{
    public class BaseResponse<T>
    {
        public  int StatusCode { get; set; }
        public T Data { get; set; }
        public string ResponseMessage { get; set; }
        public int TotalCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public BaseResponse(){}

        //return normal ok request
        public BaseResponse(T data, string responseMessage = null)
        {
            this.Data = data;
            this.StatusCode = StatusCodes.Status200OK;
            this.ResponseMessage = responseMessage;
        }

        // for bagination responses
        public BaseResponse(T data, int totalCount, string responseMessage = null)
        {
            this.Data = data;
            this.TotalCount = totalCount;
            this.StatusCode = StatusCodes.Status200OK;
            this.ResponseMessage = responseMessage;
        }

        //return errors 
        public BaseResponse(string error, int statusCode, List<string> errors = null)
        {
            this.StatusCode = statusCode;
            this.ResponseMessage = error;
            this.Errors = errors;
        }

        //return errors with data
        public BaseResponse(T data, string error, List<string> errors, int statusCode)
        {
            this.StatusCode = statusCode;
            this.ResponseMessage = error;
            this.Errors = errors;
            this.Data = data;
        }
        
    }
}
