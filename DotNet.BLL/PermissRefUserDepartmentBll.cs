using System;
using System.Linq;
using DotNet.Entity;
using DotNet.Entity.Enum;

namespace DotNet.BLL
{
    public partial class PermissRefUserDepartmentBll
    {
        public bool ORMSetUserDep(Guid depId, string[] userIdArr)
        {
            Guid userId;
            for (int i = 0; i < userIdArr.Length; i++)
            {
                if (Guid.TryParse(userIdArr[i], out userId))//是GUID
                {
                    var userModel = DbSession.PermissUserLoginDal.ORMLoadEntities(u => u.Id == userId && u.DeleteFlag == DelFlagEnum.Normal).FirstOrDefault();
                    if (userModel != null)//有这个用户
                    {
                        var userDepModel = DbSession.PermissRefUserDepartmentDal.ORMLoadEntities(u => u.UserId == userModel.Id).FirstOrDefault();

                        //1用户没部门 设置部门
                        if (userDepModel == null)
                        {
                            userDepModel = new PermissRefUserDepartment();
                            userDepModel.UserId = userModel.Id;
                            userDepModel.DepartmentId = depId;
                            DbSession.PermissRefUserDepartmentDal.ORMAdd(userDepModel);
                        }
                        //2用户有部门 更新部门
                        else
                        {
                            userDepModel.DepartmentId = depId;
                            DbSession.PermissRefUserDepartmentDal.ORMUpdate(userDepModel);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (DbSession.SaveChanges()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}