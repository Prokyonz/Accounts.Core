namespace Accounts.Core.Models.Response
{
    public class SaleReport
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? UserName { get; set; }
        public string? PartyName { get; set; }
        public decimal BillAmount { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentMode { get; set; }
        public string CardNo { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class SaleReportForAdmin
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? UserName { get; set; }
        public string? PartyName { get; set; }
        public string? Address { get; set; }
        public long Pincode { get; set; }
        public string? PanNo { get; set; }
        public string? ItemName { get; set; }
        public decimal CarratQty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal Discount { get; set; }
        public decimal BillAmount { get; set; }
        public string? Payment1_Mode { get; set; }
        public string? Payment1_CardNo { get; set; }
        public decimal? Payment1_Amount { get; set; }
        public string? Payment2_Mode { get; set; }
        public string? Payment2_CardNo { get; set; }
        public decimal? Payment2_Amount { get; set; }
        public string? Payment3_Mode { get; set; }
        public string? Payment3_CardNo { get; set; }
        public decimal? Payment3_Amount { get; set; }
        public string? Payment4_Mode { get; set; }
        public string? Payment4_CardNo { get; set; }
        public decimal? Payment4_Amount { get; set; }
    }
}
