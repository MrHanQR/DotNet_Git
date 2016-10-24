using System;
using System.IO;
using System.Linq;
using DotNet.Common;
using DotNet.Entity.Enum;
using DotNet.IBLL;
using DotNet.IDAL;
using System.Web;

namespace DotNet.BLL
{
    public partial class PermissUserLoginBll:IPermissUserLoginBll
    {
        public IPermissUserDetailsDal UserDetailsDal { get; set; }
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
                return DbSession.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
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
            return DbSession.SaveChanges() > 0;
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ORMPhysicallyDelete(string id)
        {
            //拿到dbSession的DetailDal
            UserDetailsDal = DbSession.PermissUserDetailsDal;
            Guid guidId=new Guid(id);
            //如果有这条记录，删除UserLogin表的记录
            var userLoginModel = CurrentDal.ORMLoadEntities(u => u.Id == guidId).FirstOrDefault();
            if (userLoginModel!=null)
            {
                CurrentDal.ORMDelete(userLoginModel);
                //如果有对应的UserDetails，删除UserDetails表的记录
                var userDetailModel = UserDetailsDal.ORMLoadEntities(u => u.UserId == guidId).FirstOrDefault();
                if (userDetailModel!=null)
                {
                    UserDetailsDal.ORMDelete(userDetailModel);
                }
                if (DbSession.SaveChanges()>0)
                {
                    //如果不是默认头像,把头像文件也删了
                    if (userLoginModel.PhotoPath != "default.png")
                    {
                        string rootDir = @"~/Content/images/UserHeadPhoto/";
                        string dir = System.Web.HttpContext.Current.Server.MapPath(rootDir);
                        string filePath = dir + userLoginModel.PhotoPath;
                        if (File.Exists(filePath))//有文件
                        {
                            File.Delete(filePath);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
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
                return DbSession.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
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
            return DbSession.SaveChanges() > 0;
        }



        /// <summary>
        /// 查到这条记录，将DeleteFlag的值修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UpDateDeleteFlage(string id)
        {
            var model = CurrentDal.ORMLoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
            if (model != null)
            {
                model.DeleteFlag = DelFlagEnum.Deleted;
                CurrentDal.ORMUpdate(model);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool UpDatePassCheck(string id)
        {
            var model = CurrentDal.ORMLoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
            if (model != null)
            {
                model.ApplyDate = DateTime.Now;
                model.IsAble = true;
                CurrentDal.ORMUpdate(model);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}