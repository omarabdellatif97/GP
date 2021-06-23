using DAL.Models;
using Detached.Mappers.EntityFramework;
using GP_API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public class CaseService : ICaseRepo
    {
        private readonly CaseContext DB;
        public CaseService(CaseContext _DB)
        {
            DB = _DB;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                DB.Cases.Remove(await DB.Cases.FindAsync(id));
                await DB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<Case> Get(int id)
        {
            try
            {
                return await DB.Cases.Include(c => c.Tags)
                    .Include(c => c.Steps)
                    .Include(c => c.CaseFiles)
                    .Include(c => c.Applications)
                    .FirstOrDefaultAsync(c => c.Id == id);
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
                //foreach (var app in mycase.Applications)
                //{
                //    DB.Attach(app);
                //}
                DB.Update(mycase);
                await DB.SaveChangesAsync();
                return true;
                //if (await DB.AddAsync(mycase) != null)
                //{
                //    await DB.SaveChangesAsync();
                //    return true;
                //}
                //else
                //{
                //    return false;

                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool CheckName(string name, string searchName)
        {
            if (searchName == null || name.Equals(searchName)) return true;
            return false;
        }
        private bool CheckApplication(ICollection<Application> applications, string searchApplication)
        {
            if (searchApplication == null || applications.Select(a => a.Name).Contains(searchApplication)) return true;
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
                Case c = await DB.Cases.Include(c => c.Tags)
                    .Include(c => c.Steps)
                    .Include(c => c.CaseFiles)
                    .Include(c => c.Applications)
                    .FirstOrDefaultAsync(c => c.Id == id);

                DB.Entry(c).CurrentValues.SetValues(mycase);
                DB.TrackChildChanges(mycase.Tags, c.Tags, (i1, i2) => i1.Id == i2.Id && i1.Id != default && i2.Id != default);
                DB.TrackChildChanges(mycase.Steps, c.Steps, (i1, i2) => i1.Id == i2.Id && i1.Id != default && i2.Id != default);
                DB.TrackChildChanges(mycase.CaseFiles, c.CaseFiles, (i1, i2) => i1.Id == i2.Id && i1.Id != default && i2.Id != default);
                DB.TrackChildChanges(mycase.Applications, c.Applications, (i1, i2) => i1.Id == i2.Id && i1.Id != default && i2.Id != default);
                await DB.SaveChangesAsync();


                //Case c = await DB.Cases.Include(c => c.Tags).Include(c => c.Steps).Include(c=> c.CaseFiles).Include(c => c.Applications).FirstOrDefaultAsync(c => c.Id == id);
                //if (c == null)
                //{
                //    return false;
                //}
                //DB.Entry(c).CurrentValues.SetValues(mycase);
                //c.Steps.Clear();
                //c.Tags.Clear();
                //c.CaseFiles.Clear();
                //c.Applications.Clear();

                //foreach (var tag in mycase.Tags)
                //{
                //    c.Tags.Add(tag);
                //}
                //foreach (var tag in mycase.Tags)
                //{

                //}
                //foreach (var tag in mycase.Tags)
                //{

                //}
                //foreach (var tag in mycase.Tags)
                //{

                //}

                //c.Steps = mycase.Steps;
                //c.Tags = mycase.Tags;
                //c.CaseFiles = mycase.CaseFiles;
                //c.Applications = mycase.Applications;

                //c.Steps.Union(mycase.Steps);
                //c.Tags.Union(mycase.Tags);
                //c.CaseFiles.Union(mycase.CaseFiles);
                //c.Applications.Union(mycase.Applications);
                //c.Title = mycase.Title;
                //c.Description = mycase.Description;

                return true;
                //return (await DB.SaveChangesAsync())>0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<Case>> Search(string title, string[] tags)
        {
            throw new NotImplementedException();
        }
    }
}
