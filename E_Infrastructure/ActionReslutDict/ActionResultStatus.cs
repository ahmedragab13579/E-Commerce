using Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace E_Infrastructure.ActionReslutDict
{
    public class ActionResultStatus
    {
        public static IActionResult MapResult<T>(Result<T> result)
        {
            var responseMap = new Dictionary<string, Func<IActionResult>>
            {
                { "OK", () => new OkObjectResult(result) },
                { "OK_WITH_TOKEN", () => new OkObjectResult(result)}, 


                { "INVALID_DATA", () => new BadRequestObjectResult(result) },       
                { "INVALID_QTY", () => new BadRequestObjectResult(result) },        
                { "LESS_AMOUNT", () => new BadRequestObjectResult(result) },        
                { "HAS_DATA", () => new BadRequestObjectResult(result) },           
                { "EMPTY_CART", () => new BadRequestObjectResult(result) },         
                { "DUPLICATE_USER_NAME", () => new BadRequestObjectResult(result) },
                { "DUPLICATE_EMAIL", () => new BadRequestObjectResult(result) },    
                { "DUPLICATE_NAME", () => new BadRequestObjectResult(result) },     
                { "INVALID_OLD_PASSWORD", () => new BadRequestObjectResult(result)},
                { "PAYMENT_RULE_VIOLATION", () => new BadRequestObjectResult(result)},
                { "NULL_REFERENCE", () => new BadRequestObjectResult(result) },     
                { "ALREADY_BLOCKED", () => new BadRequestObjectResult(result) },    
                { "ALREADY_ACTIVE", () => new BadRequestObjectResult(result) },     
                { "ALREADY_DELETED", () => new BadRequestObjectResult(result) },    
                { "NOT_DELETED", () => new BadRequestObjectResult(result) },        
                { "CATEGORY_IN_USE", () => new BadRequestObjectResult(result) },    
                { "NOT_ADD", () => new BadRequestObjectResult(result) },            

                { "UNAUTHORIZED", () => new UnauthorizedObjectResult(result) },     
                { "INVALID_CREDENTIALS", () => new UnauthorizedObjectResult(result)},

                { "ACCOUNT_BLOCKED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden } }, 
                { "ACCOUNT_DELETED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden } }, 

                { "NOT_FOUND", () => new NotFoundObjectResult(result) },           
                { "NO_PRODUCT", () => new NotFoundObjectResult(result) },          
                { "NOT_EXIST", () => new NotFoundObjectResult(result) },           
                { "NO_ITEMS", () => new NotFoundObjectResult(result) },            
                { "NO_RETRIEVE", () => new NotFoundObjectResult(result) },            
                { "PRODUCT_NOT_FOUND", () => new NotFoundObjectResult(result) },   
                { "ROLE_NOT_FOUND", () => new NotFoundObjectResult(result) },      
                { "NO_USERS_FOUND", () => new NotFoundObjectResult(result) },      
                { "NO_ACTIVE_USERS_FOUND", () => new NotFoundObjectResult(result)},

                { "ALREADY_EXIST", () => new ConflictObjectResult(result) },       
                { "CONCURRENCY_ERROR", () => new ConflictObjectResult(result) },   
                 
                { "SAVE_FAILED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },        
                { "CHECKOUT_FAILED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },    
                { "STOCK_UPDATE_FAILED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },
                { "PAYMENT_FAILED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },     
                { "SERVER_ERROR", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },       
                { "TRANSACTION_FAILED", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } },   
                { "DOMAIN_ERROR", () => new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError } }  ,      
                { "CONNECTION_ERROR", () => new ObjectResult(result) { StatusCode = StatusCodes.Status100Continue } }        
            };

            if (responseMap.TryGetValue(result.Code, out var action))
            {
                return action();
            }

            return new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError }; 
        }
    }
}