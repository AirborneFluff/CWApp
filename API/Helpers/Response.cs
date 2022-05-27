namespace API.Helpers
{
    public class Response<T>
    {
        public Response(IEnumerable<T> results = null)
        {
            Results = results ?? Array.Empty<T>();
        }
    
    public IEnumerable<T> Results { get; set; }
    }
}