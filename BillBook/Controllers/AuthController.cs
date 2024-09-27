using BillBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace BillBook.Controllers
{
    public class AuthController : Controller
    {

        HttpClient client;
        public AuthController()
        {
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(clienthandler);
        }
        public IActionResult RequestOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RequestOtp(OtpRequestModel model)
        {
            if(ModelState.IsValid)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = "https://localhost:7095/api/Auth/requestOtp";
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ValidateOtp", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error sending OTP. Please try again.");
                }
            }
            return View(model);
        }

        public IActionResult OtpSent()
        {
            return View();
        }

        public IActionResult ValidateOtp(string email)
        {
            var model = new OtpVerificationModel { Email = email };
            return View(model);
        }
        [HttpPost]
        public IActionResult ValidateOtp(OtpVerificationModel model)
        {
            if (ModelState.IsValid)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = "https://localhost:7095/api/Auth/verifyOtp";
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Sales");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP. Please try again.");
                }
            }
            return View(model);
        }

        public IActionResult OtpSuccess()
        {
            return View();
        }
    }
}
