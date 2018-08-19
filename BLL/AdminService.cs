using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBLL;
using Model;

namespace BLL
{
    public class AdminService:BaseService<Admin>,IAdminService
    {
        public override void SetDal()
        {
            CurrentDal = DbSession.AdminDal;
        }

        public bool CheckAdmin(Admin admin)
        {
            if (LoadEntities(m => m.admin_name.Equals(admin.admin_name) && m.admin_pwd.Equals(admin.admin_pwd)).Any())
            {
                return true;
            }

            return false;
        }

    }
}
