﻿using AutoMapper.Configuration.Annotations;
using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Core.Models.Response
{
    public class PurchaseReports
    {
        public long PurchaseSlipNo { get; set; }
        public string DealerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long TotalItems { get; set; }
        public decimal BillAmount { get; set; }
    }
}