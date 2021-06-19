using DAL.Models;
using GP_API.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public class CaseService : ICaseRepo
    {
        private readonly CaseContext DB;
        public CaseService(CaseContext _DB)
        {
            this.DB = _DB;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                DB.Cases.Remove(await DB.Cases.FindAsync(id));
                return (await DB.SaveChangesAsync()) > 0;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<Case> Get(int id)
        {
            try
            {
                return await DB.Cases.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IEnumerable<Case>> GetAll()
        {
            try
            {
                return await DB.Cases.Include((c) => c.CaseFiles).Include((c) => c.Steps).Include((c) => c.Tags).Include((c) => c.Applications).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Case>> GetAll(int page)
        {
            try
            {
                return await DB.Cases.Skip((page - 1) * 25).Include((c) => c.CaseFiles).Include((c) => c.Applications).Include((c) => c.Steps).Include((c) => c.Tags).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Insert(Case mycase)
        {
            try
            {
                if (await DB.AddAsync(mycase) != null)
                {
                    await DB.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool CheckName(string name , string searchName)
        {
            if (searchName == null || name.Equals(searchName)) return true;
            return false;
        }
        private bool CheckApplication(ICollection<Application> applications, string searchApplication)
        {
            if (searchApplication == null || applications.Select(a=> a.Name ).Contains(searchApplication)) return true;
            return false;
        }
        private bool CheckTags(ICollection<Tag> Tags, ICollection<string> searchTags)
        {
            if (searchTags == null || Tags.Select(t => t.Name).Any(n => searchTags.Contains(n))) return true;
            return false;
        }
        public async Task<IEnumerable<Case>> Search(SearchModel SearchFilter)
        {
            try
            {
                return await DB.Cases.Include((c) => c.Applications).Where((C) => CheckName(C.Title, SearchFilter.Name) && CheckApplication(C.Applications, SearchFilter.Application) && CheckTags(C.Tags, SearchFilter.Tags)).Include(c => c.Steps).Include(c => c.Tags).Include(c => c.CaseFiles).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(int id, Case mycase)
        {
            try
            {
                Case c = await DB.Cases.FindAsync(id);
                c.Steps.Clear();
                c.Steps.Union(mycase.Steps);
                c.Tags.Clear();
                c.Tags.Union(mycase.Tags);
                c.CaseFiles.Clear();
                c.CaseFiles.Union(mycase.CaseFiles);
                c.Applications.Clear();
                c.Applications.Union(mycase.Applications);
                c.Title = mycase.Title;
                c.Description = mycase.Description;
                await DB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<Case>> Search(string title, string[] tags)
        {
        //    var result = DB.Cases.Where(c =>
        //        c.Title == title &&
        //        c.Tags.Any(t => tags.Any(tg => tg == t.Name))).ToList();
            throw new NotImplementedException();
        }
    }
}
