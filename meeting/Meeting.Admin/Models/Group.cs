using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
  /// <summary>
  /// 通知分组数据信息
  /// </summary>
  [DataContract]
  public class Group
  {
    /// <summary>
    /// _id
    /// </summary>
    [IgnoreDataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <summary>
    /// 分组ID
    /// </summary>
    [DataMember(Name = "GroupID")]
    public string GroupID { get; set; }

    /// <summary>
    /// 分组名称
    /// </summary>
    [DataMember(Name = "GroupName")]
    public string GroupName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [DataMember(Name = "Comment")]
    public string Comment { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [DataMember(Name = "CreateTime")]
    public string CreateTime { get; set; }

    /// <summary>
    /// 分组人员
    /// </summary>
    [DataMember(Name = "Members")]
    public List<GroupMember> Members { get; set; }
  }

  /// <summary>
  /// 分组里的人员
  /// </summary>
  public class GroupMember
  {
    /// <summary>
    /// 序号
    /// </summary>
    [DataMember(Name = "Index")]
    public int Index { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [DataMember(Name = "UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DataMember(Name = "Mobile")]
    public string Mobile { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [DataMember(Name = "Branch")]
    public string Branch { get; set; }
  }
}
