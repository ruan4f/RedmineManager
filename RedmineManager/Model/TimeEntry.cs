using System;
using System.Collections.Generic;
using System.Text;

namespace RedmineManager.Model
{
    public class TimeEntry
    {
        public int issue_id { get; set; }
        public string spent_on { get; set; }
        public double hours { get; set; }
        public string comments { get; set; }
        public int activity_id { get; set; }
    }
}
