using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imanage.Shared.ViewModels
{
    public class ServiceResult<T> 
    {
        public ServiceResult()
        {

        }

        public ServiceResult(List<ValidationResult> validationResults, bool isError)
        {
            Errors = validationResults;
            IsSuccess = isError;
        }

        public List<ValidationResult> Errors { get; set; } = new List<ValidationResult>();

        public void AddError(ValidationResult error)
        {
            Errors.Add(error);
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
