namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissRefMenuButton")]
    public partial class PermissRefMenuButton
    {
        public PermissRefMenuButton()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public Guid MenuId { get; set; }

        public Guid ButtonId { get; set; }
    }
}
