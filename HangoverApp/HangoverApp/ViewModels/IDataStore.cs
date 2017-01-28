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
        Task<IEnumerable<OpenRestaurant>> GetStoresAsync();
        Task<OpenRestaurant> AddStoreAsync(OpenRestaurant openRestaurant);
        Task<bool> RemoveStoreAsync(OpenRestaurant openRestaurant);
        Task<OpenRestaurant> UpdateStoreAsync(OpenRestaurant openRestaurant);
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetFeedbackAsync();
        Task<bool> RemoveFeedbackAsync(Feedback feedback);
        Task SyncStoresAsync();
        Task SyncFeedbacksAsync();
    }
}
