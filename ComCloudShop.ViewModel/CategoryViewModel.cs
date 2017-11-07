using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string BrandPic { get; set; }
        public int CategoryType { get; set; }

        public int ParentId { get; set; }
    }

    public class CategoryRelationViewModel
    {
        public int CategoryRelationId { get; set; }
        
        public int CategoryId { get; set; }
        
        public int ProductId { get; set; }

        public int CategoryType { get; set; }

        public string CategoryArrs { get; set; }
    }

    public class CategoryDM
    {
        public int dm { get; set; }
        public string mc { get; set; }
    }
}
