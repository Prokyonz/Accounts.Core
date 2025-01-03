using AutoMapper.Configuration.Annotations;
using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class POSMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? TIDNumber { get; set; }
        public string? TIDBankName { get; set; }
        public bool IsActive { get; set; }
    }

    public class POSChild : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Sr { get; set; }
        //public long Id { get; set; }
        //public long PermissionMasterId { get; set; }
        
        public long UserId { get; set; }
        public long POSId { get; set; }

        [ForeignKey("UserId")]
        [Ignore]
        public UserMaster? UserMaster { get; set; }
    }

    public class SeriesMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
