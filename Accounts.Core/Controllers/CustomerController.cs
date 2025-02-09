using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using System.Diagnostics;

using Microsoft.Data.SqlClient;
using Accounts.Core.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Ghostscript.NET.Rasterizer;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerMasterRepository _customerMasterRepository;
        private readonly AppDbContext _appDbContext;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerMasterRepository customerMasterRepository, AppDbContext appDbContext)
        {
            _logger = logger;
            _customerMasterRepository = customerMasterRepository;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            var result = await _customerMasterRepository.GetAllCustomers();
            return result;
        }

        /// <summary>
        /// Read all Sale from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCustomerByUser/{userId}")]
        public async Task<List<Customer>> GetPOSByUser(long userId)
        {
            return await _customerMasterRepository.GetCustomerByUser(userId);
        }

        [HttpGet("GetCustomer/{customerId}")]
        public async Task<Customer> GetRow(long customerId, int pageIndex, int pageSize)
        {
            var result = await _customerMasterRepository.GetQuery(customerId, 0, 1);
            return result;
        }

        [HttpGet("GetCustomersWithPagging")]
        public async Task<List<Customer>> GetCustomerWithPagging(int pageIndex, int pageSize)
        {
            var result = await _customerMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<Customer> Post(Customer customer)
        {
            try
            {
                return await _customerMasterRepository.AddCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<Customer> Put(Customer customer)
        {
            try
            {
                return await _customerMasterRepository.UpdateCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(long customerId, bool isHardDelete = false)
        {
            try
            {
                return await _customerMasterRepository.DeleteCustomerAsync(customerId, isHardDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("CustomerReport")]
        public async Task<List<CustomerReport>> GetCustomerReport([FromQuery] long userId, [FromQuery] string? name)
        {
            return await _customerMasterRepository.CustomerReport(userId, name);
        }

        [HttpGet("LoadCustomers")]
        public async Task LoanCustomers()
        {
            try
            {
                var customerDt = ReadExcelFile();

                foreach (var item in customerDt?.Rows)
                {
                    DataRow row = ((System.Data.DataRow)item);

                    var customer = new Customer
                    {
                        FirstName = row.ItemArray[0].ToString(),
                        LastName = row.ItemArray[1].ToString(),
                        PanNo = row.ItemArray[2].ToString(),
                        AadharNo = row.ItemArray[3].ToString(),
                        Address = row.ItemArray[4].ToString(),
                        Pincode = long.Parse(row.ItemArray[5].ToString()),
                        PanImageData = row.ItemArray[6].ToString(),
                        AadharImageFrontData = row.ItemArray[7].ToString(),
                        AadhbarImageBackData = row.ItemArray[8].ToString(),
                        EmailId = "",
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        MobileNo = "1234567890",
                        SignatureImageData = row.ItemArray[6].ToString()
                    };

                    string query = @"
                        INSERT INTO CustomerMaster 
                        (FirstName, LastName, PanNo, AadharNo, Address, Pincode, [PanImageData], 
                         [AadharImageFrontData], [AadhbarImageBackData], EmailId, CreatedDate, 
                         IsDelete, MobileNo, [SignatureImageData])
                        VALUES 
                        (@FirstName, @LastName, @PanNo, @AadharNo, @Address, @Pincode, @PanImageData, 
                         @AadharImageFrontData, @AadharImageBackData, @EmailId, @CreatedDate, 
                         @IsDelete, @MobileNo, @SignatureImageData)";

                    using (SqlConnection sqlConnection = new SqlConnection(_appDbContext.Database.GetDbConnection().ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                            cmd.Parameters.AddWithValue("@PanNo", customer.PanNo);
                            cmd.Parameters.AddWithValue("@AadharNo", customer.AadharNo);
                            cmd.Parameters.AddWithValue("@Address", customer.Address);
                            cmd.Parameters.AddWithValue("@Pincode", customer.Pincode);
                            cmd.Parameters.AddWithValue("@PanImageData", customer.PanImageData);
                            cmd.Parameters.AddWithValue("@AadharImageFrontData", customer.AadharImageFrontData);
                            cmd.Parameters.AddWithValue("@AadharImageBackData", customer.AadhbarImageBackData);
                            cmd.Parameters.AddWithValue("@EmailId", customer.EmailId);
                            cmd.Parameters.AddWithValue("@CreatedDate", customer.CreatedDate);
                            cmd.Parameters.AddWithValue("@IsDelete", customer.IsDelete);
                            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                            cmd.Parameters.AddWithValue("@SignatureImageData", customer.SignatureImageData);

                            await sqlConnection.OpenAsync();
                            int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        }
                        //await _customerMasterRepository.AddCustomerAsync(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        DataTable ReadExcelFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string filePath = "C:\\Users\\abhis\\Downloads\\Parth.xlsx";

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("CustomerName", typeof(string)),
                new DataColumn("LastName", typeof(string)),
                new DataColumn("PanCardNo", typeof(string)),
                new DataColumn("AdharCardNo", typeof(string)),
                new DataColumn("AdharAddress", typeof(string)),
                new DataColumn("PinCode", typeof(string)),
                new DataColumn("PanCardPdfBase64", typeof(string)),
                new DataColumn("AdharFrontPdfBase64", typeof(string)),
                new DataColumn("AdharBackPdfBase64", typeof(string)),
                new DataColumn("UserId", typeof(string))
            });

            if (System.IO.File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text))
                        {
                            DataRow dr = dt.NewRow();
                            dr["CustomerName"] = worksheet.Cells[row, 2].Text;
                            //dr["LastName"] = worksheet.Cells[row, 2].Text;
                            dr["PanCardNo"] = worksheet.Cells[row, 3].Text;
                            dr["AdharCardNo"] = worksheet.Cells[row, 4].Text;
                            dr["AdharAddress"] = worksheet.Cells[row, 5].Text;
                            dr["PinCode"] = worksheet.Cells[row, 6].Text;

                            var panUrl = worksheet.Cells[row, 7].Text;
                            var AdaharFUrl = worksheet.Cells[row, 8].Text;
                            var AdharBUrl = worksheet.Cells[row, 9].Text;


                            var panBase64 = DownlaodFileFromGoodle(panUrl);

                            Thread.Sleep(3000);

                            var adaharFUrlBase64 = DownlaodFileFromGoodle(AdaharFUrl);

                            Thread.Sleep(3000);

                            var adaharBUrlBase64 = DownlaodFileFromGoodle(AdharBUrl);

                            dr["PanCardPdfBase64"] = panBase64;
                            dr["AdharFrontPdfBase64"] = adaharFUrlBase64;
                            dr["AdharBackPdfBase64"] = adaharBUrlBase64;
                            dr["UserId"] = 0; // worksheet.Cells[row, 10].Text

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Excel file not found");
            }

            return dt;
        }

        string DownlaodFileFromGoodle(string url)
        {
            string id = url.Split("?id=")[1].ToString();

            //string downloadUrl = $"https://drive.usercontent.google.com/u/1/uc?id="+id+"&export=download";
            string downloadUrl = "https://drive.usercontent.google.com/download?id=" + id + "&export=download";

            Process.Start(new ProcessStartInfo
            {
                FileName = downloadUrl,
                UseShellExecute = true, // Ensures the URL is opened in the browser
            });

            var list = new DirectoryInfo(@"C:\Users\abhis\Downloads").GetFiles().OrderByDescending(x => x.LastWriteTime).ToList().FirstOrDefault();
            string filePath = list?.FullName;
            string base64 = "";

            if (list.FullName.ToLower().Contains("pdf"))
            {
                using (var rasterizer = new GhostscriptRasterizer())
                {
                    rasterizer.Open(filePath);

                    // Render first page as an image (600 DPI for good quality)
                    using (var image = rasterizer.GetPage(600, 1))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);
                            base64 = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }

                System.IO.File.Delete(filePath);

                return base64;
            }
            else
            {
                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    string base64String = Convert.ToBase64String(fileBytes);

                    System.IO.File.Delete(filePath);
                    return "data:image/"+ Path.GetExtension(filePath).ToLower() + ";base64," + base64String;
                } 
            }

            return "";
        }

    }
}