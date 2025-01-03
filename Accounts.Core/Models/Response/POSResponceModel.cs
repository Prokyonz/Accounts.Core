using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Core.Models.Response
{
    public class POSResponceModel
    {
        public Int64 Id { get; set; }
        public string? TIDNumber { get; set; }
        public string? TIDBankName { get; set; }
        public bool IsActive { get; set; }
    }
}
