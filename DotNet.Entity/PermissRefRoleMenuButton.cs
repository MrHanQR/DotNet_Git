namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissRefRoleMenuButton")]
    public partial class PermissRefRoleMenuButton
    {
        public PermissRefRoleMenuButton()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public Guid MenuId { get; set; }

        public Guid? ButtonId { get; set; }
    }
}
