namespace Accounts.Core.Models.Response
{
    public class SaleBillPrint
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? PartyName { get; set; }
        public decimal TotalCarratQty { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal BillAmount { get; set; }
        public decimal BillAmountWithoutTax { get; set; }
        public string BillAmountInWords { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public List<SaleBillItems> SaleBillItems { get; set; }
        public List<SaleBillPayments> SaleBillPayments { get; set; }
    }

    public class SaleBillItems
    {
        public long Id { get; set; }
        public long SerialNo { get; set; }
        public string ItemName { get; set; }
        public decimal CarratQty { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
    }

    public class SaleBillPayments
    {
        public long Id { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentMode { get; set; }
        public string CardNo { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
