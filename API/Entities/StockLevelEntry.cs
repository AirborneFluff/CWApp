namespace API.Entities
{
    public class StockLevelEntry
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public float RemainingStock {
            get
            {
                return _remainingStock;
            }
            set
            {
                if (value < 0) _remainingStock = 0;
                else _remainingStock = value;
            }
        }
        public string StockUnits { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    private float _remainingStock;

}
}