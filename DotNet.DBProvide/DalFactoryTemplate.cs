  
  

using DotNet.IDAL;
using DotNet.Common;
using System.Reflection;
/**        数据访问层工厂类-自动生成
 * 项目名：DotNet      程序集名：DotNet.DBProvide
 * 制作人：韩庆瑞      生成时间：11/02/2016 22:49:09
 * 生成工具：T4模板
 * 描述：
**/
namespace DotNet.DBProvide
{
    public partial class DalFactory
	{
		 //抽象工厂
        private static readonly string AssemblyName ;

        static DalFactory()
        {
            AssemblyName =  ConfigHelper.DalAssemblyName;
        } 
   
	    public static IPermissRefUserDepartmentDal GetPermissRefUserDepartmentDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRefUserDepartmentDal", true);
            return obj as IPermissRefUserDepartmentDal;
            //没用缓存
        } 
	    public static IPermissRefUserMenuButtonDal GetPermissRefUserMenuButtonDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRefUserMenuButtonDal", true);
            return obj as IPermissRefUserMenuButtonDal;
            //没用缓存
        } 
	    public static IPermissRefUserRoleDal GetPermissRefUserRoleDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRefUserRoleDal", true);
            return obj as IPermissRefUserRoleDal;
            //没用缓存
        } 
	    public static IPermissButtonDal GetPermissButtonDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissButtonDal", true);
            return obj as IPermissButtonDal;
            //没用缓存
        } 
	    public static IStateBugDal GetStateBugDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".StateBugDal", true);
            return obj as IStateBugDal;
            //没用缓存
        } 
	    public static IStateLoginLogDal GetStateLoginLogDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".StateLoginLogDal", true);
            return obj as IStateLoginLogDal;
            //没用缓存
        } 
	    public static IPermissDepartmentDal GetPermissDepartmentDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissDepartmentDal", true);
            return obj as IPermissDepartmentDal;
            //没用缓存
        } 
	    public static IPermissMenuDal GetPermissMenuDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissMenuDal", true);
            return obj as IPermissMenuDal;
            //没用缓存
        } 
	    public static IPermissRoleDal GetPermissRoleDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRoleDal", true);
            return obj as IPermissRoleDal;
            //没用缓存
        } 
	    public static IStateOperateLogDal GetStateOperateLogDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".StateOperateLogDal", true);
            return obj as IStateOperateLogDal;
            //没用缓存
        } 
	    public static IPermissUserDetailsDal GetPermissUserDetailsDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissUserDetailsDal", true);
            return obj as IPermissUserDetailsDal;
            //没用缓存
        } 
	    public static IPermissRefRoleMenuButtonDal GetPermissRefRoleMenuButtonDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRefRoleMenuButtonDal", true);
            return obj as IPermissRefRoleMenuButtonDal;
            //没用缓存
        } 
	    public static IPermissRefMenuButtonDal GetPermissRefMenuButtonDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissRefMenuButtonDal", true);
            return obj as IPermissRefMenuButtonDal;
            //没用缓存
        } 
	    public static IPermissUserLoginDal GetPermissUserLoginDal()
        {
            //抽象工厂模式——利用反射
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PermissUserLoginDal", true);
            return obj as IPermissUserLoginDal;
            //没用缓存
        } 
}
}
