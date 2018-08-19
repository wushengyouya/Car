using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IDAL;

namespace DALFactory
{
    public class DbSessionFactory
    {
        //保证线程内唯一对象
        public static IDbSession GetDbSession()
        {
            var idbSession =(IDbSession) CallContext.GetData("dbSession");
            if (idbSession==null)
            {
                idbSession= new DbSession();
                CallContext.SetData("dbSession",idbSession);
            }

            return idbSession;
        }
    }
}
