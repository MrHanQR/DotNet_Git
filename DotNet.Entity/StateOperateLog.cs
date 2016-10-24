namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateOperateLog")]
    public partial class StateOperateLog
    {
        public StateOperateLog()
        {
            Id = Guid.NewGuid();
            OperateDate = DateTime.Now;
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(24)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15)]
        public string UserIp { get; set; }

        [StringLength(64)]
        public string OperateInfo { get; set; }

        public string Description { get; set; }

        public bool IfSuccess { get; set; }

        public DateTime OperateDate { get; set; }
    }
}
