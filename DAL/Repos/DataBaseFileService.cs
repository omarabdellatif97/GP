﻿using DAL.Models;
using GP_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public class DataBaseFileService : IFileRepo
    {
        private readonly CaseContext DB;
        public DataBaseFileService(CaseContext _DB)
        {
            DB = _DB;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                DB.CaseFiles.Remove(await DB.CaseFiles.FindAsync(id));
                await DB.SaveChangesAsync();
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
                return DB.CaseFiles.FirstOrDefault((c) => c.FileURL.Equals(url));
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
                return DB.CaseFiles.FirstOrDefault((c) => c.Id.Equals(id));
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
                return DB.CaseFiles.ToList();
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
                DB.CaseFiles.Add(mycase);
                await DB.SaveChangesAsync();
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
                CaseFile temp = await DB.CaseFiles.FindAsync(id);
                temp.FileURL = mycase.FileURL;
                temp.FileSize = mycase.FileSize;
                temp.FileName = mycase.FileName;
                temp.Extension = mycase.Extension;
                temp.ContentType = mycase.ContentType;
                return await DB.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
