﻿using AspNetCore.ViewModels.Validations;
using FluentValidation.Attributes;

namespace AspNetCore.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}