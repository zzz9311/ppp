﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Password { get; set; }
    }
}
