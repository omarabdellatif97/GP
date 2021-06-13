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

    class CaseRepo
    {

    }

    class FileRepo
    {

    }



}
