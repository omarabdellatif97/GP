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
    public class CaseRepo : ICaseRepo
    {
        private readonly CaseContext DB;
        
        public CaseRepo(CaseContext _DB)
        {
            DB = _DB;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var mycase = await GetAsync(id);
                if (mycase == null)
                    throw new Exception("case not found.");

                if(mycase.CaseFiles != null)
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

        public async Task<Case> GetAsync(int id)
        {
            try
            {
                var retCases =  await DB.Cases.Include(c => c.Tags)
                    .Include(c => c.Steps)
                    .Include(c => c.CaseFiles)
                    .Include(c => c.Applications)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                //if(retCases != null)
                //    MapTemplateToDescription(retCases);
                return retCases;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IEnumerable<Case>> GetAllAsync()
        {
            try
            {
                var retCases=  await DB.Cases.Include((c) => c.CaseFiles).Include((c) => c.Steps)
                    .Include((c) => c.Tags).Include((c) => c.Applications).ToListAsync();

                //retCases.ForEach(ca =>
                //{

                //    if (ca != null)
                //        MapTemplateToDescription(ca);
                //});
                return retCases;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Case>> GetAllAsync(int page)
        {
            try
            {
                var retCases = await DB.Cases.Skip((page - 1) * 25)
                    .Include((c) => c.CaseFiles)
                    .Include((c) => c.Applications)
                    .Include(c=> c.User)
                    .Include((c) => c.Steps).Include((c) => c.Tags).AsNoTracking().ToListAsync();

                //retCases.ForEach(ca =>
                //{

                //    if (ca != null)
                //        MapTemplateToDescription(ca);
                //});
                return retCases;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Case mycase)
        {
            try
            {
                if (mycase == null) throw new ArgumentException();
                //foreach (var app in mycase.Applications)
                //{
                //    DB.Attach(app);
                //}
                // casefile url mapping
                //mycase.CaseUrl = $@"Cases/Case-{Guid.NewGuid()}";

                foreach (var tag in mycase.Tags) 
                {
                    tag.Id = 0;
                }
                //foreach (var caseFile in mycase.CaseFiles)
                //{
                //    var origCaseFile = await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id);
                //    if (origCaseFile != null)
                //    {
                //        caseFile.FileURL = (await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id)).FileURL;
                //    }
                //}

                //await MapDescriptionToTemplateAsync(mycase);
                DB.Update(mycase);
                await DB.SaveChangesAsync();
                return true;
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<IEnumerable<Case>> SearchAsync(SearchModel SearchFilter)
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
                var retCases = await cases.Include(c => c.Applications).Include(c=> c.User)
                    .Include(c => c.Tags).AsNoTracking().ToListAsync();

                // add description;
                //retCases.ForEach(ca =>
                //{
                //    if(ca != null)
                //    MapTemplateToDescription(ca);
                //});
                return retCases;
                //return await DB.Cases.Include((c) => c.Applications).Where((C) => CheckName(C.Title, SearchFilter.Name) && CheckApplication(C.Applications, SearchFilter.Application) && CheckTags(C.Tags, SearchFilter.Tags)).Include(c => c.Steps).Include(c => c.Tags).Include(c => c.CaseFiles).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }





        public async Task<bool> UpdateAsync(int id, Case mycase)
        {
            try
            {

                if (mycase == null) throw new ArgumentException();
                foreach (var tag in mycase.Tags)
                {
                    tag.Id = 0;
                }
                //foreach (var caseFile in mycase.CaseFiles)
                //{
                //    var origCaseFile = await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id);
                //    if (origCaseFile != null)
                //    {
                //        caseFile.FileURL = (await DB.CaseFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == caseFile.Id)).FileURL;
                //    }
                //}
                //await MapDescriptionToTemplateAsync(mycase);
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

                return true;
                //return (await DB.SaveChangesAsync())>0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //private async Task MapDescriptionToTemplateAsync(Case mycase)
        //{
        //    if (!string.IsNullOrWhiteSpace(mycase.Description))
        //    {
        //        string template = fileUrlMapper.GenerateTemplate(mycase.Description);
        //        var ids = fileUrlMapper.ExtractIds(mycase.Description);
        //        var urls = fileUrlMapper.ExtractFullUrls(mycase.Description);
        //        if (ids != null || !ids.Any())
        //        {

        //            mycase.Description = template;
        //            var caseFiles = await DB.CaseFiles.Where(file => ids.Any(i => i == file.Id)).AsNoTracking().ToListAsync();
                    
        //            foreach (var item in caseFiles)
        //            {
        //                if (!mycase.CaseFiles.Any(c => c.Id == item.Id)) {
        //                    item.IsDescriptionFile = true;
        //                    mycase.CaseFiles.Add(item);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void MapTemplateToDescription(Case mycase)
        //{
            
        //    if (!string.IsNullOrWhiteSpace(mycase.Description))
        //        mycase.Description = fileUrlMapper.GenerateDescription(mycase.Description);
        //}
    }
}
