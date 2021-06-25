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
                var mycase = await Get(id);
                if (mycase == null)
                    throw new Exception("case not found.");

                if (mycase.CaseFiles != null)
                    DB.CaseFiles.RemoveRange(mycase.CaseFiles);


                if (mycase.Tags != null)
                    DB.Tags.RemoveRange(mycase.Tags);


                if (mycase.Steps != null)
                    DB.Steps.RemoveRange(mycase.Steps);


                //if (mycase.Applications != null) 
                //    mycase.Applications.Clear();

                DB.Cases.Remove(mycase);
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
                    .Include(c => c.User)
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
                return await DB.Cases.Skip((page - 1) * 25)
                    .Include((c) => c.CaseFiles)
                    .Include((c) => c.Applications)
                    .Include(c => c.User)
                    .Include((c) => c.Steps).Include((c) => c.Tags).ToListAsync();
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
                foreach (var tag in mycase.Tags)
                {
                    tag.Id = 0;
                }
                foreach (var caseFile in mycase.CaseFiles)
                {
                    var origCaseFile = await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id);
                    if (origCaseFile != null)
                    {
                        caseFile.FileURL = (await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id)).FileURL;
                    }
                }
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


        //private bool CheckName(string name, string searchName)
        //{
        //    if (searchName == null || name.Equals(searchName)) return true;
        //    return false;
        //}
        //private bool CheckApplications(ICollection<Application> applications, ICollection<string> searchApplications)
        //{
        //    if (searchApplications == null || applications.Select(t => t.Name.ToLower()).Any(n => searchApplications.Select(s => s.ToLower()).Contains(n))) return true;
        //        return false;
        //}
        //private bool CheckTags(ICollection<Tag> Tags, ICollection<string> searchTags)
        //{
        //    if (searchTags == null || Tags.Select(t => t.Name.ToLower()).Any(n => searchTags.Select(s => s.ToLower()).Contains(n))) return true;
        //    return false;
        //}



        public async Task<IEnumerable<Case>> Search(SearchModel SearchFilter)
        {
            try
            {
                IQueryable<Case> cases = DB.Cases;
                if (SearchFilter.Applications != null && SearchFilter.Applications.Count != 0)
                {
                    cases = cases.Where(C => C.Applications.Select(A => A.Name.ToLower()).Any(A => SearchFilter.Applications.Contains(A)));
                }

                if (SearchFilter.Tags != null && SearchFilter.Tags.Count != 0)
                {
                    cases = cases.Where(C => C.Tags.Select(T => T.Name.ToLower()).Any(T => SearchFilter.Tags.Contains(T)));
                }

                if (SearchFilter.Title != null && SearchFilter.Title != "")
                {
                    cases = cases.Where(C => C.Title.ToLower().Contains(SearchFilter.Title.ToLower()));
                }

                if (SearchFilter.PageNum != null)
                {
                    if (SearchFilter.PageCnt != null && SearchFilter.PageCnt != 0)
                    {
                        cases = cases.Skip((int)SearchFilter.PageNum * (int)SearchFilter.PageCnt);
                    }
                    else
                    {
                        cases = cases.Skip((int)SearchFilter.PageNum * 10);
                    }
                }
                else if (SearchFilter.PageCnt != null)
                {
                    cases = cases.Take((int)SearchFilter.PageCnt);
                }
                var retCases = await cases.Include(c => c.Applications).Include(c => c.User)
                    .Include(c => c.Tags).ToListAsync();
                return retCases;
                //return await DB.Cases.Include((c) => c.Applications).Where((C) => CheckName(C.Title, SearchFilter.Name) && CheckApplication(C.Applications, SearchFilter.Application) && CheckTags(C.Tags, SearchFilter.Tags)).Include(c => c.Steps).Include(c => c.Tags).Include(c => c.CaseFiles).ToListAsync();

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

                foreach (var caseFile in mycase.CaseFiles)
                {
                    var origCaseFile = await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id);
                    if (origCaseFile != null)
                    {
                        caseFile.FileURL = (await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id)).FileURL;
                    }
                }
                Case c = await DB.Cases
                    .Include(c => c.Tags)
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

        //public Task<IEnumerable<Case>> Search(string title, string[] tags)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
