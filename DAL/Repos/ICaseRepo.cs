using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public interface ICaseRepo
    {
        Task<Case> Get(int id);
        Task<IEnumerable<Case>> GetAll();
        Task<IEnumerable<Case>> GetAll(int page);
        Task<IEnumerable<Case>> Search(string title, string[] tags);
        Task<IEnumerable<Case>> Search(SearchModel model);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, Case mycase);
        Task<bool> Insert(Case mycase);

    }
}
