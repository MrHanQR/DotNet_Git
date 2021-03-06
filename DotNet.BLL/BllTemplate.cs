﻿  
  

using DotNet.Entity;
using DotNet.IBLL;
/**        业务逻辑类-自动生成
 * 项目名：DotNet      程序集名：DotNet.BLL
 * 制作人：韩庆瑞      生成时间：11/01/2016 15:09:56
 * 生成工具：T4模板
 * 描述：
**/
namespace DotNet.BLL
{
	public partial class PermissButtonBll :BaseBll<PermissButton>,IPermissButtonBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissButtonDal;
        }
    } 	 
	public partial class PermissDepartmentBll :BaseBll<PermissDepartment>,IPermissDepartmentBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissDepartmentDal;
        }
    } 	 
	public partial class PermissMenuBll :BaseBll<PermissMenu>,IPermissMenuBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissMenuDal;
        }
    } 	 
	public partial class PermissRefMenuButtonBll :BaseBll<PermissRefMenuButton>,IPermissRefMenuButtonBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRefMenuButtonDal;
        }
    } 	 
	public partial class PermissRefRoleMenuButtonBll :BaseBll<PermissRefRoleMenuButton>,IPermissRefRoleMenuButtonBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRefRoleMenuButtonDal;
        }
    } 	 
	public partial class PermissRefUserDepartmentBll :BaseBll<PermissRefUserDepartment>,IPermissRefUserDepartmentBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRefUserDepartmentDal;
        }
    } 	 
	public partial class PermissRefUserMenuButtonBll :BaseBll<PermissRefUserMenuButton>,IPermissRefUserMenuButtonBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRefUserMenuButtonDal;
        }
    } 	 
	public partial class PermissRefUserRoleBll :BaseBll<PermissRefUserRole>,IPermissRefUserRoleBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRefUserRoleDal;
        }
    } 	 
	public partial class PermissRoleBll :BaseBll<PermissRole>,IPermissRoleBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissRoleDal;
        }
    } 	 
	public partial class PermissUserDetailsBll :BaseBll<PermissUserDetails>,IPermissUserDetailsBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissUserDetailsDal;
        }
    } 	 
	public partial class PermissUserLoginBll :BaseBll<PermissUserLogin>,IPermissUserLoginBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.PermissUserLoginDal;
        }
    } 	 
	public partial class StateBugBll :BaseBll<StateBug>,IStateBugBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.StateBugDal;
        }
    } 	 
	public partial class StateLoginLogBll :BaseBll<StateLoginLog>,IStateLoginLogBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.StateLoginLogDal;
        }
    } 	 
	public partial class StateOperateLogBll :BaseBll<StateOperateLog>,IStateOperateLogBll
    {
       public override void SetCurrentDal()
        {
            CurrentAdoDal = DbSession.StateOperateLogDal;
        }
    } 	 
}
