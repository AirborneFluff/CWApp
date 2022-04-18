namespace API.Extensions
{
    public static class CollectionExtensions
    {
        public static bool ContainsWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            var result = collection.FirstOrDefault(predicate);
            if (result == null) return false;
            return true;
        }
    }
}