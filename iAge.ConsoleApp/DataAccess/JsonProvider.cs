using Newtonsoft.Json;
using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp.DataAccess
{
    public class JsonProvider<Model> : IDataProvider<Model>, IInvocableProvider<Model>
        where Model : class, IIndexableModel, ICopyableModel<Model>
    {
        private string JsonPath { get; set; }

        public JsonProvider(string jsonPath)
        {
            JsonPath = jsonPath;

            bool jsonExists = File.Exists(JsonPath);

            if (!jsonExists)
            {
                using var fs = File.Create(JsonPath);
            }
        }

        public int Add(Model modelToAdd)
        {
            bool deserialiseSuccess = TryReadAndDeserializeJson(JsonPath, out List<Model> models);

            if (!deserialiseSuccess)
                return 0;

            if (models == null)
            {
                models = new List<Model>();
                modelToAdd.Id = 0;
            }
            else
            {
                int maxId = models.Select(m => m.Id).Max();
                modelToAdd.Id = maxId + 1;
            }

            models.Add(modelToAdd);

            bool serialiseAndWriteSuccess = TrySerializeAndWriteJson(JsonPath, models);

            if (!serialiseAndWriteSuccess)
                return 0;

            return 1;
        }

        public int Delete(int id)
        {
            bool readAndDeserialiseSuccess = TryReadAndDeserializeJson(JsonPath, out List<Model> models);

            if (!readAndDeserialiseSuccess)
                return 0;

            var modelToDelete = models.FirstOrDefault(m => m.Id == id);

            if (modelToDelete == null)
                return 0;

            models.Remove(modelToDelete);

            bool serialiseAndWriteSuccess = TrySerializeAndWriteJson(JsonPath, models);

            if (!serialiseAndWriteSuccess)
                return 0;

            return 1;
        }

        public Model? Get(int id)
        {
            bool readAndDeserialiseSuccess = TryReadAndDeserializeJson(JsonPath, out List<Model> models);

            if (!readAndDeserialiseSuccess)
                return null!;

            return models.FirstOrDefault(m => m.Id == id);
        }

        public int Update(int id, Model newModel)
        {
            bool readAndDeserialiseSuccess = TryReadAndDeserializeJson(JsonPath, out List<Model> models);

            if (!readAndDeserialiseSuccess)
                return 0;

            var requiredModel = models.FirstOrDefault(m => m.Id == id);

            if (requiredModel == null)
                return 0;

            requiredModel.DeepCopyFrom(newModel, ignoreNull: false);

            bool serialiseAndWriteSuccess = TrySerializeAndWriteJson(JsonPath, models);

            if (!serialiseAndWriteSuccess)
                return 0;

            return 1;
        }

        public IEnumerable<Model> GetAll()
        {
            bool readAndDeserialiseSuccess = TryReadAndDeserializeJson(JsonPath, out List<Model> models);

            if (!readAndDeserialiseSuccess)
                return null!;

            return models;
        }

        private static bool TryReadAndDeserializeJson<T>(string path, out List<T> deserialisedData)
        {
            deserialisedData = null!;
            string readData;

            try
            {
                readData = File.ReadAllText(path);
            }
            catch (Exception)
            {
                return false;
            }

            deserialisedData = JsonConvert.DeserializeObject<List<T>>(readData)!;

            return true;
        }

        private static bool TrySerializeAndWriteJson<T>(string path, List<T> listToSerialise)
        {
            string serialisedData = JsonConvert.SerializeObject(listToSerialise);

            try
            {
                File.WriteAllText(path, serialisedData);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void OnAdd(object? sender, ModelEventArgs<Model> e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            int recordsAffected = Add(e.Model);

            if (recordsAffected == 0)
                Console.WriteLine("Cannot add a new record.");
            else
                Console.WriteLine("Successfully added a new record.");
        }

        public void OnDelete(object? sender, ModelEventArgs<Model> e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            int recordsAffected = Delete(e.Model.Id);

            if (recordsAffected == 0)
                Console.WriteLine($"Cannot delete record on id [{e.Model.Id}].");
            else
                Console.WriteLine($"Successfully deleted record on id [{e.Model.Id}].");
        }

        public void OnGet(object? sender, ModelEventArgs<Model> e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            var newEmployee = Get(e.Model.Id);

            if (newEmployee == null)
                Console.WriteLine($"Cannot get record on id [{e.Model.Id}].");
            else
                Console.WriteLine(newEmployee.ToString());
        }

        public void OnUpdate(object? sender, ModelEventArgs<Model> e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            int recordsAffected = Update(e.Model.Id, e.Model);

            if (recordsAffected == 0)
                Console.WriteLine($"Cannot update record on id [{e.Model.Id}].");
            else
                Console.WriteLine($"Successfully updated record on id [{e.Model.Id}].");
        }

        public void OnGetAll(object? sender, ModelEventArgs<Model> e)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            var models = GetAll();

            if (models == null)
                Console.WriteLine("Cannot get records.");
            else
            {
                foreach (var model in models)
                    Console.WriteLine(model.ToString());
            }
        }
    }
}
