using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public class DataBaseFileService : IFileRepo
    {
        private readonly CaseContext DB;
        public DataBaseFileService(CaseContext _DB)
        {
            this.DB = _DB;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                return DB.CaseFiles.Remove(await DB.CaseFiles.FindAsync(id)) != null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CaseFile> Get(string url)
        {
            try
            {
                return DB.CaseFiles.FirstOrDefault((c) => c.FileURL.Equals(url));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CaseFile> GetById(string id)
        {
            try
            {
                return DB.CaseFiles.FirstOrDefault((c) => c.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<CaseFile> GetAll()
        {
            try
            {
                return DB.CaseFiles.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Insert(CaseFile mycase)
        {
            try
            {
                return (await DB.CaseFiles.AddAsync(mycase)) != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(int id, CaseFile mycase)
        {
            try
            {
                CaseFile temp = await DB.CaseFiles.FindAsync(id);
                temp.FileURL = mycase.FileURL;
                temp.FileSize = mycase.FileSize;
                temp.FileName = mycase.FileName;
                temp.Extension = mycase.Extension;
                temp.ContentType = mycase.ContentType;
                return (await DB.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
