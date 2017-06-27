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
        int Create(T a);
        IEnumerable<T> GetAll(int limit, int offset, string order_by);
        Stocks Read(int id);
        int Delete(int id);
        int Edit(int id, T a);
    }
}
