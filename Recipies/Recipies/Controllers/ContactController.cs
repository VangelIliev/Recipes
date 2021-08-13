using Microsoft.AspNetCore.Mvc;
using Recipies.Models.EmailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    public class ContactController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(EmailInputFormModel model)
        {
            var name = model.Name;
            return View();
        }
    }
}
