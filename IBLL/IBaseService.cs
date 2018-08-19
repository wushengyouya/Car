using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IBaseService<T> where T:class ,new ()
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">数据条数</param>
        /// <param name="totalCount">总数据条数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderByLambda">排序表达式</param>
        /// <param name="isAsc">是否顺序</param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<TS>(int pageIndex, int pageSize, out int totalCount,
            System.Linq.Expressions.Expression<Func<T, bool>> whereLambda,
            System.Linq.Expressions.Expression<Func<T, TS>> orderByLambda, bool isAsc);

        /// <summary>
        /// 添加model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T AddEntity(T entity);


        /// <summary>
        /// 编辑model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditEntity(T entity);

        /// <summary>
        /// 删除一个model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool DeleteEntity(T entity);

        /// <summary>
        /// 删除多个model
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        bool DeleteEntity(List<T> entityList);

    }
}
