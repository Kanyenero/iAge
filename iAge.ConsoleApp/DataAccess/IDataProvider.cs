using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp.DataAccess
{
    public interface IDataProvider<T> 
        where T : class, IIndexableModel
    {
        int Add(T modelToAdd);

        int Update(int id, T newModel);

        T? Get(int id);

        int Delete(int id);

        IEnumerable<T> GetAll();
    }
}
