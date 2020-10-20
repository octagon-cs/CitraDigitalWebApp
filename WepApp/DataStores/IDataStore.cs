using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.DataStores{
    public interface IDataStores<T> where T :class{
       Task<T> Insert(T t);
        Task<T> InsertAndGetLastId(T t);
        Task<bool> Update(T t);
        Task<bool> Delete(T t );

        Task<T> GetById(int id);
        Task<IEnumerable<T>> Get();

    }
}