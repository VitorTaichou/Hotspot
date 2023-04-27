using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "IsUsernameInUse", controller: "Account")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Senha e Confirmação de senha não são iguais.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}
