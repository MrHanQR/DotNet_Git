using System;
using System.Data.Entity;
using System.Linq;
using DotNet.Entity;

namespace DotNet.BLL
{
    public partial class PermissButtonBll
    {
        //删除这个按钮
        //删除MenuButton
        //删除RoleMenuButton
        //删除UserMenuButton
        /// <summary>
        /// 删除数据库中该按钮所有的相关信息
        /// Button MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool DeleteButonAtAnyWhere(string id)
        {
            if (!DeleteAny(id))
            {
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }
        /// <summary>
        /// 删除数据库中该按钮所有的相关信息
        /// Button MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="idArr">主键数组</param>
        /// <returns></returns>
        public bool DeleteButonAtAnyWhere(string[] idArr)
        {
            for (int i = 0; i < idArr.Length; i++)
            {
                if (!DeleteAny(idArr[i]))
                {
                    return false;
                }
            }
            return DbContext.SaveChanges() > 0;
        }

        private bool DeleteAny(string strId)
        {
            Guid id;
            if (Guid.TryParse(strId, out id))
            {
                //删除Button
                Delete(id);
                //删除RoleMenuButton
                var roleMenuButtonContext = DbContext.Set<PermissRefRoleMenuButton>();
                roleMenuButtonContext.RemoveRange(roleMenuButtonContext.Where(w => w.ButtonId == id));
                //删除UserMenuButton
                var menuButtonContext = DbContext.Set<PermissRefMenuButton>();
                var userMenuButtonContext = DbContext.Set<PermissRefUserMenuButton>();
                IQueryable<PermissRefMenuButton> menuButtonCollection = menuButtonContext.Where(w => w.ButtonId == id);
                foreach (PermissRefMenuButton item in menuButtonCollection)
                {
                    userMenuButtonContext.RemoveRange(userMenuButtonContext.Where(w => w.MenuButtonId == item.Id));
                }
                //删除MenuButton
                menuButtonContext.RemoveRange(menuButtonCollection);
                return true;
            }
            return false;
        }
    }
}