namespace API.Interfaces
{
    public interface ISourcePriceRepository
    {

        void RemoveSourcePrice(SourcePrice price);
        Task<SourcePrice> GetSourcePriceById(int id);
    }
}