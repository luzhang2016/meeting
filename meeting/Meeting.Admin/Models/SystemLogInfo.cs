using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
  /// <summary>
  /// 系统日志数据信息
  /// </summary>
  [DataContract]
  public class SystemLogInfo
  {
    /// <summary>
    /// _id
    /// </summary>
    [IgnoreDataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <summary>
    /// 操作账号
    /// </summary>
    [DataMember(Name = "User")]
    public string User { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    [DataMember(Name = "LogTime")]
    public string LogTime { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [DataMember(Name = "IP")]
    public string IP { get; set; }

    /// <summary>
    /// 操作信息
    /// </summary>
    [DataMember(Name = "LogInfo")]
    public string LogInfo { get; set; }

  }

  /// <summary>
  /// 系统日志查询条件信息
  /// </summary>
  [DataContract]
  public class SearchKeys
  {
    /// <summary>
    /// 关键字
    /// </summary>
    [DataMember(Name = "Keyword")]
    public string Keyword { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [DataMember(Name = "STime")]
    public string STime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DataMember(Name = "ETime")]
    public string ETime { get; set; }

    /// <summary>
    /// 部门ID
    /// </summary>
    [DataMember(Name = "BranchID")]
    public string BranchID { get; set; }

    /// <summary>
    /// 当前页
    /// </summary>
    [DataMember(Name = "page")]
    public int page { get; set; }

    /// <summary>
    /// 每页条数
    /// </summary>
    [DataMember(Name = "pagesize")]
    public int pagesize { get; set; }
  }
}
