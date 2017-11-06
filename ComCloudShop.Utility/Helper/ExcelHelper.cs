using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Utility.Helper
{
    public class ExcelHelper
    {
        //Excel数据转List<T>
        public static DataTable ReadExcelToEntityList<T>(string filePath,string pid) where T : class, new()
        {
            DataTable tbl = ReadExcelToDataTable(filePath);//读取Excel数据到DataTable

            //IList<T> list = DataTableToList<T>(tbl,pid);

            return tbl;

        }

        //Excel数据转DataTable 使用的oledb读取方式
        public static DataTable ReadExcelToDataTable(string filePath)
        {

            if (filePath == string.Empty) throw new ArgumentNullException("路径参数不能为空");
            string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Persist Security Info=False;Data Source=" + filePath + "; Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbDataAdapter adapter = new OleDbDataAdapter("select * From[Sheet1$]", ConnectionString); //默认读取的Sheet1,你也可以把它封装变量,动态读取你的Sheet工作表
            DataTable table = new DataTable("TempTable");
            adapter.Fill(table);
            return table;
        }

        
    }
}
