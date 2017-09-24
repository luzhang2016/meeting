using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
  /// <summary>
  /// 接口传参数
  /// </summary>
  [DataContract]
  public class Params
  {
    /// <summary>
    /// Token
    /// </summary>
    [DataMember(Name = "token")]
    public string token { get; set; }

    /// <summary>
    /// meetingInfo
    /// </summary>
    [DataMember(Name = "meetingInfo")]
    public MeetingInfo meetingInfo { get; set; }

    /// <summary>
    /// files
    /// </summary>
    [DataMember(Name = "files")]
    public List<Attachment> files { get; set; }
  }

  /// <summary>
  /// 接口传参数
  /// </summary>
  [DataContract]
  public class Params<T>
  {
    /// <summary>
    /// Token
    /// </summary>
    [DataMember(Name = "token")]
    public string token { get; set; }

    /// <summary>
    /// value
    /// </summary>
    [DataMember(Name = "value")]
    public T value { get; set; }

    /// <summary>
    /// 登录用户账号
    /// </summary>
    [DataMember(Name = "user")]
    public string user { get; set; }
  }

  /// <summary>
  /// 会议
  /// </summary>
  [DataContract]
  public class MeetingInfo
  {
    /// <summary>
    /// _id
    /// </summary>
    [IgnoreDataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <summary>
    /// UUID
    /// </summary>
    [DataMember(Name = "UUID")]
    public string UUID { get; set; }

    /// <summary>
    /// 会议名称
    /// </summary>
    [DataMember(Name = "MeetingName")]
    public string MeetingName { get; set; }

    /// <summary>
    /// 会议地址
    /// </summary>
    [DataMember(Name = "MeetingAddr")]
    public string MeetingAddr { get; set; }

    /// <summary>
    /// 会议开始时间
    /// </summary>
    [DataMember(Name = "StartTime")]
    public string StartTime { get; set; }

    /// <summary>
    /// 会议结束时间
    /// </summary>
    [DataMember(Name = "EndTime")]
    public string EndTime { get; set; }

    /// <summary>
    /// 会议性质
    /// </summary>
    [DataMember(Name = "MeetingNature")]
    public int MeetingNature { get; set; }

    /// <summary>
    /// 会议摘要
    /// </summary>
    [DataMember(Name = "MeetingSummary")]
    public string MeetingSummary { get; set; }

    /// <summary>
    /// 会议内容
    /// </summary>
    [DataMember(Name = "MeetingContent")]
    public string MeetingContent { get; set; }

    /// <summary>
    /// 发送方式
    /// </summary>
    [DataMember(Name = "SendMethod")]
    public List<string> SendMethod { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [DataMember(Name = "MessageContent")]
    public string MessageContent { get; set; }

    /// <summary>
    /// 模板ID
    /// </summary>
    [DataMember(Name = "TemplateID")]
    public string TemplateID { get; set; }

    /// <summary>
    /// 通知发送时间标志
    /// </summary>
    [DataMember(Name = "SendTimeFlag")]
    public List<string> SendTimeFlag { get; set; }

    /// <summary>
    /// 会议状态
    /// </summary>
    [DataMember(Name = "MeetingStatus")]
    public int MeetingStatus { get; set; }

    /// <summary>
    /// 发布状态
    /// </summary>
    [DataMember(Name = "PublishStatus")]
    public int PublishStatus { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [DataMember(Name = "ReleaseTime")]
    public string ReleaseTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [DataMember(Name = "CreateUser")]
    public string CreateUser { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [DataMember(Name = "CreateTime")]
    public string CreateTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [DataMember(Name = "UpdateUser")]
    public string UpdateUser { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [DataMember(Name = "UpdateTime")]
    public string UpdateTime { get; set; }

    /// <summary>
    /// 会议议程
    /// </summary>
    [DataMember(Name = "MeetingAgenda")]
    public List<MeetingAgenda> MeetingAgenda { get; set; }

    /// <summary>
    /// 会议参加人员
    /// </summary>
    [DataMember(Name = "MeetingMember")]
    public List<MeetingMember> MeetingMember { get; set; }

    /// <summary>
    /// 会务信息
    /// </summary>
    [DataMember(Name = "MeetingBusiness")]
    public List<MeetingBusiness> MeetingBusiness { get; set; }
  }

  /// <summary>
  /// 会议议程
  /// </summary>
  public class MeetingAgenda
  {
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
    /// 主题
    /// </summary>
    [DataMember(Name = "Title")]
    public string Title { get; set; }

    /// <summary>
    /// 发言人
    /// </summary>
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// 发言内容
    /// </summary>
    [DataMember(Name = "Content")]
    public string Content { get; set; }

    /// <summary>
    /// 摘要
    /// </summary>
    [DataMember(Name = "Summary")]
    public string Summary { get; set; }
  }

  /// <summary>
  /// 会议参加人员
  /// </summary>
  public class MeetingMember
  {
    /// <summary>
    /// 报名时间
    /// </summary>
    [DataMember(Name = "SignUpTime")]
    public string SignUpTime { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DataMember(Name = "Mobile")]
    public string Mobile { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [DataMember(Name = "Branch")]
    public string Branch { get; set; }

    /// <summary>
    /// 职务
    /// </summary>
    [DataMember(Name = "Duty")]
    public string Duty { get; set; }

    /// <summary>
    /// 人员区分：1主持人 2发言人 3普通参会人员
    /// </summary>
    [DataMember(Name = "Flag")]
    public int Flag { get; set; }

    /// <summary>
    /// 签到标志
    /// </summary>
    [DataMember(Name = "SignInFlag")]
    public int SignInFlag { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    [DataMember(Name = "SignInTime")]
    public string SignInTime { get; set; }

    /// <summary>
    /// 签到地址
    /// </summary>
    [DataMember(Name = "SignInAddr")]
    public string SignInAddr { get; set; }

    /// <summary>
    /// 签到排名
    /// </summary>
    [DataMember(Name = "SignInIndex")]
    public int SignInIndex { get; set; }
  }

  /// <summary>
  /// 会务信息
  /// </summary>
  public class MeetingBusiness
  {
    /// <summary>
    /// 作息安排
    /// </summary>
    [DataMember(Name = "Schedule")]
    public List<Plan> Schedule { get; set; }

    /// <summary>
    /// 会议物品
    /// </summary>
    [DataMember(Name = "MeetingItems")]
    public string MeetingItems { get; set; }

    /// <summary>
    /// 接送安排
    /// </summary>
    [DataMember(Name = "Pickup")]
    public List<Plan> Pickup { get; set; }

    /// <summary>
    /// 接送地址
    /// </summary>
    [DataMember(Name = "RAddress")]
    public string RAddress { get; set; }

    /// <summary>
    /// 接待电话
    /// </summary>
    [DataMember(Name = "RTell")]
    public string RTell { get; set; }

    /// <summary>
    /// 接待时间
    /// </summary>
    [DataMember(Name = "RTime")]
    public string RTime { get; set; }

    /// <summary>
    /// 注意事项
    /// </summary>
    [DataMember(Name = "Matters")]
    public string Matters { get; set; }
  }

  /// <summary>
  /// 作息安排或接送安排
  /// </summary>
  public class Plan
  {
    /// <summary>
    /// 事项
    /// </summary>
    [DataMember(Name = "Item")]
    public string Item { get; set; }

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
  }
}
