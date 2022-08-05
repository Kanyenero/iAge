namespace iAge.ConsoleApp.Models
{
    public delegate void ModelEventHandler<Model>(object sender, ModelEventArgs<Model> e);

    public class ModelEventArgs<T> : EventArgs
    {
        public T Model { get; protected set; }

        public ModelEventArgs(T model)
        {
            Model = model;
        }

        public static implicit operator ModelEventArgs<T>(T model) => new(model);
    }
}
