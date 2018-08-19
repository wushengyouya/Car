using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DALFactory;
using IBLL;
using IDAL;

namespace BLL
{
    public abstract class BaseService<T> where T : class, new()
    {
        //获取数据操作类
        public IDbSession DbSession
        {
            get { return DbSessionFactory.GetDbSession(); }
        }
        public IBaseDal<T> CurrentDal { get; set; }

        public abstract void SetDal();

        protected BaseService()
        {
            SetDal();
        }

        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            return DbSession.SaveChanges()?entity:null;
        }

        //删除单个模型
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return DbSession.SaveChanges();
        }

        //编辑
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return DbSession.SaveChanges();
        }

        //查询
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }

        //分页
        public IQueryable<T> LoadPageEntities<TS>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, TS>> orderByLambda, bool isAsc)
        {
            return  CurrentDal.LoadPageEntities<TS>(pageIndex, pageSize, out totalCount, whereLambda, orderByLambda, isAsc);
        }

        //删除多个模型
        public bool DeleteEntity(List<T> entityList)
        {
           
            foreach (T item in entityList)
            {
                CurrentDal.DeleteEntity(item);
            }
            return DbSession.SaveChanges();
        }


    }
}
