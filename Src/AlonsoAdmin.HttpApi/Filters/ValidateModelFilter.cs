using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Filters
{
    public class ValidateModelFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
 
                string message = string.Empty;
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        message += error.ErrorMessage + "|";
                    }
                }

                context.Result = new JsonResult(ResponseEntity.Error(message));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //if (context.Exception != null) { 
                
            //}
        }
    }
}
