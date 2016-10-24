namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissButton")]
    public partial class PermissButton
    {
        public PermissButton()
        {
            Id = Guid.NewGuid();
            AddDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [StringLength(8)]
        public string Name { get; set; }

        [StringLength(16)]
        public string Icon { get; set; }
        public int Sort { get; set; }

        public DateTime AddDate { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(4)]
        public string HttpMethod { get; set; }

        [Required]
        [StringLength(24)]
        public string ActionNameCode { get; set; }
    }
}
