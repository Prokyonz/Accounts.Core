namespace Accounts.Core.Models.Response
{
    public class UserReport
    {
        public long? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public string? EmailId { get; set; }
        public bool? IsAgent { get; set; }
        public long? ParentUserId { get; set; }
        public bool? IsActive { get; set; }
    }
}
