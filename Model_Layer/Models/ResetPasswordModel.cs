using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Layer.Models
{
    public class ResetPasswordModel
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
