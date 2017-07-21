using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace activitytool
{
    public class atcDate
    {
        public string ver { get; set; }
        public List<actinfo> Date { get; set; }

    }
    public class actinfo
    {
        public string actname { get; set; }
        public int actid { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public string giftname { get; set; }
        public int model { get; set; }
        public List<actinfo> atcExt { get; set; }

    }
    //public class atcExt
    //{
    //    public string atcname { get; set; }
    //    public int actid { get; set; }
    //    public int start_time { get; set; }
    //    public int end_time { get; set; }
    //    public string Host { get; set; }
    //    public string Referer { get; set; }
    //    public string giftname { get; set; }
    //}

}
