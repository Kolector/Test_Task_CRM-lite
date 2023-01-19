using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Task_CRM_lite.Models
{
    public class Tasks
    {
        public int id { get; set; }
        public string customer_name { get; set; }
        public string implementer_name { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string deadline { get; set; }

    }
}
