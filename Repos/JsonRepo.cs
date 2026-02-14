using System.Text.Json;

namespace MiniStore.Repos
{
    public class JsonRepo<T>
    {
        private string _path;

        public JsonRepo(string path)
        {
           
            _path = path;
             if(!File.Exists(_path))
                 File.WriteAllText(_path, "[]");
        }

        public List<T> ReadAll()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<T>>(json)?? new List<T>();
        }

        public void WriteAll(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(_path, json);
        }
    }
}