using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class OrderCommentViewModel
    {
        public OrderCommentViewModel() {

        }
        public OrderCommentViewModel(int member, string orderID) {
            this.MemberID = member;
            this.OrderNum = orderID;
        }

        public int ID { get; set; }
        
        public int MemberID { get; set; }
        [Display(Name = "会员名称")]
        public string UserName { get; set; }
        public string OrderNum { get; set; }
        public int ProductID { get; set; }

        [Display(Name = "评论内容")]
        public string Contents { get; set; }
        public string Pics { get; set; }
        [Display(Name = "产品评分")]
        public int ProductScore { get; set; }
        public int ProductPackaging { get; set; }
        public int DeliverySpeed { get; set; }
        public DateTime AddTime { get; set; }
        public string Remack { get; set; }
    }
}
