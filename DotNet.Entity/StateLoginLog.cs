namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateLoginLog")]
    public partial class StateLoginLog
    {
        public StateLoginLog()
        {
            Id = Guid.NewGuid();
            LoginDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(24)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15)]
        public string UserIp { get; set; }

        [Required]
        [StringLength(32)]
        public string City { get; set; }

        public bool Success { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
