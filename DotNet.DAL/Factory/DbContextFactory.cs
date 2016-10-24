using System.Data.Entity;
using System.Web;
using DotNet.Entity;

namespace DotNet.DAL.Factory
{
    public class DbContextFactory
    {
        /// <summary>
        /// 创建DbContext，对EF的DbContext对象DotNetModelContainer进行封装
        /// </summary>
        /// <returns></returns>
        public static DbContext GetCurrentDbContext()
        {
            //线程内实例唯一
            DotNetEntities db = HttpContext.Current.Items["DbContext"] as DotNetEntities;
            if (db == null)
            {
                db = new DotNetEntities();
                //db.InitDataBase();
                HttpContext.Current.Items.Add("DbContext", db);
            }
            return db;
        }
    }
}