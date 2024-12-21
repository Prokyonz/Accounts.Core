namespace Accounts.Core.Models.Response
{
    public class StockReport
    {
        public long RowNum { get; set; }
        public long ItemId { get; set; }
        public string? Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal GSTPer { get; set; }
    }
}
