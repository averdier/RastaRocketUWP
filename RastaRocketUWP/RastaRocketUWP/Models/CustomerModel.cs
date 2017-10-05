using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class CustomerContainer
    {
        public List<CustomerModel> Customers { get; set; }
    }
}
