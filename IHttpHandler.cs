using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swift
{
    public interface IHttpHandler
    {
        Task<List<T>> GetObjects<T>(string endpoint);
    }
}