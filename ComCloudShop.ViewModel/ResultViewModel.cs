using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class ResultViewModel<T>
    {
        public int error { get; set; }
        public string msg { get; set; }
        public int total { get; set; }
        public T result { get; set; }
    }


    public enum ErrorEnum
    {
        OK = 100,
        Error = -100,
        Fail = -200
    }
}
