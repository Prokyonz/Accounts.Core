namespace Accounts.Core.Models.Response
{
    public class SaleReport
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? PartyName { get; set; }
        public decimal BillAmount { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentMode { get; set; }
        public string CardNo { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
