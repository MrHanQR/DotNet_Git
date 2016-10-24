using System;
using System.Collections.Generic;
using DotNet.Entity;

namespace DotNet.MVC.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime AddDate { get; set; }

        public DateTime ModifyDate { get; set; }
        public string Department { get; set; }
        
    }
}