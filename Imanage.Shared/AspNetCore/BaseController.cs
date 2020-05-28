
using Dafmis.Shared.Identity;
using Imanage.Shared.DI;
using Imanage.Shared.Enums;
using Imanage.Shared.Helpers;
using Imanage.Shared.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Imanage.Shared.AspNetCore
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController()
        {
            _logger = ServiceLocator.Current.GetInstance<ILogger<BaseController>>();
        }

        protected UserPrincipal CurrentUser
        {
            get
            {
                return new UserPrincipal(User as ClaimsPrincipal);
            }
        }

        public IActionResult ApiResponse<T>(T data = default(T), string message = null,
            ApiResponseCodes codes = ApiResponseCodes.OK, int? totalCount = 0, params string[] errors) where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                TotalCount = totalCount ?? 0,
                Errors = errors.ToList(),
                Payload = data,
                Code = !errors.Any() ? codes : codes == ApiResponseCodes.OK ? ApiResponseCodes.ERROR : codes
            };

            response.Description = message ?? response.Code.GetDescription();
            return Ok(response);
        }
      
        //protected string GetModelStateValidationErrors()
        //{
        //    string message = string.Join("; ", ModelState.Values
        //                            .SelectMany(a => a.Errors)
        //                            .Select(e => e.ErrorMessage));
        //    return message;
        //}

        protected string GetModelStateValidationError()
        {
            string message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
            return message;
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            _logger.LogError(ex, ex.Message);


            ApiResponse<string> rsp = new ApiResponse<string>();
            rsp.Code = ApiResponseCodes.ERROR;
#if DEBUG
            rsp.Errors = new List<string>() { $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}" };
            return Ok(rsp);
#else
             rsp.Errors = new List<string>() {  customErrorMessage ?? "An error occurred while processing your request!"};
             return Ok(rsp);
#endif
        }

        protected async Task<ApiResponse<T>> HandleApiOperationAsync
            <T>
            (
           Func<Task<ApiResponse<T>>> action,
           [CallerLineNumber] int lineNo = 0,
           [CallerMemberName] string method = "")
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = ApiResponseCodes.OK
            };

            try
            {

                var methodResponse = await action.Invoke();

                apiResponse.Payload = methodResponse.Payload;
                apiResponse.TotalCount = methodResponse.TotalCount;
                apiResponse.Code = methodResponse.Code;
                apiResponse.Errors = methodResponse.Errors;
                apiResponse.Description = string.IsNullOrEmpty(apiResponse.Description) ? methodResponse.Description : apiResponse.Description;

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                apiResponse.Code = ApiResponseCodes.EXCEPTION;

#if DEBUG
                apiResponse.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";
#else
                apiResponse.Description = "An error occurred while processing your request!";
#endif
                apiResponse.Errors.Add(apiResponse.Description);
                return apiResponse;
            }
        }

    }
}