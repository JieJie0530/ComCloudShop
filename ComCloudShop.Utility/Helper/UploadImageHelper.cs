using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ComCloudShop.Utility.Helper
{
    public class UploadImageHelper
    {

        public enum OperateResult
        {
            success = 1,
            fail = 2
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public static WebResult WebImgUpload()
        {
            string dirTempPath = HttpContext.Current.Server.MapPath("~/File/");
            string MinImagePath = HttpContext.Current.Server.MapPath("~/MinImage/");
            if (!Directory.Exists(dirTempPath))
            {
                Directory.CreateDirectory(dirTempPath);
            }
            if (!Directory.Exists(MinImagePath))
            {
                Directory.CreateDirectory(MinImagePath);
            }
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return new WebResult() { retCode = 0, retShowMsg = "请选择上传图片" };
            }
            HttpPostedFile file = HttpContext.Current.Request.Files[0];

            int code = (int)OperateResult.success;
            string msg = "上传成功";
            string fileExt = ".jpg";
            String newFileName = "";
            String ymd = "";
            //最大文件大小
            int maxSize = 10000000;
            if (file.ContentLength <= 0)
            {
                code = (int)OperateResult.fail;
                msg = "请选择上传文件。";
            }
            else if (file.ContentLength > maxSize)
            {
                code = (int)OperateResult.fail;
                msg = " 上传文件大小超过限制";
            }
            else
            {
                fileExt = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                //定义允许上传的文件扩展名
                String fileTypes = "gif,jpg,jpeg,png,bmp";
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    code = (int)OperateResult.fail;
                    msg = " 上传文件扩展名是不允许的扩展名";
                }
                else
                {
                    ymd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (!Directory.Exists(dirTempPath + ymd))
                    {
                        Directory.CreateDirectory(dirTempPath + ymd);
                    }
                    if (!Directory.Exists(MinImagePath + ymd))
                    {
                        Directory.CreateDirectory(MinImagePath + ymd);
                    }
                    newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    file.SaveAs(Path.Combine(dirTempPath + ymd + "\\", newFileName + fileExt));
                    //fileinfo.CopyTo(Path.Combine(dirTempPath + ymd + "\\", newFileName + fileExt), true);
                    //fileinfo.Delete();
                    // UpLoadImageApi(file);
                }
            }
            //ImageRectangle img = new ImageRectangle() { ThumbnailType = SysEnum.ThumbnailType.AutoFixed };

            //string result = SetMinImage(dirTempPath + ymd + "/" + newFileName + fileExt, MinImagePath + ymd + "/" + newFileName + fileExt, img);

            //if (result != "1" && result != "2")
            //{
            //    return new WebResult() { retCode = code, retShowMsg = msg, DataResult = "/MinImage/" + ymd + "/" + newFileName + fileExt };
            //}
            return new WebResult() { retCode = code, retShowMsg = msg, DataResult = "/File/" + ymd + "/" + newFileName + fileExt };
        }


        public class WebResult {
            public int retCode { get; set; }
            public string retShowMsg { get; set; }

            public string DataResult { get; set; }
        }
        
    }
}
