namespace DotNet.IBLL
{
    public partial interface IPermissButtonBll
    {
        /// <summary>
        /// 删除数据库中该按钮所有的相关信息
        /// Button MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        bool DeleteButonAtAnyWhere(string id);
             /// <summary>
        /// 删除数据库中该按钮所有的相关信息
        /// Button MenuButton RoleMenuButton UserMenuButton
        /// </summary>
        /// <param name="idArr">主键数组</param>
        /// <returns></returns>
        bool DeleteButonAtAnyWhere(string[] idArr);
    }
}