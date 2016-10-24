using System;
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
                return DbSession.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
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
            return DbSession.SaveChanges() > 0;
        }

        private bool DeleteAny(string strId)
        {
            Guid id;
            if (Guid.TryParse(strId, out id))
            {
                //删除Button
                CurrentDal.ORMDeleteById(id);
                //删除RoleMenuButton
                DbSession.PermissRefRoleMenuButtonDal.ORMDeleteList(w => w.ButtonId == id);
                //删除UserMenuButton
                var menuButtonModels = DbSession.PermissRefMenuButtonDal.ORMLoadEntities(w => w.ButtonId == id);
                foreach (var permissRefMenuButton in menuButtonModels)
                {
                    DbSession.PermissRefUserMenuButtonDal.ORMDeleteList(w => w.MenuButtonId == permissRefMenuButton.Id);
                }
                //删除MenuButton
                DbSession.PermissRefMenuButtonDal.ORMDeleteList(w => w.ButtonId == id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}