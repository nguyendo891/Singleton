using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    public interface IDatabase
    {
        string GetPopulation(string name);
    }

    
    public class SingletonDatabase: IDatabase
    {
        private Dictionary<string, string> capitals = new Dictionary<string, string>();

        private SingletonDatabase()
        {
            Console.WriteLine("Initializing database");
            var lines = File.ReadAllLines("capitals.txt").Select(x => x.Split('\n')).ToList();
            for (int i = 0; i < lines.Count - 1; i += 2)
            {
                this.capitals.Add(lines[i][0], lines[i + 1][0]);
            }
        }

        public string GetPopulation(string name)
        {
            return capitals[name];
        }

        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(()=> new SingletonDatabase());
        public static SingletonDatabase Instance => instance.Value;
    }


    public class ConfigurableRecordFinder
    {
        private IDatabase database;
        public ConfigurableRecordFinder(IDatabase database)
        {
            if(database == null)
            {
                throw new ArgumentNullException(paramName: nameof(database));
            }
            this.database = database;
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result +=int.Parse(database.GetPopulation(name));
            }
            return result;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var db =  SingletonDatabase.Instance;
            var city = "Tokyo";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
        }
    }
}
