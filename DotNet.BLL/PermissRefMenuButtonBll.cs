using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNet.Entity;

namespace DotNet.BLL
{
    public partial class PermissRefMenuButtonBll
    {
        /// <summary>
        /// 修改菜单的按钮，删除原来的，添加最新的
        /// 操作RoleMenuButton关系表，删除Role对应的Menu已经删除的Button
        /// </summary>
        /// <param name="whereLambda">要删除部分的查询条件</param>
        /// <param name="idList">新的按钮Id集合</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        public bool ORMChangeButton(string[] idList, string menuId)
        {
             Guid gMenuId=new Guid(menuId);
            //删除该Menu原来的Button
           CurrentDal.ORMDeleteList(m=>m.MenuId==gMenuId);
            //添加新的MenuButton
            for (int i = 0; i < idList.Length; i++)
            {
                PermissRefMenuButton model=new PermissRefMenuButton();
                Guid buttonId = new Guid(idList[i]);
                model.ButtonId = buttonId;
                model.MenuId = gMenuId;
                CurrentDal.ORMAdd(model);
            }
            return DbSession.SaveChanges() > 0;
        }
    }
}