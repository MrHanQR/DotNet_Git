using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Entity;

namespace DotNet.BLL
{
    public partial class PermissDepartmentBll
    {
        /// <summary>
        /// 构建整个部门树
        /// </summary>
        /// <param name="allDeps">部门列表</param>
        /// <param name="rootDepsList">根部门集合</param>
        /// <returns>拼接好的html字符串</returns>
        public string CreateDepartmentTree(IList<PermissDepartment> allDeps, IList<PermissDepartment> rootDepsList)
        {
            if (allDeps.Count == 0)//如果部门列表空，则返回空
            {
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                if (rootDepsList == null)
                {
                    //先拿到根节点，构建根
                    rootDepsList = (from d in allDeps
                                    where d.ParentId == null
                                    select d).OrderBy(d => d.Sort).ToList();
                }
                foreach (var item in rootDepsList)
                {
                    sb.Append(CreateTree(item, allDeps));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 构建整个部门树
        /// </summary>
        /// <param name="allDeps">部门列表</param>
        /// <returns>拼接好的html字符串</returns>
        public string CreateDepartmentTree(IList<PermissDepartment> allDeps)
        {
            return CreateDepartmentTree(allDeps, null);
        }

        private string CreateTree(PermissDepartment root, IList<PermissDepartment> allDepartments)
        {
            StringBuilder sb = new StringBuilder();
            if (root.ParentId == null)//是根节点
            {
                if (root.HaveChild)//有孩子
                {
                    sb.Append("<ul>");
                    //先添加自己
                    sb.Append("<li>");
                    sb.AppendFormat("<span>" +
                                    "<i class='glyphicon glyphicon-home'></i>" +
                                    " <input type='checkbox' name='checkDepId' class='checkboxId' value='{0}' sort='{1}' parentId='-1' haveChild=True txt='{2}'>{2}</span>", root.Id, root.Sort, root.DepartmentName);
                    sb.AppendFormat("<a href='javascript:void(0)' onclick=openUserList('{0}')>查看</a>", root.Id);
                    sb.Append("<ul>");
                    //再遍历集合添加孩子
                    foreach (var item in allDepartments)
                    {
                        if (item.ParentId == root.Id)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<span>" +
                                            "<i class='glyphicon glyphicon-flag'></i>" +
                                            " <input type='checkbox' name='checkDepId' class='checkboxId' value='{0}' sort='{1}' parentId='{2}' haveChild={3} txt='{4}'>{4}</span>", item.Id, item.Sort, item.ParentId, item.HaveChild, item.DepartmentName);
                            sb.AppendFormat("<a href='javascript:void(0)' onclick=openUserList('{0}')>查看</a>", item.Id);
                            sb.Append(CreateTree(item, allDepartments));
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                    sb.Append("</ul>");
                }
                else//无孩子的根节点
                {
                    sb.Append("<ul>");
                    sb.Append("<li>");
                    sb.AppendFormat("<span>" +
                                    "<i class='glyphicon glyphicon-home'></i>" +
                                    " <input type='checkbox' name='checkDepId' class='checkboxId' value='{0}' sort='{1}' parentId='-1' haveChild=False txt='{2}'>{2}</span>", root.Id, root.Sort, root.DepartmentName);
                    sb.AppendFormat("<a href='javascript:void(0)' onclick=openUserList('{0}')>查看</a>", root.Id);

                    sb.Append("</li>");
                    sb.Append("</ul>");
                }
            }
            else//不是根节点
            {
                if (root.HaveChild)
                {
                    sb.Append("<ul>");
                    foreach (var item in allDepartments)
                    {
                        if (item.ParentId == root.Id)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<span>" +
                                            "<i class='glyphicon glyphicon-flag'></i>" +
                                            " <input type='checkbox' name='checkDepId' class='checkboxId' value='{0}' sort='{1}' parentId='{2}' haveChild={3} txt='{4}'>{4}</span>", item.Id, item.Sort, item.ParentId, item.HaveChild, item.DepartmentName);
                            sb.AppendFormat("<a href='javascript:void(0)' onclick=openUserList('{0}')>查看</a>", item.Id);
                            sb.Append(CreateTree(item, allDepartments));
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取后代部门集合
        /// </summary>
        /// <param name="allDepartments"></param>
        /// <param name="fatherNode"></param>
        /// <param name="progenList"></param>
        public void GetProgenyDepartmentList(IList<PermissDepartment> allDepartments, PermissDepartment fatherNode, IList<PermissDepartment> progenList)
        {
            foreach (var item in allDepartments)
            {
                if (item.ParentId == fatherNode.Id)
                {
                    progenList.Add(item);
                    GetProgenyDepartmentList(allDepartments, item, progenList);
                }
            }
        }
    }
}