using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Password { get; set; }
    }
}
