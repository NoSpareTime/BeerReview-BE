namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int BeerId { get; set; }
        public long CreatedAd { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public Round Round { get; set; }
        public Beer Beer { get; set; }
    }
}