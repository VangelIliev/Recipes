using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Recipies.Models.AdminModels
{
    public class RemoveCategoryModel
    {
        public string Category { get; set; }

        public Dictionary<string,string> Categories { get; set; }

        public string Id { get; set; }
    }
}
