namespace DotNet.IBLL
{
    public partial interface IPermissUserLoginBll
    {
        /// <summary>
        /// 逻辑删除一条记录
        /// DeleteFlag标记为Delted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ORMLogicDelete(string id);
       
        /// <summary>
        /// 逻辑删除多条记录
        /// DeleteFlag标记为Deleted
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        bool ORMLogicDelete(string[] idList);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ORMPhysicallyDelete(string id);
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ORMPassCheck(string id);
        /// <summary>
        /// 批量通过审核
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        bool ORMPassCheck(string[] idList);
    }
}