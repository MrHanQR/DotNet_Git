using System.Collections.Generic;
using DotNet.Entity;

namespace DotNet.IBLL
{
    public partial interface IPermissDepartmentBll
    {
        /// <summary>
        /// 构建整个部门树
        /// </summary>
        /// <param name="allDeps">部门列表</param>
        /// <param name="rootDepsList">根部门集合</param>
        /// <returns>拼接好的html字符串</returns>
        string CreateDepartmentTree(IList<PermissDepartment> allDeps, IList<PermissDepartment> rootDepsList);

        /// <summary>
        /// 构建整个部门树
        /// </summary>
        /// <param name="allDeps">部门列表</param>
        /// <returns>拼接好的html字符串</returns>
        string CreateDepartmentTree(IList<PermissDepartment> allDeps);

        /// <summary>
        /// 获取后代部门集合
        /// </summary>
        /// <param name="allDepartments"></param>
        /// <param name="fatherNode"></param>
        /// <param name="progenList"></param>
        void GetProgenyDepartmentList(IList<PermissDepartment> allDepartments, PermissDepartment fatherNode,
            IList<PermissDepartment> progenList);
    }
}