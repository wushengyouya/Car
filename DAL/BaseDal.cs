using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class BaseDal<T> where T:class ,new ()
    {
        private readonly DbContext _carEntities = DbContextFactory.GetDbContext();
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return _carEntities.Set<T>().Where(whereLambda);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<TS>(int pageIndex, int pageSize, out int totalCount,
            System.Linq.Expressions.Expression<Func<T, bool>> whereLambda,
            System.Linq.Expressions.Expression<Func<T, TS>> orderByLambda, bool isAsc)
        {
            var temp = _carEntities.Set<T>().Where(whereLambda);
            totalCount = temp.Count();//返回总数
            if (isAsc)
            {
                temp = temp.OrderBy<T, TS>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, TS>(orderByLambda).Skip<T>((pageIndex - 1) * pageSize).Take(pageSize);
            }

            
            return temp;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            return _carEntities.Set<T>().Add(entity);
        }


        public bool EditEntity(T entity)
        {
           _carEntities.Entry(entity).State=EntityState.Modified;
            return true;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            _carEntities.Entry(entity).State=EntityState.Deleted;
            return true;
        }
    }
}
