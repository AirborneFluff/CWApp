namespace API.Entities
{
    public class PartsListEntry
    {
        public int Id { get; set; }
        
        public int PartId { get; set; }
        public int PartsListId { get; set; }
        public float Quantity { get; set; }
        public string ComponentLocation { get; set; }

    }
}