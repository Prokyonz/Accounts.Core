using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Core.Models
{
    public class ItemMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public decimal Rate { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal CGSTRate { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal IGSTRate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
