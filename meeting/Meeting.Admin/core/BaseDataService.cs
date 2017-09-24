using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Meeting.Admin.Core
{
    /// <summary>
    /// 数据访问基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataService<T>
    {
        readonly MongoClient client;
        readonly IMongoDatabase database = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setting"></param>
        public BaseDataService(IOptions<AppSettings> setting)
        {
            client = new MongoClient(setting.Value.connectionString);
            database = client.GetDatabase(setting.Value.dbName);
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection(string name)
        {
            return this.database.GetCollection<T>(name);
        }

        //static string connStr;

        //static string dbName;

        //string collectionName;

        //public BaseDataProvider(string collectionName, IOptions<AppSettings> settings)
        //{
        //    this.collectionName = collectionName;

        //    connStr = settings.Value.connectionString;
        //    dbName = settings.Value.dbName;
        //}

        //void OpenCollection(Action<IMongoCollection<T>> action)
        //{
        //    var mongo = new MongoClient(connStr);
        //    try
        //    {
        //        var database = mongo.GetDatabase(dbName);

        //        var collection = database.GetCollection<T>(collectionName);

        //        action(collection);

        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Current.Error(ex);
        //        //log ex

        //        Debug.WriteLine(ex);
        //    }
        //}

        //void Q(Action<IQueryable<T>> q)
        //{
        //    OpenCollection(e => q(e.AsQueryable()));
        //}

        ///// <summary>
        ///// 获取列表
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public virtual IEnumerable<T> Find(Func<T, bool> query)
        //{

        //    IEnumerable<T> result = null;

        //    this.Q((e) =>
        //    {
        //        if (query == null)
        //            result = e;
        //        else
        //            result = e.Where(query);
        //    });

        //    return result;
        //}

        //private async void find()
        //{
        //    var mongo = new MongoClient(connStr);
        //    var database = mongo.GetDatabase(dbName);

        //    var collection = database.GetCollection<MeetingInfo>(collectionName);

        //    var list = await collection.Find(x => true).ToListAsync();

        //    foreach (var person in list)
        //    {
        //        Console.WriteLine(person.MeetingName);
        //    }
        //}

        //public virtual T FindOne(Func<T, bool> query)
        //{
        //    T result = default(T);
        //    this.Q((e) => result = e.FirstOrDefault(query));
        //    return result;
        //}

        //public virtual void Insert(T doc)
        //{
        //    if (doc == null)
        //        return;
        //    this.OpenCollection(e =>
        //    {
        //        e.InsertOne(doc);
        //    });

        //}


        ////public virtual void Update(T doc)
        ////{
        ////    if (doc == null)
        ////        return;

        ////    this.OpenCollection(e =>
        ////    {
        ////        var bd = BsonExtensionMethods.ToBsonDocument(doc);

        ////        var query = Query.EQ("ID", bd["ID"]);

        ////        var old = e.find(query);

        ////        if (old == null)
        ////            return;

        ////        var oldDoc = BsonExtensionMethods.ToBsonDocument(old);

        ////        bd["_id"] = oldDoc["_id"];

        ////        e.Update(query, new UpdateDocument(bd));

        ////    });
        ////}

        ////public virtual void Delete<TValue>(Expression<Func<T, TValue>> memberExpression, TValue value)
        ////{
        ////    this.OpenCollection(e =>
        ////    {
        ////        var query = Query<T>.EQ<TValue>(memberExpression, value);
        ////        e.Remove(query);
        ////    });
        ////}

        //public virtual int Count(Func<T, bool> query)
        //{
        //    int total = 0;
        //    this.Q(e =>
        //    {
        //        if (query == null)
        //            total = e.Count();
        //        else
        //            total = e.Count(query);
        //    });
        //    return total;
        //}

        //public virtual bool Exists(Func<T, bool> query)
        //{
        //    bool result = false;

        //    this.Q((e) => result = e.Any(query));

        //    return result;
        //}

        //public virtual IEnumerable<T> GetPagedList<K>(out int total, int pageindex, int pagesize, Func<T, K> orderby, Func<T, bool> query)
        //{
        //    if (pageindex <= 0)
        //        pageindex = 1;

        //    if (pagesize <= 0)
        //        pagesize = 10;

        //    IEnumerable<T> items = null;
        //    int t = 0, skip;
        //    this.Q(e =>
        //    {
        //        IEnumerable<T> q = e;

        //        if (query != null)
        //            q = e.Where(query);

        //        t = q.Count();

        //        skip = (pageindex - 1) * pagesize;

        //        if (skip > t)
        //            skip = t;

        //        items = q.OrderByDescending(orderby).Skip(skip).Take(pagesize);

        //    });

        //    total = t;

        //    return items;

        //}
        //public virtual IEnumerable<T> GetPagedListMulitOrder<K, M, N>(out int total, int pageindex, int pagesize, Func<T, K> orderby, Func<T, M> orderby2, Func<T, N> orderby3, Func<T, bool> query)
        //{
        //    if (pageindex <= 0)
        //        pageindex = 1;

        //    if (pagesize <= 0)
        //        pagesize = 10;

        //    IEnumerable<T> items = null;
        //    int t = 0, skip;
        //    this.Q(e =>
        //    {
        //        IEnumerable<T> q = e;

        //        if (query != null)
        //            q = e.Where(query);

        //        t = q.Count();

        //        skip = (pageindex - 1) * pagesize;

        //        if (skip > t)
        //            skip = t;

        //        items = q.OrderByDescending(orderby).ThenByDescending(orderby2).ThenByDescending(orderby3).Skip(skip).Take(pagesize);

        //    });

        //    total = t;

        //    return items;

        //}



    }
}

