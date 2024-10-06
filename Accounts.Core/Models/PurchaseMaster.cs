using BaseClassLibrary;
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
        public string Description { get; set; }
        public long BrokerId { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class PurchaseDetails : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long StockId { get; set; }
        public long PurchaseMasterId { get; set; }

    }
}
