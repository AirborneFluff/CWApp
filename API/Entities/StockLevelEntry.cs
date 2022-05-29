namespace API.Entities
{
    public class StockLevelEntry
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public float RemainingStock { get; set; }
        public string StockUnits { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}