namespace DotNet.IBLL
{
    public partial interface IPermissRoleBll
    {
        /// <summary>
        /// 删除数据库中该角色所有的相关信息
        /// 删除Role UserRole RoleMenuButton
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        bool DeleteButonAtAnyWhere(string id);
        /// <summary>
        /// 删除数据库中该角色所有的相关信息
        /// 删除Role UserRole RoleMenuButton
        /// </summary>
        /// <param name="idArr">主键数组</param>
        /// <returns></returns>
        bool DeleteButonAtAnyWhere(string[] idArr);
    }
}