using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using DotNet.Common;
using DotNet.Entity.Enum;
using DotNet.IBLL;
using DotNet.IDAL;
using System.Web;
using DotNet.Entity;

namespace DotNet.BLL
{
    public partial class PermissUserLoginBll : IPermissUserLoginBll
    {
        /// <summary>
        /// 添加/注册用户
        /// </summary>
        /// <param name="entity">用户Model</param>
        /// <returns></returns>
        public override int Add(PermissUserLogin entity)
        {
            //密码加盐加密
            entity.LoginPwd = DESEncryptHelper.GetStringMD5(entity.LoginId + entity.LoginPwd);
            return base.Add(entity);
        }
        /// <summary>
        /// 逻辑删除一条记录
        /// DeleteFlag标记为Delted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ORMLogicDelete(string id)
        {
            if (UpDateDeleteFlage(id))
            {
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 逻辑删除多条记录
        /// DeleteFlag标记为Deleted
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public bool ORMLogicDelete(string[] idList)
        {
            for (int i = 0; i < idList.Length; i++)
            {
                if (!UpDateDeleteFlage(idList[i]))
                {
                    return false;
                }
            }
            return DbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ORMPhysicallyDelete(string id)
        {
            //拿到dbSession的DetailDal
            DbSet<PermissUserDetails> userDetailsContext = DbContext.Set<PermissUserDetails>(); //DbSession.PermissUserDetailsDal;

            Guid guidId = new Guid(id);
            //如果有这条记录，删除UserLogin表的记录
            var userLoginModel = LoadEntities(u => u.Id == guidId).FirstOrDefault();
            if (userLoginModel != null)
            {
                Delete(userLoginModel);
                //如果有对应的UserDetails，删除UserDetails表的记录
                var userDetailModel = userDetailsContext.FirstOrDefault(u => u.UserId == guidId);
                if (userDetailModel != null)
                {
                    userDetailsContext.Remove(userDetailModel);
                }
                //如果数据库删除成功了，把磁盘上的用户头像删除了
                if (DbContext.SaveChanges() > 0)
                {
                    //如果不是默认头像,把头像文件也删了
                    if (userLoginModel.PhotoPath != "default.png")
                    {
                        //todo 删除另外一台服务器上的文件
                        //string rootDir = @"~/Content/images/UserHeadPhoto/";
                        //string dir = System.Web.HttpContext.Current.Server.MapPath(rootDir);
                        //string filePath = dir + userLoginModel.PhotoPath;
                        //if (File.Exists(filePath))//有文件
                        //{
                        //    File.Delete(filePath);
                        //}
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ORMPassCheck(string id)
        {
            if (UpDatePassCheck(id))
            {
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 批量通过审核
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public bool ORMPassCheck(string[] idList)
        {

            for (int i = 0; i < idList.Length; i++)
            {
                if (!UpDatePassCheck(idList[i]))
                {
                    return false;
                }
            }
            return DbContext.SaveChanges() > 0;
        }



        /// <summary>
        /// 查到这条记录，将DeleteFlag的值修改
        /// </summary>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        private bool UpDateDeleteFlage(string primaryKeyValue)
        {
            Guid id;
            if (Guid.TryParse(primaryKeyValue, out id))
            {
                var model = GetModel(id);
                if (model != null)
                {
                    model.DeleteFlag = DelFlagEnum.Deleted;
                    DbContext.Entry(model).State = EntityState.Modified;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool UpDatePassCheck(string primaryKeyValue)
        {
            Guid id;
            if (Guid.TryParse(primaryKeyValue, out id))
            {
                var model = GetModel(id);
                if (model != null)
                {
                    model.ApplyDate = DateTime.Now;
                    model.IsAble = true;
                    DbContext.Entry(model).State = EntityState.Modified;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}