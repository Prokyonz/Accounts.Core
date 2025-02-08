using System;
using System.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using OfficeOpenXml;

class Program
{
    static async Task Main(string[] args)
    {
        string filePath = "C:\\Users\\abhis\\Downloads\\Parth.xlsx";
        string connectionString = "your_connection_string";
        string credentialsPath = "C:\\Users\\abhis\\Downloads\\my-drive-privte-key-file.json";

        var driveService = AuthenticateGoogleDrive(credentialsPath);
        DataTable dataTable = await ReadExcelAndConvertToBase64(filePath, driveService);
        BulkInsertToSQL(dataTable, connectionString);
    }

    static DriveService AuthenticateGoogleDrive(string credentialsPath)
    {
        GoogleCredential credential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
        }
        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "GoogleDriveFileDownload"
        });
    }

    static async Task<DataTable> ReadExcelAndConvertToBase64(string filePath, DriveService driveService)
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[] {
            new DataColumn("Timestamp", typeof(DateTime)),
            new DataColumn("CustomerName", typeof(string)),
            new DataColumn("PanCardNo", typeof(string)),
            new DataColumn("AdharCardNo", typeof(string)),
            new DataColumn("AdharAddress", typeof(string)),
            new DataColumn("PinCode", typeof(string)),
            new DataColumn("PanCardPdfBase64", typeof(string)),
            new DataColumn("AdharFrontPdfBase64", typeof(string)),
            new DataColumn("AdharBackPdfBase64", typeof(string))
        });

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[1];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                DataRow dr = dt.NewRow();
                /*dr["Timestamp"] = DateTime.Parse(worksheet.Cells[row, 1].Text);
                dr["CustomerName"] = worksheet.Cells[row, 2].Text;
                dr["PanCardNo"] = worksheet.Cells[row, 3].Text;
                dr["AdharCardNo"] = worksheet.Cells[row, 4].Text;
                dr["AdharAddress"] = worksheet.Cells[row, 5].Text;
                dr["PinCode"] = worksheet.Cells[row, 6].Text;
                */
                dr["PanCardPdfBase64"] = await DownloadPdfAndConvertToBase64(driveService, worksheet.Cells[row, 7].Text);
                dr["AdharFrontPdfBase64"] = await DownloadPdfAndConvertToBase64(driveService, worksheet.Cells[row, 8].Text);
                dr["AdharBackPdfBase64"] = await DownloadPdfAndConvertToBase64(driveService, worksheet.Cells[row, 9].Text);
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    static async Task<string> DownloadPdfAndConvertToBase64(DriveService driveService, string fileUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return string.Empty;

            string fileId = ExtractFileIdFromGoogleDriveUrl(fileUrl);
            if (string.IsNullOrEmpty(fileId)) return string.Empty;

            var request = driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            return Convert.ToBase64String(stream.ToArray());
        }
        catch
        {
            return string.Empty;
        }
    }

    static string ExtractFileIdFromGoogleDriveUrl(string url)
    {
        if (url.Contains("drive.google.com/file/d/"))
        {
            return url.Split(new string[] { "/d/", "/view" }, StringSplitOptions.RemoveEmptyEntries)[1];
        }
        return string.Empty;
    }

    static void BulkInsertToSQL(DataTable dt, string connectionString)
    {
        /*using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.DestinationTableName = "CustomerData";
                bulkCopy.WriteToServer(dt);
            }
        }*/
    }
}
