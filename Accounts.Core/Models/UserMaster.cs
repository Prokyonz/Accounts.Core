﻿using AutoMapper.Configuration.Annotations;
using BaseClassLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Core.Models
{
    public class UserMaster : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public long? Pin { get; set; }
        public string? EmailId { get; set; }
        public bool? IsAgent { get; set; }
        public long? ParentUserId { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        [Ignore]
        public List<POSChild>? POSChilds { get; set; }

        [Ignore]
        public List<UserPermissionChild>? Permissions { get; set;}
    }
}
