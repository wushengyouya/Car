using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class DbContextFactory
    {
        public static DbContext GetDbContext()
        {
            //保证线程内唯一
            var dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext==null)
            {
                dbContext = new CarEntities();
                CallContext.SetData("dbContext", dbContext);
            }

            return dbContext;
        }
    }
}
