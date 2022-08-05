namespace iAge.ConsoleApp.Models
{
    public interface ICopyableModel<T>
        where T : class
    {
        void ShallowCopyFrom(T other, bool ignoreNull);
        void DeepCopyFrom(T other, bool ignoreNull);
    }
}
