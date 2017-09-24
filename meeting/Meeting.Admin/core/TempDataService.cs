using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Meeting.Admin.core
{
    /// <summary>
    /// 临时数据访问层
    /// </summary>
    public class TempDataService : BaseDataService<TempInfo>
    {
        readonly IMongoCollection<TempInfo> db;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings"></param>
        public TempDataService(IOptions<AppSettings> settings) : base(settings)
        {
            this.db = base.GetCollection("tempData");
        }

        /// <summary>
        /// 查询时间戳
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="random">随机数</param>
        /// <returns></returns>
        public async Task<TempInfo> FindOne(string timestamp, int random)
        {
            var filter = Builders<TempInfo>.Filter;

            return await this.db.Find<TempInfo>(filter.Eq("Timestamp", timestamp) & filter.Eq("Random", random)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 插入临时数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task Insert(TempInfo info)
        {
            await this.db.InsertOneAsync(info);
        }
    }
}
