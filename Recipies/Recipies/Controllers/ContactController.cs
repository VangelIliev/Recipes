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

namespace Recipies.Controllers
{
    public class ContactController : Controller
    {
        private IEmailSender _emailSender;

        public ContactController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
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
           return View();
        }
       
    }
    
}
