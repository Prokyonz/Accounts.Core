using BaseClassLibrary;
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
        public DateTime EntryDate { get; set; }        
        public decimal DiscountAmount { get; set; }
    }

    public class SalesDetails : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long StockId { get; set; }
        public decimal CarratQty { get; set; }
        public decimal Rate { get; set; }
        public decimal GstAmount { get; set; }
        public AmountReceived AmountReceived { get; set; }
    }

    public class AmountReceived : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public bool FromDebitCard { get; set; }
        public bool FromCreditCard { get; set; }
        public string CardNo { get; set; }
        public string NameOnCard { get; set; }
        public decimal CardAmount { get; set; }
        public decimal CashAmount { get; set; }

    }
}
