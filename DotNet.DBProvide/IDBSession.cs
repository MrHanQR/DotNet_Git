  
  

using DotNet.IDAL;
/**        数据库会话接口类-自动生成
 * 项目名：DotNet      程序集名：DotNet.DBProvide
 * 制作人：韩庆瑞      生成时间：11/01/2016 15:06:21
 * 生成工具：T4模板
 * 描述：
**/
namespace DotNet.DBProvide
{
	public interface IDBSession
	{ 
	   IPermissButtonDal PermissButtonDal { get; } 
	   IPermissDepartmentDal PermissDepartmentDal { get; } 
	   IPermissMenuDal PermissMenuDal { get; } 
	   IPermissRefMenuButtonDal PermissRefMenuButtonDal { get; } 
	   IPermissRefRoleMenuButtonDal PermissRefRoleMenuButtonDal { get; } 
	   IPermissRefUserDepartmentDal PermissRefUserDepartmentDal { get; } 
	   IPermissRefUserMenuButtonDal PermissRefUserMenuButtonDal { get; } 
	   IPermissRefUserRoleDal PermissRefUserRoleDal { get; } 
	   IPermissRoleDal PermissRoleDal { get; } 
	   IPermissUserDetailsDal PermissUserDetailsDal { get; } 
	   IPermissUserLoginDal PermissUserLoginDal { get; } 
	   IStateBugDal StateBugDal { get; } 
	   IStateLoginLogDal StateLoginLogDal { get; } 
	   IStateOperateLogDal StateOperateLogDal { get; } 
       int SaveChanges();
    }
}