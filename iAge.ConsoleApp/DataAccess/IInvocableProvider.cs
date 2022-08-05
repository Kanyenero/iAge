using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp.DataAccess
{
    public interface IInvocableProvider<T>
    {
        void OnAdd(object? sender, ModelEventArgs<T> e);

        void OnUpdate(object? sender, ModelEventArgs<T> e);

        void OnGet(object? sender, ModelEventArgs<T> e);

        void OnDelete(object? sender, ModelEventArgs<T> e);

        void OnGetAll(object? sender, ModelEventArgs<T> e);
    }
}
