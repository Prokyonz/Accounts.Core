using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class PermissionMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Sr { get; set; }
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string KeyName { get; set; }
    }

    public class UserPermissionChild : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Sr { get; set; }
        public long Id { get; set; }
        public long PermissionMasterId { get; set; }
        public long UserId { get; set; }
        public string KeyName { get; set; }
        public bool Status { get; set; } //Permission Status true/false
    }
}
