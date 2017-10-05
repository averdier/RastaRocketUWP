using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Models
{
    public class NeedModel
    {
        public String Id { get; set; }
        public String Title { get; set; }

        public String Created_At { get; set; }

        public String Customer { get; set; }

        public CustomerModel Customer_Obj { get; set; }

        public PersonModel Contact_Obj { get; set; }

        public String Contact { get; set; }

        public String Status { get; set; }

        public float Month_Duration { get; set; }

        public float Rate { get; set; }

        public float Week_Frequency { get; set; }

        public List<string> Success_Key { get; set; }

        public string Description { get; set; }

        public List<PersonModel> Consultants_Obj { get; set; }
    }

    public class NeedContainer
    {
        public List<NeedModel> Needs { get; set; }
    }
}
