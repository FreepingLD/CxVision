using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Sockets;

namespace DateBase
{
    public   class MangoDbCtrl
    {
        private static MangoDbCtrl _instance;
        private static object LockObj =new object();

        public IMongoDatabase database;
        public MongoClient client;
        public string connStr = "mongodb://localhost:27017";
        //public string connStr = "mongodb://192.168.1.100:27017";
        /// <summary> 对应的数据库名字 </summary>
        public string dbName = "FM";
        public IMongoCollection<BsonDocument> BsonDocCollect;

        public static MangoDbCtrl Instance
        {
            get{
                lock (LockObj) {
                    return _instance = _instance ??  new MangoDbCtrl(); ;
                }         
            }
        }

        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <returns></returns>
        public bool ConnectDB()
        {
            try {
                client = new MongoClient(connStr);
                database = client.GetDatabase(dbName);
                return true;
            }
            catch (Exception ex)
            {
                //FileLib. Logger.Pop("数据库连接失败："+ex.ToString());
                return false;
            }
        }

        public bool DisConnectDB()
        {
            try  {
                client.DropDatabase(dbName);
            }
            catch  {
                return false;            
            }           
            return true;
        }

        /// <summary>
        /// 增加一个Doc到MgDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MgDbDocName">文档名字 </param>
        /// <param name="DataIn">插入文档中的没数据</param>
        /// <returns></returns>
        public bool AddDocToDb<T>(string MgDbDocName, T DataIn)
        {
            //不耗时
            try
            {
                this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(DataIn);
                var document = BsonSerializer.Deserialize<BsonDocument>(str);
                this.BsonDocCollect.InsertOne(document);
                return true;
            }
            catch (Exception e0)
            {
                //FileLib.Logger.Pop("检测结果写入数据库失败。"+ e0.ToString());
                return false;
            }
        }

        public List<T> FindLateastRecode<T>(String MgDbDocName, T TypeIn)
        {
            List<T> MyList = new List<T>();
            try
            {
                this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
                var filter = Builders<BsonDocument>.Filter;
                var sort = Builders<BsonDocument>.Sort;
                var docs = this.BsonDocCollect.Find(filter.Exists("_id"))//查询包含字段"_id"的所有数据
                                     .Sort(sort.Descending("Time")).Limit(7).ToList();//按时间降序，查询最新7条数据
                foreach (var item in docs)
                {
                    string str = item.ToJson();
                    T IT = BsonSerializer.Deserialize<T>(str);
                    MyList.Add(IT);
                }
            }
            catch (Exception e0)
            {
            }
            return MyList;
        }

        public List<T> FindDocFromMgDb2<T>(String MgDbDocName, T TypeIn)
        {
            List<T> MyList = new List<T>();
            try {
                this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
                var result = this.BsonDocCollect.Find(new BsonDocument()).ToList();
                foreach (var item in result) {
                    string str = item.ToJson();
                    T IT = BsonSerializer.Deserialize<T>(str);
                    //T IT = BsonSerializer.Deserialize<T>(item);
                    MyList.Add(IT);
                }
            }
            catch (Exception e0)
            { }
            return MyList;
        }

        /// <summary>
        /// 删除数据库中相关的Doc
        /// </summary>
        /// <param name="MgDbDocName">数据库文档名字 </param>
        /// <param name="VariName"> 变量名 </param>
        /// <param name="VariValue">变量值</param>
        /// <returns></returns>
        public bool DeleteAllMgDbDoc(string MgDbDocName, string VariName, string VariValue)
        {
            this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
            var filter = Builders<BsonDocument>.Filter.Eq(VariName, VariValue);
            var rlt = this.BsonDocCollect.DeleteManyAsync(filter).Result;
            return true;
        }

        /// <summary>
        /// 删除数据库中一个Doc
        /// </summary>
        /// <param name="MgDbDocName">数据库名字 </param>
        /// <param name="VariName"> 变量名 </param>
        /// <param name="VariValue">变量值</param>
        /// <returns></returns>
        public bool DeleteOneMgDbDoc(string MgDbDocName, string VariName, string VariValue)
        {
            this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
            var filter = Builders<BsonDocument>.Filter.Eq(VariName, VariValue);
            var rlt = this.BsonDocCollect.DeleteOneAsync(filter).Result;
            //var rlt1 =  this.BsonDocCollect.Find(filter);
            return true;
        }

        /// <summary>
        /// 查找数据中所有相关文档的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MgDbDocName"></param>
        /// <param name="TypeIn"></param>
        /// <returns></returns>
        public List<T> FindDocFromMgDb<T>(String MgDbDocName, T TypeIn)
        {
            List<T> MyList = new List<T>();
            try{
                this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
                var result = this.BsonDocCollect.Find(new BsonDocument()).ToList();
                foreach (var item in result){
                    T IT = BsonSerializer.Deserialize<T>(item);
                    MyList.Add(IT);
                }
            }
            catch(Exception e0)
            { }
            return MyList;
        }
        /// <summary>
        /// 找到指定的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MgDbDocName"></param>
        /// <param name="VariName"></param>
        /// <param name="VariValue"></param>
        /// <param name="OutList"></param>
        /// <returns></returns>
        public bool FindMgDbDoc<T>(string MgDbDocName, string VariName, string VariValue, out List<T> OutList)
        {
            OutList = new List<T>();

            this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
            var filter = Builders<BsonDocument>.Filter.Eq(VariName, VariValue);
          
            var rlt = this.BsonDocCollect.Find(filter).ToList();
            foreach (var item in rlt){
                T IT = BsonSerializer.Deserialize<T>(item);
                OutList.Add(IT);
            }
            return true;
        }

        public bool FindMgDbDocTime<T>(string MgDbDocName, string VariName, DateTime StartTime, out List<T> OutList)
        {
            OutList = new List<T>();
            var filter = Builders<BsonDocument>.Filter.Gt(VariName, StartTime);
            var rlt = this.BsonDocCollect.Find(filter).ToList();
            foreach (var item in rlt){
                T IT = BsonSerializer.Deserialize<T>(item);
                OutList.Add(IT);
            }
            return true;
        }


        /// <summary>
        /// 更改数据库中某个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MgDbDocName"></param>
        /// <param name="VariName"></param>
        /// <param name="VariValue"></param>
        /// <param name="DataIn"></param>
        /// <returns></returns>
        public bool ReplaceDocInDb<T>(string MgDbDocName, string VariName, string VariValue, T DataIn)
        {
            this.BsonDocCollect = database.GetCollection<BsonDocument>(MgDbDocName);
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(DataIn);
            var document = BsonSerializer.Deserialize<BsonDocument>(str);
            var filter = Builders<BsonDocument>.Filter.Eq(VariName, VariValue);
            this.BsonDocCollect.ReplaceOne(filter, document);
            return true;

          
        }
    }

}
