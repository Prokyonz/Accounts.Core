using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class Customer : BaseClassLibrary.Models.BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string AadharNo { get; set; }
        public string PanNo { get; set; }
        public string AadharImageFrontData { get; set; }
        public string AadhbarImageBackData { get; set; }
        public string? AadharImageFileName { get; set; }
        public string? PanImageFileName { get; set; }
        public string PanImageData { get; set; }
        public string? SignatureFileName { get; set; }
        public string SignatureImageData { get; set; }
    }
}
