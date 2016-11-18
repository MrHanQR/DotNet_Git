using System;
using System.Linq;
using DotNet.Entity;

namespace DotNet.BLL
{
    public partial class PermissMenuBll
    {
        /// <summary>
        /// 删除数据库中该菜单所有的相关信息
        /// Menu MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool DeleteMenuAtAnyWhere(string id)
        {
            if (DeleteAny(id))
            {
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 删除数据库中该菜单所有的相关信息
        /// Menu MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="idArr">主键数组</param>
        /// <returns></returns>
        public bool DeleteMenuAtAnyWhere(string[] idArr)
        {
            for (int i = 0; i < idArr.Length; i++)
            {
                if (DeleteAny(idArr[i]))
                {
                    return false;
                }
            }
            return DbContext.SaveChanges() > 0;
        }
        private bool DeleteAny(string primaryKeyValue)
        {
            Guid id;
            if (Guid.TryParse(primaryKeyValue, out id))
            {
                //删除Menu
                Delete(id);
                //删除RoleMenuButton
                var roleMenuButtonContext = DbContext.Set<PermissRefRoleMenuButton>();
                roleMenuButtonContext.RemoveRange(roleMenuButtonContext.Where(w => w.MenuId == id));
                //删除UserMenuButton
                var userMenuButtonContext = DbContext.Set<PermissRefUserMenuButton>();
                var menuButtonContext = DbContext.Set<PermissRefMenuButton>();
                IQueryable<PermissRefMenuButton> userMenuButtonCollection = menuButtonContext.Where(w => w.MenuId == id);
                foreach (var item in userMenuButtonCollection)
                {
                    userMenuButtonContext.RemoveRange(userMenuButtonContext.Where(w => w.MenuButtonId == item.Id));
                }
                //删除MenuButton
                menuButtonContext.RemoveRange(userMenuButtonCollection);
                return true;
            }
            return false;
        }
    }
}