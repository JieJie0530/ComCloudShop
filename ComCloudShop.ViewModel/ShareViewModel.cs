﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class ShareViewModel
    {
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }

        public string Url { get; set; }

        public string MemberID { get; set; }
    }
}
