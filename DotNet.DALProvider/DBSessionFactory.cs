using System.Web;

namespace DotNet.DALProvider
{
    public class DBSessionFactory
    {
        /// <summary>
        /// 创建DBSession
        /// 从DBSession中调用DalFactory拿到各种Dal，然后进行增删改查操作。
        /// 封装SaveChanges方法，执行DBContext的统一提交
        /// </summary>
        /// <returns></returns>
        public static IDBSession GetCurrentDbSession()
        {
            IDBSession dbSession = HttpContext.Current.Items["DBSession"] as IDBSession;
            if (dbSession == null)
            {
                dbSession = new DBSession();
                HttpContext.Current.Items.Add("DBSession", dbSession);
            }

            return dbSession;
        }
    }
}