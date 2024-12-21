namespace Accounts.Core.Models.Response
{
    public class SaleReport
    {
        public long SaleSlipNo { get; set; }
        public string? PartyName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long TotalItems { get; set; }
        public decimal BillAmount { get; set; }
    }
}
