using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZReport.Models;

namespace SZReport.PublicCode
{
    public static class DBHelper
    {
        public static Context _db = new Context();
        public static int Insert<T>(T data)
        {
            _db.Add(data);
            return _db.SaveChanges();
        }

        public static int Insert<T>(List<T> data)
        {
            _db.Add(data);
            return _db.SaveChanges();
        }

        public static int Delete<T>(T data)
        {
            _db.Remove(data);
            return _db.SaveChanges();
        }

        public static int Delete<T>(List<T> data)
        {
            _db.Remove(data);
            return _db.SaveChanges();
        }


    }
}
