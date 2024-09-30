using BillBook.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Collections.Generic;

namespace BillBook.Controllers
{
    public class PaymentController : Controller
    {
        private const string KeyId = "rzp_test_Kl7588Yie2yJTV";
        private const string KeySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";
        private readonly ApplicationDbContext db;
        public PaymentController(ApplicationDbContext db)
        {
            this.db = db;
        }
        

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InitiatePayment(decimal amount, string invoiceId)
        {
            //int invoiceidd=int.Parse(invoiceId);
            //var sale = db.sales.FirstOrDefault(s => s.InvoiceId== invoiceidd);
            //if (sale != null)
            //{
            //    sale.Status = "Paid"; // Update status
            //    db.SaveChanges(); // Save changes to the database
            //}
            var client = new RazorpayClient(KeyId, KeySecret);

            // Create an order
            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 },
                { "currency", "INR" },
                { "receipt", invoiceId },
                { "payment_capture", "1" }
            };

           
            Order order = client.Order.Create(options);

            ViewBag.OrderId = order["id"];
            ViewBag.Amount = amount;
            ViewBag.InvoiceId = invoiceId;
            ViewBag.KeyId = KeyId;
            return View("InitiatePayment");
            

        }


        

    }
}
