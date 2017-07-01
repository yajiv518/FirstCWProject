using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Interfaces
{
    public interface IDataAccess<T>
    {
        ReadStock Create(T a);
        //IEnumerable<T> GetAll(int limit, int offset, string order_by);
        ReadStock Read(int id);
        int Delete(int id);
        ReadStock Edit(int id, T a);
        IEnumerable<ESGetDetail> GetAllStockDetail();
    }
}
