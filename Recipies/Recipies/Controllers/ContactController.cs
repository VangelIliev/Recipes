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
        public async Task<ActionResult> Index(EmailInputFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                await _emailSender.SendEmailAsync(model.Email, model.Subject, model.Message);

                ViewBag.message = "The mail has been sent we will call you back as soon as possible!";
            }
            catch (Exception e)
            {
                return RedirectToAction("CustomError", "Errors");
            }                        
           return View();
        }
       
    }
    
}
