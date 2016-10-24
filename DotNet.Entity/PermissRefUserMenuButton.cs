namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissRefUserMenuButton")]
    public partial class PermissRefUserMenuButton
    {
        public PermissRefUserMenuButton()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public bool IsPass { get; set; }

        public Guid UserId { get; set; }

        public Guid MenuButtonId { get; set; }
    }
}
