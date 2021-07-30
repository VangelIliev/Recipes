using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public  class EditUserRequest
    {
        public string Role { get; set; }
        public string NewRole { get; set; }
    }
}
