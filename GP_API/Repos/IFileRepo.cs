﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{
    public interface IFileRepo
    {

        Task<CaseFile> Get(string url);
        Task<CaseFile> GetById(string id);
        IEnumerable<CaseFile> GetAll();
        Task<bool> Delete(int id);
        Task<bool> Update(int id, CaseFile mycase);
        Task<bool> Insert(CaseFile mycase);


    }

}
