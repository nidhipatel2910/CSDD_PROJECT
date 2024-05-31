using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Test2.Data;
using Test2.Models;
using OfficeOpenXml;

namespace Test2.Controllers
{
    public class EmailController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Test2Context _context;

        public EmailController(IConfiguration configuration, Test2Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail()
        {
            try
            {
                // Get SMTP configuration and predefined email details from appsettings.json
                string smtpServer = _configuration["SmtpSettings:Server"];
                int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
                string smtpUsername = _configuration["SmtpSettings:Username"];
                string smtpPassword = _configuration["SmtpSettings:Password"];
                string subject = _configuration["SmtpSettings:Subject"];
                string bodyTemplate = _configuration["SmtpSettings:Body"];

                // Get all employees
                var employees = _context.Employee.ToList();

                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    foreach (var employee in employees)
                    {
                        string recipientEmail = employee.Email;
                        string acceptLink = Url.Action("AcceptInvitation", "Email", new { id = employee.Id }, Request.Scheme);
                        string body = bodyTemplate.Replace("{AcceptLink}", acceptLink);

                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(smtpUsername);
                            mailMessage.To.Add(recipientEmail);
                            mailMessage.Subject = subject;
                            mailMessage.Body = body;
                            mailMessage.IsBodyHtml = true; // Set to true if using HTML in the body

                            // Send the email
                            smtpClient.Send(mailMessage);
                        }
                    }
                }

                ViewBag.Message = "Emails sent successfully!";
            }
            catch (SmtpException smtpEx)
            {
                // Log detailed SMTP exception
                ViewBag.Message = "Error sending email: SMTP Exception - " + smtpEx.Message;
                if (smtpEx.InnerException != null)
                {
                    ViewBag.Message += " Inner Exception - " + smtpEx.InnerException.Message;
                }
            }
            catch (Exception ex)
            {
                // Log general exception
                ViewBag.Message = "Error sending email: General Exception - " + ex.Message;
                if (ex.InnerException != null)
                {
                    ViewBag.Message += " Inner Exception - " + ex.InnerException.Message;
                }
            }

            return View("Index");
        }


        [HttpPost]
        public IActionResult SendEmail1111()
        {
            try
            {
                Console.WriteLine($"SMTP Details");
                // Get SMTP configuration and predefined email details from appsettings.json
                string smtpServer = _configuration["SmtpSettings:Server"];
                int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
                string smtpUsername = _configuration["SmtpSettings:Username"];
                string smtpPassword = _configuration["SmtpSettings:Password"];
                Console.WriteLine($"SMTP Username: {smtpUsername}");
                Console.WriteLine($"SMTP Password: {smtpPassword}");
                Console.WriteLine($"SMTP smtpServer: {smtpServer}");
                Console.WriteLine($"SMTP smtpPort: {smtpPort}");
                string recipientEmail = _configuration["SmtpSettings:RecipientEmail"];
                string subject = _configuration["SmtpSettings:Subject"];
                string body = _configuration["SmtpSettings:Body"];
                Console.WriteLine($"SMTP recipientEmail: {recipientEmail}");
                Console.WriteLine($"SMTP subject: {subject}");
                // Configure the SMTP client
                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true; // Set to true if using HTML in the body

                        Console.WriteLine($"SMTP sending mail body: {body}");
                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                }

                ViewBag.Message = "Email sent successfully!";
            }
            catch (SmtpException smtpEx)
            {
                // Log detailed SMTP exception
                ViewBag.Message = "Error sending email: SMTP Exception - " + smtpEx.Message;
                if (smtpEx.InnerException != null)
                {
                    ViewBag.Message += " Inner Exception - " + smtpEx.InnerException.Message;
                }
            }
            catch (Exception ex)
            {
                // Log general exception
                ViewBag.Message = "Error sending email: General Exception - " + ex.Message;
                if (ex.InnerException != null)
                {
                    ViewBag.Message += " Inner Exception - " + ex.InnerException.Message;
                }
            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult GenerateReport()
        {
            // Get employees with InvitationAccepted == true
            var employees = _context.Employee.Where(e => e.InvitationAccepted).ToList();

            // Generate Excel report
            var stream = new MemoryStream();
            // Set the license context before using EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");
                worksheet.Cells.LoadFromCollection(employees, true);
                package.Save();
            }

            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
        }

        public IActionResult AcceptInvitation(int id)
        {
            var employee = _context.Employee.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                employee.InvitationAccepted = true;
                _context.SaveChanges();
                ViewBag.Message = "Thank you for accepting the invitation!";
            }
            else
            {
                ViewBag.Message = "Invalid invitation.";
            }
            return View();
        }

    }
}
