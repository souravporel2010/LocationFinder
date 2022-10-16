using LocationFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationFinder.Domain.Others
{
    public class LocationRepositoryResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public Location? location { get; set; }
    }
}
