using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNet.Entity;

namespace DotNet.IBLL
{
    public partial interface IPermissRefMenuButtonBll
    {
        /// <summary>
        /// 修改菜单的按钮，删除原来的，添加最新的
        /// </summary>
        /// <param name="delButtonIdList">新的按钮Id集合</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        bool ORMChangeButton( string[] delButtonIdList, string menuId);
    }
}