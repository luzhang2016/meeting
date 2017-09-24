using System.Collections.Generic;

namespace Meeting.Admin.Models
{
  /// <summary>
  /// 部门
  /// </summary>
  public class BranchUserInfo
  {

    /// <summary>
    /// 部门表ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 上级部门ID
    /// </summary>
    public int ParentID { get; set; }

    /// <summary>
    /// 服务器标识ID
    /// </summary>
    public string ServerID { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// 部门类型
    /// </summary>
    public int BranchType { get; set; }

    /// <summary>
    /// 排序字段，从小到大排
    /// </summary>
    public double Position { get; set; }

    /// <summary>
    /// 组织结构显示的顶级部门
    /// </summary>
    public int DisplayLevel { get; set; }

    /// <summary>
    /// 是否是隐私部门
    /// </summary>
    public bool IsSecrecy { get; set; }

    /// <summary>
    /// 如果是隐私部门，可以看到隐私的部门。ID以,隔开
    /// </summary>
    public string AllowSeenBranchs { get; set; }

    /// <summary>
    /// 如果是隐私部门，可以看到隐私部门的友好部门，多个ID以,隔开
    /// </summary>
    public string FriendBranchs { get; set; }

    /// <summary>
    /// 如果是隐私部门，可以看到隐私部门的角色ID，多个以,隔开
    /// </summary>
    public string AllowRoleIDs { get; set; }

    /// <summary>
    /// 版本
    /// </summary>      
    public int Version { get; set; }

    /// <summary>
    /// 上级部门
    /// </summary>
    public BranchUserInfo Parent { get; set; }
  }

  /// <summary>
  /// 调用外部API返回的结果
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class OuterApiResult2<T>
  {
    /// <summary>
    /// 是否成功标志
    /// </summary>
    public bool success;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string message;

    /// <summary>
    /// 数据
    /// </summary>
    public T data;
  }

  /// <summary>
  /// 调用外部API返回的结果
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class OuterApiResult<T>
  {
    /// <summary>
    /// 是否成功标志
    /// </summary>
    public bool success;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string message;

    /// <summary>
    /// 数据
    /// </summary>
    public IEnumerable<T> data;
  }

  /// <summary>
  /// 用户信息
  /// </summary>
  public class User
  {
    /// <summary>
    /// 编号
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 登录账号
    /// </summary>      
    public string LoginName { get; set; }

    /// <summary>
    /// 显示名
    /// </summary>      
    public string DisplayName { get; set; }

    /// <summary>
    /// 部门编号
    /// </summary>
    public int BranchID { get; set; }

    /// <summary>
    /// 排序顺序，按从小到大排
    /// </summary>      
    public double Position { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string Company { get; set; }
  }
  }
