using System.Web.Mvc;
using DotNet.Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DotNet.Entity
{
    [Table("PermissUserLogin")]
    public partial class PermissUserLogin
    {
        public PermissUserLogin()
        {
            Id = Guid.NewGuid();
            ApplyDate = DateTime.Now;
            DeleteFlag = DelFlagEnum.Normal;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(24)]
        public string LoginId { get; set; }

        [Required]
        [StringLength(24)]
        public string UserName { get; set; }

        [Required]
        [MinLength(32),StringLength(32)]
        public string LoginPwd { get; set; }

        public bool IsAble { get; set; }

        public DateTime? AddDate { get; set; }

        [StringLength(40)]
        public string ShortDescription { get; set; }

        [Required]
        [StringLength(32)]
        public string UserEmail { get; set; }
        [StringLength(200)]
        public string PhotoPath { get; set; }
        public DateTime ApplyDate { get; set; }
        public DelFlagEnum DeleteFlag { get; set; }
    }
}
