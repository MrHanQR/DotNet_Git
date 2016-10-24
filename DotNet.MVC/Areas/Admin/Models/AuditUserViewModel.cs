using System;

namespace DotNet.MVC.Models
{
    
    public class AuditUserViewModel
    {
        public Guid Id { get; set; }
        public string LoginId { get; set; }
        public string LoginPwd { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime ApplyDate { get; set; }
    }
}