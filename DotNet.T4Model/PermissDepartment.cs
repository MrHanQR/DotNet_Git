//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotNet.T4Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PermissDepartment
    {
        public System.Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public int Sort { get; set; }
        public System.DateTime AddDate { get; set; }
        public bool HaveChild { get; set; }
    }
}
