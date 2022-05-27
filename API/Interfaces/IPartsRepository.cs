namespace API.Interfaces
{
    public interface IPartsRepository
    {
        void AddPart(NewPartDto newPart);
        void RemovePart(Part Part);
        Task RemovePartByPartCode(string partCode);
        Task UpdatePart(Part part);
        Task<Part> GetPartById(int id);
        Task<Part> GetPartByPartCode(string partCode);
        Task<PagedList<PartDto>> GetParts(PaginationParams partParams, Func<PartDto, bool> predicate);
        Task<List<Part>> GetAllPartsAsList();
        Task<List<string>> GetAllPartCodes();
        Task<bool> Exists(string partCode);
    }
}