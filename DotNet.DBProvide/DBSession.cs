  
  

using DotNet.IDAL;
using DotNet.DAL.Factory;
/**        数据库会话类-自动生成
 * 项目名：DotNet      程序集名：DotNet.DBProvide
 * 制作人：韩庆瑞      生成时间：10/31/2016 17:22:28
 * 生成工具：T4模板
 * 描述：
**/
namespace DotNet.DBProvide
{
    public partial class DBSession:IDBSession
	{   
	    private IPermissButtonDal _PermissButtonDal;
		public IPermissButtonDal PermissButtonDal
		{
			get
			{
				if (_PermissButtonDal == null)
				{
					_PermissButtonDal = DalFactory.GetPermissButtonDal();
				}
				return _PermissButtonDal;
			}
		}
	    private IPermissDepartmentDal _PermissDepartmentDal;
		public IPermissDepartmentDal PermissDepartmentDal
		{
			get
			{
				if (_PermissDepartmentDal == null)
				{
					_PermissDepartmentDal = DalFactory.GetPermissDepartmentDal();
				}
				return _PermissDepartmentDal;
			}
		}
	    private IPermissMenuDal _PermissMenuDal;
		public IPermissMenuDal PermissMenuDal
		{
			get
			{
				if (_PermissMenuDal == null)
				{
					_PermissMenuDal = DalFactory.GetPermissMenuDal();
				}
				return _PermissMenuDal;
			}
		}
	    private IPermissRefMenuButtonDal _PermissRefMenuButtonDal;
		public IPermissRefMenuButtonDal PermissRefMenuButtonDal
		{
			get
			{
				if (_PermissRefMenuButtonDal == null)
				{
					_PermissRefMenuButtonDal = DalFactory.GetPermissRefMenuButtonDal();
				}
				return _PermissRefMenuButtonDal;
			}
		}
	    private IPermissRefRoleMenuButtonDal _PermissRefRoleMenuButtonDal;
		public IPermissRefRoleMenuButtonDal PermissRefRoleMenuButtonDal
		{
			get
			{
				if (_PermissRefRoleMenuButtonDal == null)
				{
					_PermissRefRoleMenuButtonDal = DalFactory.GetPermissRefRoleMenuButtonDal();
				}
				return _PermissRefRoleMenuButtonDal;
			}
		}
	    private IPermissRefUserDepartmentDal _PermissRefUserDepartmentDal;
		public IPermissRefUserDepartmentDal PermissRefUserDepartmentDal
		{
			get
			{
				if (_PermissRefUserDepartmentDal == null)
				{
					_PermissRefUserDepartmentDal = DalFactory.GetPermissRefUserDepartmentDal();
				}
				return _PermissRefUserDepartmentDal;
			}
		}
	    private IPermissRefUserMenuButtonDal _PermissRefUserMenuButtonDal;
		public IPermissRefUserMenuButtonDal PermissRefUserMenuButtonDal
		{
			get
			{
				if (_PermissRefUserMenuButtonDal == null)
				{
					_PermissRefUserMenuButtonDal = DalFactory.GetPermissRefUserMenuButtonDal();
				}
				return _PermissRefUserMenuButtonDal;
			}
		}
	    private IPermissRefUserRoleDal _PermissRefUserRoleDal;
		public IPermissRefUserRoleDal PermissRefUserRoleDal
		{
			get
			{
				if (_PermissRefUserRoleDal == null)
				{
					_PermissRefUserRoleDal = DalFactory.GetPermissRefUserRoleDal();
				}
				return _PermissRefUserRoleDal;
			}
		}
	    private IPermissRoleDal _PermissRoleDal;
		public IPermissRoleDal PermissRoleDal
		{
			get
			{
				if (_PermissRoleDal == null)
				{
					_PermissRoleDal = DalFactory.GetPermissRoleDal();
				}
				return _PermissRoleDal;
			}
		}
	    private IPermissUserDetailsDal _PermissUserDetailsDal;
		public IPermissUserDetailsDal PermissUserDetailsDal
		{
			get
			{
				if (_PermissUserDetailsDal == null)
				{
					_PermissUserDetailsDal = DalFactory.GetPermissUserDetailsDal();
				}
				return _PermissUserDetailsDal;
			}
		}
	    private IPermissUserLoginDal _PermissUserLoginDal;
		public IPermissUserLoginDal PermissUserLoginDal
		{
			get
			{
				if (_PermissUserLoginDal == null)
				{
					_PermissUserLoginDal = DalFactory.GetPermissUserLoginDal();
				}
				return _PermissUserLoginDal;
			}
		}
	    private IStateBugDal _StateBugDal;
		public IStateBugDal StateBugDal
		{
			get
			{
				if (_StateBugDal == null)
				{
					_StateBugDal = DalFactory.GetStateBugDal();
				}
				return _StateBugDal;
			}
		}
	    private IStateLoginLogDal _StateLoginLogDal;
		public IStateLoginLogDal StateLoginLogDal
		{
			get
			{
				if (_StateLoginLogDal == null)
				{
					_StateLoginLogDal = DalFactory.GetStateLoginLogDal();
				}
				return _StateLoginLogDal;
			}
		}
	    private IStateOperateLogDal _StateOperateLogDal;
		public IStateOperateLogDal StateOperateLogDal
		{
			get
			{
				if (_StateOperateLogDal == null)
				{
					_StateOperateLogDal = DalFactory.GetStateOperateLogDal();
				}
				return _StateOperateLogDal;
			}
		}
        /// <summary>
        /// 保存ORM执行的操作
        /// </summary>
        /// <returns>受影响的行数</returns>
		public int SaveChanges()
		{
			//让当前的上下文进行提交(一次请求一个上下文)
			return DbContextFactory.GetCurrentDbContext().SaveChanges();
		}
    }
}