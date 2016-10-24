using System;
using DotNet.Entity.Enum;

namespace DotNet.MVC.Models
{
    public class UserDetailsViewModel
    {
        public Guid Id { get; set; }
        public string LoginId { get; set; }
        public string LoginPwd { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string IsAble { get; set; }
        public string DeleteFlag { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ShortDescription { get; set; }
        public string PhotoPath { get; set; }
        public string RealName { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string IdentityCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        
    }
}