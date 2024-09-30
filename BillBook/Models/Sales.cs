﻿using System.ComponentModel.DataAnnotations;

namespace BillBook.Models
{
    public class Sales
    {
        [Key]
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string HSN { get; set; }
        public string Particulars { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }

        public string? Status {  get; set; }
    }
}
