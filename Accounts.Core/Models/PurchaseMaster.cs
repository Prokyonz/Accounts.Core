using AutoMapper.Configuration.Annotations;
using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class PurchaseMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long PurchaseSlipNo { get; set; }
        public long CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
        public long BrokerId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal BillAmount { get; set; }

        [Ignore]
        public virtual List<PurchaseDetails> PurchaseDetails { get; set; }
    }

    public class PurchaseDetails : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long StockId { get; set; }

        [ForeignKey("PurchaseMasterId")]
        public long PurchaseMasterId { get; set; }
        public string? ItemDescription { get; set; }
        public long Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal Total { get; set; }
    }
}