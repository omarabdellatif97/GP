using DAL.Models;
using GP_API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public class CaseFileRepo : ICaseFileRepo
    {
        private readonly CaseContext db;
        //private readonly ICaseFileUrlMapper fileUrlMapper;

        public CaseFileRepo(CaseContext _DB)
        {
            db = _DB;
            //this.fileUrlMapper = _fileUrlMapper;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                db.CaseFiles.Remove(await db.CaseFiles.FindAsync(id));

                //var scheduled = db.ScheduledCaseFiles.FirstOrDefault(d => d.CaseFileId == id);
                //if (scheduled  != null)
                //    db.ScheduledCaseFiles.Remove(scheduled);

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<CaseFile> Get(string url)
        {
            try
            {
                return db.CaseFiles.FirstOrDefault((c) => c.FileURL.Equals(url));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CaseFile> GetById(int id)
        {
            try
            {
                return await db.CaseFiles.FirstOrDefaultAsync((c) => c.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<CaseFile> GetAll()
        {
            try
            {
                return db.CaseFiles.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Insert(CaseFile mycase)
        {
            try
            {
                db.CaseFiles.Add(mycase);
                //db.ScheduledCaseFiles.Add(new ScheduledCaseFile() { CaseFile= mycase });
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(int id, CaseFile mycase)
        {
            try
            {
                CaseFile temp = await db.CaseFiles.FindAsync(id);
                temp.FileURL = mycase.FileURL;
                temp.FileSize = mycase.FileSize;
                temp.FileName = mycase.FileName;
                temp.Extension = mycase.Extension;
                temp.ContentType = mycase.ContentType;
                return await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CaseFile>> GetAll(List<int> ids)
        {
            return await db.CaseFiles.Where(file => ids.Any(i => file.Id == i)).AsNoTracking().ToListAsync();
            
        }
    }
}
