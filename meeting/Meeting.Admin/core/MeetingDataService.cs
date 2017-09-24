using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeting.Admin.core
{
    /// <summary>
    /// 会议数据访问层
    /// </summary>
    public class MeetingDataService : BaseDataService<MeetingInfo>
    {
        readonly IMongoCollection<MeetingInfo> db;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings"></param>
        public MeetingDataService(IOptions<AppSettings> settings) : base(settings)
        {
            this.db = base.GetCollection("meeting");
        }

        /// <summary>
        /// 根据UUID获取会议详细
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<MeetingInfo> FindOne(string uuid)
        {
            var filter = Builders<MeetingInfo>.Filter;

            return await this.db.Find<MeetingInfo>(filter.Eq("UUID", uuid)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <param name="name">检索条件</param>
        /// <param name="meetingStatus">会议状态</param>
        /// <param name="publishStatus">发布状态</param>
        /// <returns></returns>
        public async Task<IEnumerable<MeetingInfo>> Find(string name, int? publishStatus, int? meetingStatus)
        {
            var filter = Builders<MeetingInfo>.Filter;
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            FilterDefinition<MeetingInfo> temp = filter.Empty;

            // 空
            if (!meetingStatus.HasValue) { }
            else
            {
                switch (meetingStatus) { 
                    case 2:// 待进行
                        temp = filter.Gt("StartTime", now);
                        break;
                    case 3:// 进行中
                        temp = filter.And(filter.Lte("StartTime", now), filter.Gt("EndTime", now));
                        break;
                    case 4:// 已结束
                        temp = filter.Lt("EndTime", now);
                        break;
                    default:// 编辑中、已关闭
                        temp = filter.Eq("MeetingStatus", meetingStatus);
                        break;
                }
            }

            return await this.db.Find<MeetingInfo>(filter.And((!String.IsNullOrEmpty(name) ? filter.Regex("MeetingName", name) : filter.Empty),
                (publishStatus.HasValue ? filter.Eq("PublishStatus", publishStatus) : filter.Empty), temp)).ToListAsync();
        }

        /// <summary>
        /// 获取会议列表（分页）
        /// </summary>
        /// <param name="name">检索条件</param>
        /// <param name="meetingStatus">会议状态</param>
        /// <param name="publishStatus">发布状态</param>
        /// <param name="page">当前页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns></returns>
        public async Task<IEnumerable<MeetingInfo>> Find(string name, int? publishStatus, int? meetingStatus, int page, int pagesize)
        {
            var filter = Builders<MeetingInfo>.Filter;
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            FilterDefinition<MeetingInfo> temp = filter.Empty;

            // 空
            if (!meetingStatus.HasValue) { }
            else
            {
                switch (meetingStatus)
                {
                    case 2:// 待进行
                        temp = filter.Gt("StartTime", now);
                        break;
                    case 3:// 进行中
                        temp = filter.And(filter.Lte("StartTime", now), filter.Gt("EndTime", now));
                        break;
                    case 4:// 已结束
                        temp = filter.Lt("EndTime", now);
                        break;
                    default:// 编辑中、已关闭
                        temp = filter.Eq("MeetingStatus", meetingStatus);
                        break;
                }
            }

            return await this.db.Find<MeetingInfo>(filter.And((!String.IsNullOrEmpty(name) ? filter.Regex("MeetingName", name) : filter.Empty),
                (publishStatus.HasValue ? filter.Eq("PublishStatus", publishStatus) : filter.Empty), temp)).Skip((page - 1)*pagesize).Limit(pagesize).ToListAsync();
        }

        /// <summary>
        /// 获取会议列表（不含会议议程和参会人员）
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="meetingStatus">会议状态</param>
        /// <returns></returns>
        public async Task<IEnumerable<MeetingInfo>> FindSimple(string sTime, string eTime, int? meetingStatus)
        {
            var filter = Builders<MeetingInfo>.Filter;
            var projection = Builders<MeetingInfo>.Projection.Exclude("MeetingAgenda").Exclude("MeetingMember");
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            FilterDefinition<MeetingInfo> temp = filter.Empty;

            // 空
            if (!meetingStatus.HasValue) { }
            else
            {
                switch (meetingStatus)
                {
                    case 2:// 待进行
                        temp = filter.Gt("StartTime", now);
                        break;
                    case 3:// 进行中
                        temp = filter.And(filter.Lte("StartTime", now), filter.Gt("EndTime", now));
                        break;
                    case 4:// 已结束
                        temp = filter.Lt("EndTime", now);
                        break;
                    default:
                        break;
                }
            }

            return await this.db.Find<MeetingInfo>(filter.And(!String.IsNullOrEmpty(sTime) ? filter.Gte("StartTime", sTime) : filter.Empty,
                !String.IsNullOrEmpty(eTime) ? filter.Lte("StartTime", eTime) : filter.Empty,
                filter.Eq("PublishStatus", 1), temp)).Project<MeetingInfo>(projection).ToListAsync();
        }

        /// <summary>
        /// 创建会议
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task Insert(MeetingInfo info)
        {
           await this.db.InsertOneAsync(info);
        }

        /// <summary>
        /// 编辑会议
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task Update(MeetingInfo info)
        {
            var filter = Builders<MeetingInfo>.Filter.Eq("UUID", info.UUID);

            var update = Builders<MeetingInfo>.Update.Set("MeetingName", info.MeetingName)
                                                     .Set("MeetingAddr", info.MeetingAddr)
                                                     .Set("StartTime", info.StartTime)
                                                     .Set("EndTime", info.EndTime)
                                                     .Set("MeetingNature", info.MeetingNature)
                                                     .Set("MeetingSummary", info.MeetingSummary)
                                                     .Set("MeetingContent", info.MeetingContent)
                                                     .Set("SendMethod", info.SendMethod)
                                                     .Set("MessageContent", info.MessageContent)
                                                     .Set("TemplateID", info.TemplateID)
                                                     .Set("SendTimeFlag", info.SendTimeFlag)
                                                     .Set("MeetingStatus", info.MeetingStatus)
                                                     .Set("PublishStatus", info.PublishStatus)
                                                     .Set("ReleaseTime", info.ReleaseTime)
                                                     .Set("UpdateUser", info.UpdateUser)
                                                     .Set("UpdateTime", info.UpdateTime)
                                                     .Set("MeetingAgenda", info.MeetingAgenda)
                                                     .Set("MeetingMember", info.MeetingMember)
                                                     .Set("MeetingBusiness", info.MeetingBusiness);

            await this.db.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// 删除会议
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task Delete(string uuid)
        {
            var filter = Builders<MeetingInfo>.Filter.Eq("UUID", uuid);

            await this.db.DeleteOneAsync(filter);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public async Task<UpdateResult> SignIn(string uuid, List<MeetingMember> members)
        {
            var filter = Builders<MeetingInfo>.Filter.Eq("UUID", uuid);

            var update = Builders<MeetingInfo>.Update.Set("MeetingMember", members);

            return await this.db.UpdateOneAsync(filter, update);
        }
    }
}
