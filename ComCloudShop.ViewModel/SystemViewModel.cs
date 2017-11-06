using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class HomeImageViewModel
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Useful { get; set; }

        public string Link { get; set; }
    }

    /// <summary>
    /// 活动列表Model
    /// </summary>
    public class BatchListViewModel
    {
        public int PicId { get; set; }
        public string Image { get; set; }
        public string Useful { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public int Sort { get; set; }
    }

}
