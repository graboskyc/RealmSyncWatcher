using System;
using System.Threading.Tasks;
using Nito.AsyncEx;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System.Text;

namespace RealmSyncTimerApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Opening...");

            // parse cli args
            if (args.Length != 2)
            {
                Console.WriteLine("Need 2 args: realm app ID followed by API key for auth in that app.");
                return;
            }

            var realmAppId = args[0];
            var apiKey = args[1];

            AsyncContext.Run(async () => await MainAsync(realmAppId, apiKey));
        }

        private static async Task MainAsync(string realmAppId, string apiKey)
        {
            // realm setup
            var app = App.Create(realmAppId);
            var user = await app.LogInAsync(Credentials.ApiKey(apiKey));
            var partition = "GLOBAL";
            RealmConfiguration.DefaultConfiguration = new SyncConfiguration(partition, user);

            using var realm = Realm.GetInstance();

            //Console.WriteLine("There is this much data: " + realm.All<DataPoint>().Count().ToString());       

            realm.RealmChanged += (sender, eventArgs) =>
            {

                Console.WriteLine("MSG RECVD: " + DateTime.Now.ToString("HH:mm:ss.ffffff"));
            };

            // wait forever
            Console.WriteLine("Press any key to exit.");
            await Task.Run(() =>
            {
                Console.ReadLine();
            });
        }



        public class DataPoint : RealmObject {
            [PrimaryKey]
            [MapTo("_id")]
            public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
            [MapTo("key")]
            public string Key { get; set; }
            [MapTo("reading")]
            public string Reading { get; set; }
        }
    }
}