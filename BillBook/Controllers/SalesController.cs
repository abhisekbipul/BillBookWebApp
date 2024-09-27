using BillBook.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BillBook.Controllers
{
    public class SalesController : Controller
    {
        HttpClient client;

        public SalesController()
        {
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(clienthandler);
        }
        public IActionResult Index()
        {
            string url = "https://localhost:7095/api/Sales/GetSales";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var salesList = JsonConvert.DeserializeObject<List<Sales>>(jsonData);
                return View(salesList);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching sales data. Please try again.");
                return View(new List<Sales>());
            }
        }

        public IActionResult Details(int id)
        {
            string url = $"https://localhost:7095/api/Sales/GetSalesById/{id}";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var sale = JsonConvert.DeserializeObject<Sales>(jsonData);
                return View(sale);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching sale details. Please try again.");
                return RedirectToAction("Index");
            }
        }

        public IActionResult DownloadInvoice(int id)
        {
            string url = $"https://localhost:7095/api/Sales/DownloadInvoice/{id}";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var pdfContent = response.Content.ReadAsByteArrayAsync().Result;
                return File(pdfContent, "application/pdf", $"Invoice_{id}.pdf");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error downloading invoice. Please try again.");
                return RedirectToAction("Index");
            }
        }
    }
}
