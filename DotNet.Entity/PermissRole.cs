namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissRole")]
    public partial class PermissRole
    {
        public PermissRole()
        {
            Id = Guid.NewGuid();
            AddDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(16)]
        public string RoleName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime ModifyDate { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
