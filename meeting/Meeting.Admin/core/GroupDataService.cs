using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeting.Admin.core
{
  /// <summary>
  /// 通知分组数据访问层
  /// </summary>
  public class GroupDataService : BaseDataService<Group>
  {
    readonly IMongoCollection<Group> db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="settings"></param>
    public GroupDataService(IOptions<AppSettings> settings) : base(settings)
    {
      this.db = base.GetCollection("group");
    }

    /// <summary>
    /// 查询分组详细内容
    /// </summary>
    /// <param name="groupID">分组ID</param>
    /// <returns></returns>
    public async Task<Group> FindOne(string groupID)
    {
      var filter = Builders<Group>.Filter;

      return await this.db.Find<Group>(filter.Eq("GroupID", groupID)).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 查询所有分组
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Group>> Find()
    {
      var filter = Builders<Group>.Filter;
      var projection = Builders<Group>.Projection.Exclude("Members").Exclude("CreateTime");

      return await this.db.Find<Group>(filter.Empty).Project<Group>(projection).ToListAsync();
    }

    /// <summary>
    /// 分组更新
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<UpdateResult> Update(Group value)
    {
      var filter = Builders<Group>.Filter.Eq("GroupID", value.GroupID);

      var update = Builders<Group>.Update.Set("GroupName", value.GroupName).Set("Comment", value.Comment);

      return await this.db.UpdateOneAsync(filter, update);
    }

    /// <summary>
    /// 分组的人员更新
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<UpdateResult> UpdateMembers(Group value)
    {
      var filter = Builders<Group>.Filter.Eq("GroupID", value.GroupID);

      var update = Builders<Group>.Update.Set("Members", value.Members);

      return await this.db.UpdateOneAsync(filter, update);
    }

    /// <summary>
    /// 保存分组
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task Insert(Group value)
    {
      await this.db.InsertOneAsync(value);
    }

    /// <summary>
    /// 删除分组
    /// </summary>
    /// <param name="groupID"></param>
    /// <returns></returns>
    public async Task Delete(string groupID)
    {
      var filter = Builders<Group>.Filter.Eq("GroupID", groupID);
      await this.db.DeleteOneAsync(filter);
    }

    /// <summary>
    /// 删除多个分组
    /// </summary>
    /// <param name="groupIDs"></param>
    /// <returns></returns>
    public async Task MultiDelete(string groupIDs)
    {
      var filter = Builders<Group>.Filter.In("GroupID", groupIDs.Split(','));
      await this.db.DeleteManyAsync(filter);
    }
  }
}
