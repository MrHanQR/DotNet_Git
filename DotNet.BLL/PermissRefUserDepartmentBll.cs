using System;
using System.Data.Entity;
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
                    var userLoginContext = DbContext.Set<PermissUserLogin>();
                    var userModel = userLoginContext.Single(u => u.Id == userId && u.DeleteFlag == DelFlagEnum.Normal);
                    if (userModel != null)//有这个用户
                    {
                        var userDepartmentContext = DbContext.Set<PermissRefUserDepartment>();
                        var userDepModel = userDepartmentContext.Single(u => u.UserId == userModel.Id);

                        //1用户没部门 设置部门
                        if (userDepModel == null)
                        {
                            userDepModel = new PermissRefUserDepartment();
                            userDepModel.UserId = userModel.Id;
                            userDepModel.DepartmentId = depId;
                            userDepartmentContext.Add(userDepModel);
                        }
                        //2用户有部门 更新部门
                        else
                        {
                            userDepModel.DepartmentId = depId;
                            DbContext.Entry(userDepModel).State = EntityState.Modified;
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
            return DbContext.SaveChanges() > 0;
        }
    }
}