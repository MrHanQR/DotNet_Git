namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateBug")]
    public partial class StateBug
    {
        public StateBug()
        {
            Id=Guid.NewGuid();
            BugDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(24)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15)]
        public string UserIp { get; set; }

        [Required]
        public string BugInfo { get; set; }

        public string BugReply { get; set; }

        public DateTime BugDate { get; set; }

        public bool IfShow { get; set; }

        public bool IfSolve { get; set; }
    }
}
