using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class CreateUserResponse
    {
        public CreateUserResponse()
        {
            this.Errors = new List<string>();
            this.Success = false;
        }

        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
