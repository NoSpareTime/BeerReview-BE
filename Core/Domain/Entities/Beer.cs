using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Beer
    {
        public int Id { get; set; }
        public int BreweryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Brewery Brewery { get; set; }
        public ICollection<Round> Rounds { get; set; }
    }
}
