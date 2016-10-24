
using DotNet.IDAL;
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