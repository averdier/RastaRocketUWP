using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Models
{
    public class NeedPostModel
    {
        public String title { get; set; }
        public String customer { get; set; }
        public String contact { get; set; }
        public String description { get; set; }
        public String start_at_latest { get; set; }
        public String status { get; set; }
        public double week_frequency { get; set; } = 0;
        public double month_duration { get; set; } = 0;
        public List<String> success_keys { get; set; } = new List<string>();
        public List<String> consultants { get; set; } = new List<string>();

        public double rate { get; set; }

    }
}
