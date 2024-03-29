﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public interface ICaseFileRepo
    {

        Task<CaseFile> Get(string url);
        Task<CaseFile> GetById(int id);
        IEnumerable<CaseFile> GetAll();
        Task<IEnumerable<CaseFile>> GetAll(List<int> ids);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, CaseFile mycase);
        Task<bool> Insert(CaseFile mycase);
    }

}
