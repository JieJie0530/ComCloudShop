using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace ComCloudShop.Layer
{
    public class SetSupService
    {
        public SetSupViewModel Read()
        {
           return DeSearializa();
        }

        public SetSupViewModel Read1()
        {
            return DeSearializa1();
        }

        public bool Write(string bl) {
            try
            {
                SetSupViewModel model = new SetSupViewModel();
                model.SetBL = bl;
                Serialize(model);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        void Serialize(SetSupViewModel model)
        {
            BinaryFormatter bf = new BinaryFormatter();
            System.IO.FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/Images/mesite.dat"), FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, model);
            fs.Close();
        }
        SetSupViewModel DeSearializa()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/Images/mesite.dat"), FileMode.Open, FileAccess.Read);
                SetSupViewModel model = (SetSupViewModel)bf.Deserialize(fs);
                fs.Close();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        SetSupViewModel DeSearializa1()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/Admin/Images/mesite.dat"), FileMode.Open, FileAccess.Read);
                SetSupViewModel model = (SetSupViewModel)bf.Deserialize(fs);
                fs.Close();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
