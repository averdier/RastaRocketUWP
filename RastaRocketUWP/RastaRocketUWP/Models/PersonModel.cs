using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Models
{
    public class PersonModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class ContactContainer
    {
        public List<PersonModel> Contacts { get; set; }
    }

    public class ConsultantContainer
    {
        public List<PersonModel> Consultants { get; set; }
    }
}
