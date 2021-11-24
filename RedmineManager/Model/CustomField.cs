using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedmineManager.Model
{
    public class CustomField
    {
        [JsonProperty(PropertyName = "37")]
        public string Numero { get; set; }
    }
}
