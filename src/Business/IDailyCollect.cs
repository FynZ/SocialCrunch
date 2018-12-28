using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IDailyCollect<T>
    {
        Task<T> CollectData();
    }
}
