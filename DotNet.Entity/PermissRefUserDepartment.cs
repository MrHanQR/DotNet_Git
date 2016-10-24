namespace DotNet.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissRefUserDepartment")]
    public partial class PermissRefUserDepartment
    {
        public PermissRefUserDepartment()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
