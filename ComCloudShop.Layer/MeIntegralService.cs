using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;
using System.IO;

namespace ComCloudShop.Layer
{
    public class MeIntegralService:BaseService
    {
        public void  Update(MeIntegralViewModel model) {
            try
            {
                var m = db.MeIntegrals.FirstOrDefault(x => x.OpenId == model.OpenId);
                if (m != null)
                {

                    try
                    {
                        m.JiFen = model.JiFen;
                        db.SaveChanges();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    m = new MeIntegral();
                    m.OpenId = model.OpenId;
                    m.NickName = model.NickName;
                    m.JiFen = model.JiFen;
                    db.MeIntegrals.Add(m);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                WriteTokenToTxt(ex.ToString() + "," + model.OpenId + "," + model.NickName + "," + model.JiFen);
            }

            //var  m = new MeIntegral();
            // m.OpenId = model.OpenId;
            // m.NickName = model.NickName;
            // m.JiFen = model.JiFen;
            // db.MeIntegrals.Add(m);
            // db.SaveChanges();
        }

        public void WriteTokenToTxt(string token)
        {
            try
            {
                FileStream fs = new FileStream(@"D://1.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write("");
                sw.Write(token + "\r\n");
                sw.Write(DateTime.Now.ToString());
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }


    }
}
