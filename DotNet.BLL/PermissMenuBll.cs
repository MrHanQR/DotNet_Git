using System;

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
                return DbSession.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
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
            return DbSession.SaveChanges() > 0;
        }
        private bool DeleteAny(string id)
        {
            if (id.Length != 36)
            {
                return false;
            }
            else
            {
                //删除Menu
                CurrentDal.ORMDeleteById(id);
                //删除RoleMenuButton
                DbSession.PermissRefRoleMenuButtonDal.ORMDeleteList(w => w.MenuId == new Guid(id));
                //删除UserMenuButton
                var menuButtonModels = DbSession.PermissRefMenuButtonDal.ORMLoadEntities(w => w.MenuId == new Guid(id));
                foreach (var permissRefMenuButton in menuButtonModels)
                {
                    DbSession.PermissRefUserMenuButtonDal.ORMDeleteList(w => w.MenuButtonId == permissRefMenuButton.Id);
                }
                //删除MenuButton
                DbSession.PermissRefMenuButtonDal.ORMDeleteList(w => w.MenuId == new Guid(id));
                return true;
            }
        }
    }
}