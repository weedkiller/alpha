using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Utilities
{
    public class JsonResponseFactory : HttpResponseBase
    {
        public static object ErrorResponse(string error)
        {
            return new { Success = false, ErrorMessage = error };
        }

        public static object SuccessResponse()
        {
            return new { Success = true };
        }


        public static object SuccessResponse(string message, object referenceObject)
        {
            return new { Success = true, Message = message, Object = referenceObject };
        }


        public static object SuccessResponse(string message)
        {
            return new { Success = true, Message = message };
        }

        public static object SuccessResponse(object referenceObject)
        {
            return new { Success = true, Object = referenceObject };
        }
 
    }
}