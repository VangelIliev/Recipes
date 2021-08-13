using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Models.EmailModels
{
    public class EmailInputFormModel
    {
        [MaxLength(20)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Message { get; set; }

        public string Answer  { get; set; }
    }
}
