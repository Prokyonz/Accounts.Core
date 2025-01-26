using Accounts.Core.DbContext;
using Accounts.Core.Migrations;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Accounts.Core.Repositories
{
    public interface ISalesMasterRepository
    {
        Task<List<SalesMaster>> GetAllSales(bool includeDetails);
        Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster);
        Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster);
        Task<bool> DeleteSalesAsync(long salesId, bool isHardDelete = false);
        Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails);
        Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize, bool includeDetails);
        Task<List<SaleReport>> SalesReport(long userId, DateTime? fromDate, DateTime? toDate, string? name);
        Task<SaleBillPrint> SalesBillPrint(long saleMasterID);
        Task<long> GetMaxInvoiceNo(string seriesName);
        Task<bool> UpdatePDFOnly(long salesId, string pdf = "");
    }

    public class SalesMasterRepository : ISalesMasterRepository
    {
        private readonly IBaseRepository<SalesMaster, AppDbContext> _salesRepo;
        private readonly IBaseRepository<SaleReport, AppDbContext> _salesReportRepo;
        private readonly IBaseRepository<SeriesMaster, AppDbContext> _seriesMasterRepo;
        private readonly AppDbContext _appDbContext;

        public SalesMasterRepository(IBaseRepository<SalesMaster, AppDbContext> salesRepo,
            IBaseRepository<SaleReport, AppDbContext> salesReportRepo,
            IBaseRepository<SeriesMaster, AppDbContext> seriesMasterRepo,
            AppDbContext appDbContext)
        {
            _salesRepo = salesRepo;
            _salesReportRepo = salesReportRepo;
            _seriesMasterRepo = seriesMasterRepo;
            _appDbContext = appDbContext;
        }

        public async Task<List<SaleReport>> SalesReport(long userId, DateTime? fromDate, DateTime? toDate, string? name)
        {
            //object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            string spName = "salesReport";
            if (userId > 0)
                spName += " " + userId;
            else
                spName += " " + "NULL";
            if (fromDate != null)
                spName += " ,'" + fromDate?.ToString("yyyyMMdd") + "'";
            else
                spName += " ," + "NULL";
            if (toDate != null)
                spName += " ,'" + toDate?.ToString("yyyyMMdd") + "'";
            else
                spName += " ," + "NULL";
            if (!string.IsNullOrWhiteSpace(name))
                spName += " ,'" + name + "'";
            else
                spName += " ," + "NULL";
            var result = await _salesReportRepo.ExecuteStoredProcedureAsync(spName);

            return result;
        }

        public async Task<SaleBillPrint> SalesBillPrint(long saleMasterID)
        {
            string spName = "SalesBillPrint";
            var result = await _salesReportRepo.ExecuteStoredProcedureDSAsync(spName, "@salesId", saleMasterID);
            return ConvertDataSetToModel(result);
        }

        public SaleBillPrint ConvertDataSetToModel(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count < 3)
                throw new ArgumentException("Invalid DataSet. Ensure it contains at least three tables.");

            // Extract tables
            DataTable saleBillPrintTable = dataSet.Tables[0];
            DataTable saleBillItemsTable = dataSet.Tables[1];
            DataTable saleBillPaymentsTable = dataSet.Tables[2];

            // Ensure each table has data
            if (saleBillPrintTable.Rows.Count == 0)
                throw new InvalidOperationException("SaleBillPrint table is empty.");

            // Map SaleBillPrint
            DataRow printRow = saleBillPrintTable.Rows[0];
            var saleBillPrint = new SaleBillPrint
            {
                Id = Convert.ToInt64(printRow["Id"]),
                InvoiceNo = Convert.ToString(printRow["InvoiceNo"]),
                InvoiceDate = Convert.ToDateTime(printRow["InvoiceDate"]),
                PartyName = printRow["PartyName"] as string,
                BillAmount = Convert.ToDecimal(printRow["BillAmount"]),
                BillAmountInWords = ConvertNumberToWords(Convert.ToDecimal(printRow["BillAmount"])),
                Address = Convert.ToString(printRow["Address"]),
                MobileNo = Convert.ToString(printRow["MobileNo"]),
                EmailId = Convert.ToString(printRow["EmailId"]),
                PanNo = Convert.ToString(printRow["PanNo"]),
                Pincode = Convert.ToInt64(printRow["Pincode"]),
                BrokerName = Convert.ToString(printRow["BrokerName"]),
                CreationDate = Convert.ToString(printRow["CreationDate"]),
                SaleBillItems = new List<SaleBillItems>(),
                SaleBillPayments = new List<SaleBillPayments>()
            };

            decimal totalCGST = 0;
            decimal totalSGST = 0;
            decimal totalIGST = 0;
            decimal totalCarratQty = 0;

            // Map SaleBillItems
            foreach (DataRow row in saleBillItemsTable.Rows)
            {
                saleBillPrint.SaleBillItems.Add(new SaleBillItems
                {
                    Id = Convert.ToInt64(row["Id"]),
                    SerialNo = Convert.ToInt64(row["SerialNo"]),
                    ItemName = Convert.ToString(row["ItemName"]),
                    CarratQty = Convert.ToDecimal(row["CarratQty"]),
                    Rate = Convert.ToDecimal(row["Rate"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    SGST = Convert.ToDecimal(row["SGST"]),
                    CGST = Convert.ToDecimal(row["CGST"]),
                    IGST = Convert.ToDecimal(row["IGST"]),
                });

                totalCGST += Convert.ToDecimal(row["CGST"]);
                totalSGST += Convert.ToDecimal(row["SGST"]);
                totalIGST += Convert.ToDecimal(row["IGST"]);
                totalCarratQty += Convert.ToDecimal(row["TotalAmount"]);
            }

            saleBillPrint.CGST = totalCGST;
            saleBillPrint.SGST = totalSGST;
            saleBillPrint.IGST = totalIGST;
            saleBillPrint.TotalCarratQty = totalCarratQty;
            saleBillPrint.BillAmountWithoutTax = saleBillPrint.BillAmount - saleBillPrint.IGST;

            // Map SaleBillPayments
            foreach (DataRow row in saleBillPaymentsTable.Rows)
            {
                saleBillPrint.SaleBillPayments.Add(new SaleBillPayments
                {
                    Id = Convert.ToInt64(row["Id"]),
                    PaymentNo = Convert.ToString(row["PaymentNo"]),
                    PaymentMode = Convert.ToString(row["PaymentMode"]),
                    CardNo = Convert.ToString(row["CardNo"]),
                    PaidAmount = Convert.ToDecimal(row["PaidAmount"]),
                });
            }

            return saleBillPrint;
        }

        public async Task<long> GetMaxInvoiceNo(string seriesName)
        {
            try
            {
                var result = await _salesRepo.QueryAsync(
                           query => query.Id > 0 && query.IsDelete == false && query.SeriesName == seriesName,
                           orderBy: c => c.InvoiceNo,
                           0, int.MaxValue);

                long invoiceNo = 0;
                if (result != null && result.Count > 0)
                {
                    invoiceNo = result.Max(x => x.InvoiceNo);
                }

                return invoiceNo += 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster)
        {
            try
            {
                var series = await _seriesMasterRepo.QueryAsync(
                           query => query.Id > 0 && query.IsDelete == false && query.IsActive == true,
                           orderBy: c => c.CreatedDate ?? DateTime.Now,
                           0, 10);

                salesMaster.SeriesName = series[0].Name;

                salesMaster.InvoiceNo = await GetMaxInvoiceNo(salesMaster.SeriesName);

                await _salesRepo.BeginTransactionAsync();

                var result = await _salesRepo.AddAsync(salesMaster);

                await _salesRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _salesRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSalesAsync(long salesId, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                await _salesRepo.DeleteAsync(salesId);
            }
            else
            {
                await _salesRepo.BeginTransactionAsync();
                var result = await _salesRepo.GetByIdAsync(salesId);
                result.UpdatedDate = DateTime.Now;
                result.IsDelete = true;
                await _salesRepo.CommitTransactionAsync();
            }
            return true;
        }

        public async Task<List<SalesMaster>> GetAllSales(bool includeDetails = false)
        {
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;
            Expression<Func<SalesMaster, object>> salesDetails = x => x.SalesDetails.Where(s => !s.IsDelete);
            Expression<Func<SalesMaster, object>> amountReceived = m => m.AmountReceived.Where(s => !s.IsDelete);

            if (includeDetails)
                return await _salesRepo.GetAllAsync(predicate, salesDetails, amountReceived);
            else
                return await _salesRepo.GetAllAsync(predicate);
        }

        public async Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _salesRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.EntryDate,
                pageIndex, pageSize);
        }

        public async Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id == salesId && c.IsDelete == false;
            Expression<Func<SalesMaster, object>> salesDetails = x => x.SalesDetails.Where(s => !s.IsDelete);
            Expression<Func<SalesMaster, object>> amountReceived = m => m.AmountReceived.Where(s => !s.IsDelete);

            SalesMaster salesMaster = new SalesMaster();

            if (includeDetails)
            {
                var salesWithAllDetails = await _salesRepo.GetAllAsync(predicate, salesDetails, amountReceived);

                if (salesWithAllDetails.Any())
                {
                    salesMaster = salesWithAllDetails.FirstOrDefault();
                }
            }
            else
            {
                var result = await _salesRepo.QueryAsync(
                    query => query.Id == salesId && query.IsDelete == false,
                    orderBy: c => c.EntryDate,
                    pageIndex, pageSize);

                salesMaster = result?.FirstOrDefault();
            }

            return salesMaster;
        }

        public async Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster)
        {
            try
            {
                // get the old amount received. 
                var salesMasterAmountIds = salesMaster.AmountReceived
                .Where(x => x.IsDelete == false)
                .Select(x => x.Id)
                .ToList();

                var amountReceived = await _appDbContext.AmountReceived.Where(
                    query => salesMasterAmountIds.Contains(query.Id)).ToListAsync();

                var salesMasterDetailsIds = salesMaster.SalesDetails
                .Where(x => x.IsDelete == false)
                .Select(x => x.Id)
                .ToList();

                var salesDetails = await _appDbContext.SalesDetails.Where(
                    query => salesMasterDetailsIds.Contains(query.Id)).ToListAsync();

                await _appDbContext.Database.BeginTransactionAsync();

                if (amountReceived.Any())
                    _appDbContext.AmountReceived.RemoveRange(amountReceived);

                if (salesDetails.Any())
                    _appDbContext.SalesDetails.RemoveRange(salesDetails);

                // Add the new records

                if (salesMaster.AmountReceived.Any())
                    await _appDbContext.AmountReceived.AddRangeAsync(salesMaster.AmountReceived);

                if (salesMaster.SalesDetails.Any())
                    await _appDbContext.SalesDetails.AddRangeAsync(salesMaster.SalesDetails);

                await _salesRepo.UpdateAsync(salesMaster);

                await _appDbContext.SaveChangesAsync();

                await _appDbContext.Database.CommitTransactionAsync();

                return salesMaster;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string ConvertNumberToWords(decimal number)
        {
            if (number == 0)
                return "zero";

            var words = "";

            // Split the number into integer and decimal parts
            var integerPart = (int)Math.Floor(number);
            var decimalPart = (int)((number - integerPart) * 100);

            words += ConvertToWords(integerPart) + " rupees";

            if (decimalPart > 0)
            {
                words += " and " + ConvertToWords(decimalPart) + " paise";
            }

            return ConvertToTitleCase(words.Trim());
        }

        private static string ConvertToWords(int number)
        {
            string[] unitsMap = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number == 0)
                return "";

            if (number < 20)
                return unitsMap[number];

            if (number < 100)
                return tensMap[number / 10] + (number % 10 > 0 ? "-" + unitsMap[number % 10] : "");

            if (number < 1000)
                return unitsMap[number / 100] + " hundred" + (number % 100 > 0 ? " and " + ConvertToWords(number % 100) : "");

            if (number < 100000)
                return ConvertToWords(number / 1000) + " thousand" + (number % 1000 > 0 ? " " + ConvertToWords(number % 1000) : "");

            if (number < 10000000)
                return ConvertToWords(number / 100000) + " lakh" + (number % 100000 > 0 ? " " + ConvertToWords(number % 100000) : "");

            return ConvertToWords(number / 10000000) + " crore" + (number % 10000000 > 0 ? " " + ConvertToWords(number % 10000000) : "");
        }

        private static string ConvertToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return string.Join(" ", input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }

        public async Task<bool> UpdatePDFOnly(long salesId, string pdf = "")
        {
            try
            {
                var salesMaster = await _appDbContext.SalesMasters.Where(x => x.Id == salesId).FirstOrDefaultAsync();

                if (salesMaster != null)
                {
                    salesMaster.SalesBillImage = pdf;
                }

                await _appDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}