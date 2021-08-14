using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Recipies.Models.EmailModels;
using System.Threading.Tasks;
using System;

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(model.Email));
                email.To.Add(MailboxAddress.Parse("recipes.vangel@gmail.com"));
                email.Subject = model.Subject;
                email.Body = new TextPart(TextFormat.Plain) { Text = model.Message };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587);
                smtp.Authenticate("recipes.vangel@gmail.com", "parolatamiemnogodobra");
                smtp.Send(email);
                smtp.Disconnect(true);

                ViewBag.message = "The mail has been sent we will call you back as soon as possible!";
            }
            catch (Exception e)
            {

                throw;
            }                        
           return View();
        }
       
    }
    
}
