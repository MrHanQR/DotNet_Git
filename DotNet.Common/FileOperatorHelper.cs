using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace DotNet.Common
{
    public class FileOperatorHelper
    {
        public static string SaveImages(HttpPostedFileBase file, out string errorMsg,int width,int height)
        {
            string fileName = string.Empty;
            if (file.ContentLength>0)
            {
                //获取扩展名
                string extName = Path.GetExtension(file.FileName);
                if (extName == ".jpg" || extName == ".png" || extName == ".jpeg" || extName == ".gif" || extName == ".bmp")
                {
                    //文件名重命名 防止重名，同样的文件可以用一份就可以了，所以用MD5
                    string md5FileName = DESEncryptHelper.GetStreamMd5(file.InputStream);
                    //根据上传的文件的日期，创建文件夹
                    //string dir = "~/Content/images/UserHeadPhoto/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                    string dir = "~/Content/images/UserHeadPhoto/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    Directory.CreateDirectory(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath(dir)));//创建文件夹
                    //构建一个完整的文件存放路径
                    //string fullDir = dir + md5FileName + extName;
                    //保存缩略图,返回保存的路径
                    string fullDir=ImageHelper.CreateThumbnail(file, dir, extName, width, height, md5FileName,
                        ImageHelper.ThumbModel.NoDeformationAllThumb);
                    //保存文件
                    //file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(fullDir));
                    fileName = fullDir.Substring(31,fullDir.Length-31);//2016/5/8/165C9DCC948C38B305578011F06852FC.jpg
                    errorMsg = string.Empty;
                }
                else
                {
                    errorMsg = "请选择正确的格式";
                }
            }
            else
            {
                fileName = "default.png";
                errorMsg = string.Empty;
            }
            return fileName;
        }

        public static void FileUpload()
        {

        }
    }
}