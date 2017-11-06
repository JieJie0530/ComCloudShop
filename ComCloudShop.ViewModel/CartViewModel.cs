using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class CartDisplayModel
    {
        public int CartId { get; set; }

        public int ProductNum { get; set; }

        public int MemberId { get; set; }

        public int ProductId { get; set; }

        public string MemberName { get; set; }

        public string SPMC { get; set; }

        public string SPGG { get; set; }

        public string SPDM { get; set; }

        public string ProductArrs { get; set; }

        public string BuyNums { get; set; }

        public string SaleArrs { get; set; }

        public string ProductGuid { get; set; }

        public decimal Sale { get; set; }

        public string Title { get; set; }

        public bool Selected { get; set; }

        public decimal DiscountSale { get; set; }

        public string P1 { get; set; }

        public string P2 { get; set; }
       
        public string P3 { get; set; }

        public string SubTitle { get; set; }

        public string Describle { get; set; }

        public decimal Total { get; set; }

        public decimal AllSum { get; set; }
    }

    public class CartViewModel 
    {
        public int MemberId { get; set; }

        public ICollection<CartProductViewModel> CartProducts { get; set; }
    }

    public class CartProductViewModel 
    {
        public int ProductId { get; set; }

        public int BuyNum { get; set; }
    }


    public class CartListForOrderViewModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string SPMC { get; set; }
        public string SPGG { get; set; }
        public int ProductNum { get; set; }
        public decimal Sale { get; set; }
        public decimal Discount { get; set; }
        public string Pic { get; set; }
    }

    /// <summary>
    /// For提交订单Model
    /// </summary>
    public class CartForAddOrderModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int ProductNum { get; set; }
        public decimal Sale { get; set; }
        public decimal Discount { get; set; }
    }

}
