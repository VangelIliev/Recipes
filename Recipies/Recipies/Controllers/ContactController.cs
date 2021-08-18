using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Recipies.Models.EmailModels;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Recipies.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public ContactController(IEmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Index(EmailInputFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _emailSender.SendEmailAsync(model.Email, model.Subject, model.Message);
                return RedirectToAction("All", "Recipes");
            }
            catch (Exception e)
            {
                return RedirectToAction("CustomError", "Errors");
            }                        
           
        }

        public async Task<ActionResult> ContactHost()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ContactHost(EmailInputFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            try
            {
                var hostEmail = _configuration.GetValue<string>("UserName");
                await _emailSender.SendEmailAsync("recipes.vangel@gmail.com", $"This is an email from {model.Email}", model.Message);
                return RedirectToAction("All", "Recipes");
            }
            catch (Exception e)
            {
                return RedirectToAction("CustomError", "Errors");
            }
        }


    }
    
}
