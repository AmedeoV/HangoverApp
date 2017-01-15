using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Models;

namespace HangoverApp.ViewModels
{
    public interface IDataStore
    {
        Task Init();
        Task<IEnumerable<Store>> GetStoresAsync();
        Task<Store> AddStoreAsync(Store store);
        Task<bool> RemoveStoreAsync(Store store);
        Task<Store> UpdateStoreAsync(Store store);
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetFeedbackAsync();
        Task<bool> RemoveFeedbackAsync(Feedback feedback);
        Task SyncStoresAsync();
        Task SyncFeedbacksAsync();
    }
}
