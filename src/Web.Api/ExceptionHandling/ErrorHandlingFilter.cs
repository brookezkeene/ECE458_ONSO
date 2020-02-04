using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Api.ExceptionHandling
{
    public class ApiExceptionFilter
    {
        //public override void OnException(ExceptionContext context)
        //{
        //    if (context.Exception is ApiException)
        //    {
        //        var ex = context.Exception as ApiException;
        //    }
        //}
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        //public 
    }
}
