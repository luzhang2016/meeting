using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeting.Admin.core
{
  /// <summary>
  /// 模板数据访问层
  /// </summary>
  public class TemplateDataService : BaseDataService<Template>
  {
    readonly IMongoCollection<Template> db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="settings"></param>
    public TemplateDataService(IOptions<AppSettings> settings) : base(settings)
    {
      this.db = base.GetCollection("template");
    }

    /// <summary>
    /// 查询模板详细内容
    /// </summary>
    /// <param name="templateID">模板ID</param>
    /// <returns></returns>
    public async Task<Template> FindOne(string templateID)
    {
      var filter = Builders<Template>.Filter;

      return await this.db.Find<Template>(filter.Eq("TemplateID", templateID)).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 查询所有模板
    /// </summary>
    /// <param name="searchKey">查询条件</param>
    /// <returns></returns>
    public async Task<IEnumerable<Template>> Find(string searchKey)
    {
      var filter = Builders<Template>.Filter;

      return await this.db.Find<Template>(string.IsNullOrEmpty(searchKey) ? filter.Empty : (filter.Regex("Title", searchKey) | filter.Regex("Content", searchKey))).ToListAsync();
    }

    /// <summary>
    /// 模板更新
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<UpdateResult> Update(Template value)
    {
      var filter = Builders<Template>.Filter.Eq("TemplateID", value.TemplateID);

      var update = Builders<Template>.Update.Set("Title", value.Title).Set("Content", value.Content);

      return await this.db.UpdateOneAsync(filter, update);
    }

    /// <summary>
    /// 保存模板
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task Insert(Template value)
    {
      await this.db.InsertOneAsync(value);
    }

    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="templateID"></param>
    /// <returns></returns>
    public async Task Delete(string templateID)
    {
      var filter = Builders<Template>.Filter.Eq("TemplateID", templateID);
      await this.db.DeleteOneAsync(filter);
    }

    /// <summary>
    /// 删除多个模板
    /// </summary>
    /// <param name="templateIDs"></param>
    /// <returns></returns>
    public async Task MultiDelete(string templateIDs)
    {
      var filter = Builders<Template>.Filter.In("TemplateID", templateIDs.Split(','));
      await this.db.DeleteManyAsync(filter);
    }
  }
}
