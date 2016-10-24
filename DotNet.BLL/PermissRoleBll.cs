using System;

namespace DotNet.BLL
{
    public partial class PermissRoleBll
    {
        /// <summary>
        /// 删除数据库中该角色所有的相关信息
        /// 删除Role UserRole RoleMenuButton
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
        /// 删除数据库中该角色所有的相关信息
        /// 删除Role UserRole RoleMenuButton
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
            if (Guid.TryParse(strId,out id ))
            {
                //删除Role
                CurrentDal.ORMDeleteById(id);
                //删除RoleMenuButton
                DbSession.PermissRefRoleMenuButtonDal.ORMDeleteList(w => w.RoleId == id);
                //删除UserRole
                DbSession.PermissRefUserRoleDal.ORMDeleteList(u => u.RoleId == id);
                return true;
            }
            else
            {
               return false;
            }
        }
    }
}