using AutoMapper.Configuration.Annotations;
using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class SalesMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CustomerId { get; set; }        
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime EntryDate { get; set; }        
        public decimal DiscountAmount { get; set; }
        public decimal Amount { get; set; }

        [Ignore]
        public virtual List<SalesDetails> SalesDetails { get; set; }

        [Ignore]
        public virtual List<AmountReceived> AmountReceived { get; set; }
    }

    public class SalesDetails : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("SalesMasterId")]
        public long SalesMasterId { get; set; }
        public long ItemId { get; set; }
        public decimal CarratQty { get; set; }
        public decimal Rate { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class AmountReceived : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("SalesMasterId")]
        public long SalesMasterId { get; set; }
        public string PaymentMode { get; set; }
        public bool FromDebitCard { get; set; }
        public bool FromCreditCard { get; set; }
        public string CardNo { get; set; }
        public string NameOnCard { get; set; }
        public decimal Amount { get; set; }        
    }
}
