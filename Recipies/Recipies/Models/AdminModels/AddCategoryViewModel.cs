using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Models.AdminModels
{
    public class AddCategoryViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
