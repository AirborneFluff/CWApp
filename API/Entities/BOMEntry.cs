namespace API.Entities
{
    public class BOMEntry
    {
        public int PartId { get; set; }
        public int BOMId { get; set; }
        public Part Part { get; set; }

        public float Quantity { get; set; }
        public string ComponentLocation { get; set; }

    }
}