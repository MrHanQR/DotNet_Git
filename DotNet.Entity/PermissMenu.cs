namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissMenu")]
    public partial class PermissMenu
    {
        public PermissMenu()
        {
            Id = Guid.NewGuid();
            AddDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(16)]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        [StringLength(16)]
        public string Icon { get; set; }

        public int Sort { get; set; }

        public DateTime AddDate { get; set; }

        [StringLength(32)]
        public string ControllerNameCode { get; set; }

        [StringLength(32)]
        public string ActionNameCode { get; set; }

        public bool HaveChild { get; set; }

        public int MenuLevel { get; set; }
    }
}
