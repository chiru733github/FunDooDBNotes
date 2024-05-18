using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Layer.Models
{
    public class ForgotPasswordModel
    {
        public string? Email { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
    }
}
