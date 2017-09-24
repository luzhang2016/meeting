using Meeting.Admin.Core;
using Meeting.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Meeting.Admin.core
{
  /// <summary>
  /// 系统日志数据访问层
  /// </summary>
  public class SystemLogDataService : BaseDataService<SystemLogInfo>
  {
    readonly IMongoCollection<SystemLogInfo> db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="settings"></param>
    public SystemLogDataService(IOptions<AppSettings> settings) : base(settings)
    {
      this.db = base.GetCollection("systemLog");
    }

    /// <summary>
    /// 根据查询条件搜索日志信息
    /// </summary>
    /// <param name="searchKeys">搜索条件</param>
    /// <param name="users"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SystemLogInfo>> Find(SearchKeys searchKeys, IEnumerable<User> users)
    {
      var filter = Builders<SystemLogInfo>.Filter;
      FilterDefinition<SystemLogInfo> temp = filter.Empty;
      string keyword = searchKeys.Keyword;
      string sTime = searchKeys.STime;
      string eTime = searchKeys.ETime;
      int page = searchKeys.page;
      int pagesize = searchKeys.pagesize;

      // 关键字作为查询条件
      if (!String.IsNullOrEmpty(keyword))
        temp = filter.And(temp, filter.Or(filter.Regex("User", keyword), filter.Regex("IP", keyword), filter.Regex("LogInfo", keyword)));
      // 时间作为查询条件
      if (!String.IsNullOrEmpty(sTime))
        temp = filter.And(temp, filter.Gte("LogTime", sTime + " 00:00:01"));
      if (!String.IsNullOrEmpty(eTime))
        temp = filter.And(temp, filter.Lte("LogTime", eTime + " 23:59:59"));
      // 部门作为查询条件
      if (!(users is null))
        temp = filter.And(temp, filter.In("User", users.Select(u => u.LoginName)));

      return await this.db.Find<SystemLogInfo>(temp).Skip((page - 1) * pagesize).Limit(pagesize).ToListAsync();
    }

    /// <summary>
    /// 插入日志信息
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public async Task Insert(SystemLogInfo info)
    {
      await this.db.InsertOneAsync(info);
    }
  }
}
