using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API.Services
{



    public interface ICaseRepo
    {
        Case Get(int id);
        IEnumerable<Case> GetAll();
        IEnumerable<Case> GetAll(int page);
        bool Delete(int id);
        bool Update(int id, Case mycase);
        bool Insert(Case mycase);
    }

    public interface IFileRepo
    {

        CaseFile Get(string url);
        IEnumerable<CaseFile> GetAll();
        bool Delete(int id);
        bool Update(int id, CaseFile mycase);
        bool Insert(CaseFile mycase);


    }

    class CaseRepo : ICaseRepo
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Case Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Case> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Case> GetAll(int page)
        {
            throw new NotImplementedException();
        }

        public bool Insert(Case mycase)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Case mycase)
        {
            throw new NotImplementedException();
        }
    }

    class FileRepo : IFileRepo
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CaseFile Get(string url)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CaseFile> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Insert(CaseFile mycase)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, CaseFile mycase)
        {
            throw new NotImplementedException();
        }
    }



}
