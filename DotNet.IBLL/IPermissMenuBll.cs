namespace DotNet.IBLL
{
    public partial interface IPermissMenuBll
    {
        /// <summary>
        /// 删除数据库中该菜单所有的相关信息
        /// Menu MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        bool DeleteMenuAtAnyWhere(string id);
        /// <summary>
        /// 删除数据库中该菜单所有的相关信息
        /// Menu MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="idArr">主键数组</param>
        /// <returns></returns>
        bool DeleteMenuAtAnyWhere(string[] idArr);
    }
}