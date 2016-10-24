using System;
using System.Collections.Generic;

namespace DotNet.IBLL
{
    public partial interface IPermissRefUserDepartmentBll
    {
        bool ORMSetUserDep(Guid depId, string[] userIdArr);
    }
}