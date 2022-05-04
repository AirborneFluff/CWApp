using API.DTOs.PartsListDTOs;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Interfaces
{
    public interface IPartsListsRepository
    {
        void AddNewPartsList(PartsList newPartsList);
        void RemovePartsList(PartsList partsList);
        Task<List<PartsList>> GetAllPartsLists();
        Task<PartsList> GetPartsListFromId(int partsListId);
        Task<PartsList> GetPartsListFromTitle(string title);
    }
}