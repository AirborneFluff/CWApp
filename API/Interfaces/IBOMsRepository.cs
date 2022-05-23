using API.DTOs.BOMDTOs;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Interfaces
{
    public interface IBOMsRepository
    {
        void AddNewBOM(BOM newBOM);
        void RemoveBOM(BOM BOM);
        Task<List<BOM>> GetBOMs(Product product);
        Task<List<BOM>> GetAllBOMs();
        Task<BOM> GetBOM(int bomId);
        Task<BOM> GetBOMFromTitle(string title);
    }
}