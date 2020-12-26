using System.Collections.Generic;

namespace Domain.Entities
{
    public class Round
    {
        public int Id { get; set; }
        public long StartsAt { get; set; }
        public long EndsAt { get; set; }

        public ICollection<Beer> Beers { get; set; }
    }
}