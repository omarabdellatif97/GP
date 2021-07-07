using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public interface ICaseRepo
    {
        Task<Case> GetAsync(int id);
        Task<IEnumerable<Case>> GetAllAsync();
        Task<IEnumerable<Case>> GetAllAsync(int page);
        //Task<IEnumerable<Case>> Search(string title, string[] tags);
        Task<IEnumerable<Case>> SearchAsync(SearchModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, Case mycase);
        Task<bool> InsertAsync(Case mycase);

    }
}
