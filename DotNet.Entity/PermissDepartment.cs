namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissDepartment")]
    public partial class PermissDepartment
    {
        public PermissDepartment()
        {
            Id = Guid.NewGuid();
            AddDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(24)]
        public string DepartmentName { get; set; }

        public Guid? ParentId { get; set; }

        public int Sort { get; set; }

        public DateTime AddDate { get; set; }
        public bool HaveChild { get; set; }
        public int DepartmentLevel { get; set; }
    }
}
