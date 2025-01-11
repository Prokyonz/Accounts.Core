using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class Stock : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal CGSTRate { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal IGSTRate { get; set; }
        public bool IsDelete { get; set; }
    }
}
