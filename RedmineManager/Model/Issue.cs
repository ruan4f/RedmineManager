using System;
using System.Collections.Generic;
using System.Text;

namespace RedmineManager.Model
{
    public class Issue
    {
        public int project_id { get; set; }
        public int tracker_id { get; set; }
        public string subject { get; set; }
        public int status_id { get; set; }
        public int priority_id { get; set; }
        public int assigned_to_id { get; set; }
        public string start_date { get; set; }
        public int estimated_hours { get; set; }
        public int done_ratio { get; set; }
        public CustomField custom_field_values { get; set; }

        public Issue()
        {

        }
    }
}
