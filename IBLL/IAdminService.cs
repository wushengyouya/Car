using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace IBLL
{
    public interface IAdminService:IBaseService<Admin>
    {
        bool CheckAdmin(Admin admin);
    }
}
