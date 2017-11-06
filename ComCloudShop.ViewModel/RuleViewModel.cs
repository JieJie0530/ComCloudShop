using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class RuleViewModel
    {
        public int Id { get; set; }

        public string Rule { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }
    }


    public class AdminRuleViewModel
    {
        public int RuleId { get; set; }
        /// <summary>
        /// 满减条件
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 满减金额
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Content { get; set; }


    }
}
