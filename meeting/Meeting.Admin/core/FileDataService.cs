using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeting.Admin.core
{
    /// <summary>
    /// 文件数据访问层
    /// </summary>
    public class FileDataService : BaseDataService<Attachment>
    {
        readonly IMongoCollection<Attachment> db;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings"></param>
        public FileDataService(IOptions<AppSettings> settings) : base(settings)
        {
            this.db = base.GetCollection("attachment");
        }

        /// <summary>
        /// 查询单个会议资料
        /// </summary>
        /// <param name="fileUUID">文件UUID</param>
        /// <returns></returns>
        public async Task<Attachment> FindOne(string fileUUID)
        {
            var filter = Builders<Attachment>.Filter;

            return await this.db.Find<Attachment>(filter.Eq("FileUUID", fileUUID)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询会议资料
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="flag">区分附件还是记录</param>
        /// <returns></returns>
        public async Task<IEnumerable<Attachment>> Find(string uuid, int flag)
        {
            var filter = Builders<Attachment>.Filter;

            return await this.db.Find<Attachment>(filter.Eq("UUID", uuid) & filter.Eq("Flag", flag)).ToListAsync();
        }

        /// <summary>
        /// 下载次数累加
        /// </summary>
        /// <param name="fileUUID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<UpdateResult> Update(string fileUUID, List<Download> list)
        {
            var filter = Builders<Attachment>.Filter.Eq("FileUUID", fileUUID);

            var update = Builders<Attachment>.Update.Inc("DownloadTimes", 1).Set("Download", list);

            return await this.db.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task Insert(Attachment file)
        {
            await this.db.InsertOneAsync(file);
        }
    }
}
