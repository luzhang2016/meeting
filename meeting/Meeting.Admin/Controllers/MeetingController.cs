using Meeting.Admin.core;
using Meeting.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meeting.Admin.Controllers
{
  /// <summary>
  /// API接口类
  /// </summary>
  [Produces("application/json")]
  [Route("api/meeting/")]
  public class MeetingController : Controller
  {
    IOptions<AppSettings> setting;
    IHostingEnvironment hostingEnv;
    MeetingDataService dataService;
    FileDataService fileDataService;
    TempDataService tempDataService;
    TemplateDataService templateDataService;
    GroupDataService groupDataService;
    SystemLogDataService systemLogDataService;

    HttpClient client = new HttpClient();

    // 允许时间差：默认60分钟
    const int expiredMinute = 60;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="_setting"></param>
    /// <param name="env"></param>
    /// <param name="_dataService"></param>
    /// <param name="_fileDataService"></param>
    /// <param name="_tempDataService"></param>
    /// <param name="_templateDataService"></param>
    /// <param name="_groupDataService"></param>
    /// <param name="_systemLogDataService"></param>
    public MeetingController(IOptions<AppSettings> _setting, IHostingEnvironment env,
      MeetingDataService _dataService, FileDataService _fileDataService,
      TempDataService _tempDataService, TemplateDataService _templateDataService,
      GroupDataService _groupDataService, SystemLogDataService _systemLogDataService)
    {
      this.setting = _setting;
      this.hostingEnv = env;
      this.dataService = _dataService;
      this.fileDataService = _fileDataService;
      this.tempDataService = _tempDataService;
      this.templateDataService = _templateDataService;
      this.groupDataService = _groupDataService;
      this.systemLogDataService = _systemLogDataService;
    }

    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="username">登录账号</param>
    /// <param name="password">登录密码</param>
    /// <returns></returns>
    [HttpGet]
    [Route("login")]
    public async Task<JsonResult> GetUserInfo(string token, string username, string password)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 调用登录接口
      var resUser = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/Login?serverID=" + setting.Value.serverID + "&username=" + username + "&password=" + password, null).Result;
      string user = "";
      if (resUser.IsSuccessStatusCode == true)
        user = resUser.Content.ReadAsStringAsync().Result;
      else
      {
        return Json(new { success = false, message = "登录接口调用失败" });
      }

      var userInfo = JsonConvert.DeserializeObject<OuterApiResult2<User>>(user).data;
      if (userInfo is null) return Json(new { success = false, message = "账号或密码错误" });

      return Json(new { success = true, message = "", data = new {
        UserName = userInfo.LoginName,
        Name = userInfo.DisplayName,
        Avatar = "https://raw.githubusercontent.com/taylorchen709/markdown-images/master/vueadmin/user.png" } });
    }

    /// <summary>
    /// 获取会议列表
    /// 会议状态：0编辑中 1已关闭 2待进行 3进行中 4已结束
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="name">检索条件</param>
    /// <param name="meetingStatus">会议状态</param>
    /// <param name="publishStatus">发布状态</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<JsonResult> GetMeetings(string token, string name, int? publishStatus, int? meetingStatus)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg }); 

      // 查询所有会议，不指定条件
      // 查询已发布的所有会议，只要指定发布状态
      var meetings = await this.dataService.Find(name, publishStatus, meetingStatus);
      return Json(new { success = true, message = "", data = meetings });
    }

    /// <summary>
    /// 验证令牌
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<string> CheckToken(string token)
    {
      if (String.IsNullOrEmpty(token))
        return "令牌不能为空。";

      // token:MD5(timestamp+参数名与参数值...)|timestamp|随机数
      string[] strs = token.Split('|');
      if (strs.Length != 3)
        return "令牌不对。";

      // 验证时间戳，判断请求是否过期
      var temp = await this.tempDataService.FindOne(strs[1], int.Parse(strs[2]));
      if (temp is null)
      {
        DateTime timestamp = DateTime.ParseExact(strs[1], "yyyyMMddHHmmssfff", null);
        //验证时间戳是否超过指定的分钟数
        if (DateTime.Now.Subtract(timestamp).TotalMinutes > expiredMinute)
          return "请求已经过期。";
      }
      else
        return "请求已经过期。";

      // 验证sign签名
      var sign = StringMD5Encrypt($"{strs[1]}-{strs[2]}-@*api#%^@");
      if (!sign.Equals(strs[0]))
        return "签名不对。";

      // 保存时间戳数据
      await this.tempDataService.Insert(new TempInfo { Timestamp = strs[1], Random = int.Parse(strs[2]) });

      return string.Empty;
    }

    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="strText"></param>
    /// <returns></returns>
    private static string StringMD5Encrypt(string strText)
    {
      byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(strText));

      StringBuilder sBuilder = new StringBuilder();

      for (int i = 0; i < data.Length; i++)
      {
        sBuilder.Append(data[i].ToString("x2"));
      }

      return sBuilder.ToString();
    }

    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static string StreamMD5Encrypt(Stream stream)
    {
      byte[] data = MD5.Create().ComputeHash(stream);

      StringBuilder sBuilder = new StringBuilder();

      for (int i = 0; i < data.Length; i++)
      {
        sBuilder.Append(data[i].ToString("x2"));
      }

      return sBuilder.ToString();
    }

    /// <summary>
    /// 获取会议列表（分页）
    /// 会议状态：0编辑中 1已关闭 2待进行 3进行中 4已结束
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="name">检索条件</param>
    /// <param name="meetingStatus">会议状态</param>
    /// <param name="publishStatus">发布状态</param>
    /// <param name="page">当前页码</param>
    /// <param name="pagesize">每页条数</param>
    /// <returns></returns>
    [HttpGet]
    [Route("listpage")]
    public async Task<JsonResult> GetMeetings(string token, string name, int? publishStatus, int? meetingStatus, int page, int pagesize)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 查询所有会议，不指定条件
      // 查询已发布的所有会议，只要指定发布状态
      var meetings = await this.dataService.Find(name, publishStatus, meetingStatus, page, pagesize);
      var total = await this.dataService.Find(name, publishStatus, meetingStatus);
      return Json(new { success = true, message = "", data = new { total = total.Count(), meetings = meetings } });
    }

    /// <summary>
    /// 获取会议列表（不含会议议程和参会人员）
    /// 会议状态：0编辑中 1已关闭 2待进行 3进行中 4已结束
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="sTime">开始时间:yyyy-mm-dd</param>
    /// <param name="eTime">结束时间:yyyy-mm-dd</param>
    /// <param name="meetingStatus">会议状态</param>
    /// <returns></returns>
    [HttpGet]
    [Route("listSimple")]
    public async Task<JsonResult> GetMeetingsSimple(string token, string sTime, string eTime, int? meetingStatus)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 查询所有会议，不指定条件
      var meetings = await this.dataService.FindSimple(sTime, eTime, meetingStatus);
      return Json(new { success = true, message = "", data = meetings });
    }

    /// <summary>
    /// 根据UUID获取会议详细
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="uuid">UUID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<JsonResult> GetMeeting(string token, string uuid)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var meeting = await this.dataService.FindOne(uuid);
      return Json(new { success = true, message = "", data = meeting });
    }

    /// <summary>
    /// 创建会议
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<JsonResult> CreateMeeting([FromBody]Params param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      if (String.IsNullOrEmpty(param.meetingInfo.MeetingName))
        return Json(new { success = false, message = "参数无效" });

      // 生成UUID
      string uuid = Guid.NewGuid().ToString();

      param.meetingInfo.UUID = uuid;
      // 插入会议主表
      await this.dataService.Insert(param.meetingInfo);

      int fileUUID = 1;
      param.files.ForEach(async file =>
      {
        file.UUID = uuid;
        file.FileUUID = fileUUID++.ToString();
        // 插入附件表
        await this.fileDataService.Insert(file);
      });

      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.meetingInfo.CreateUser, "创建会议"));

      return Json(new { success = true, message = "" });
    }

    /// <summary>
    /// 文件上传到服务器
    /// </summary>
    [HttpPost]
    [Route("uploads")]
    public async Task<JsonResult> SaveAttachment(string token)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var files = Request.Form.Files;

      string fileName = "", fileExtension = "", md5FileContent = "";
      long fileLength = 0;

      foreach (var file in files)
      {
        fileName = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
        fileExtension = Path.GetExtension(fileName);
        fileLength = file.Length;
        md5FileContent = StreamMD5Encrypt(file.OpenReadStream());

        // 文件存放路径
        string path = hostingEnv.WebRootPath + @"\uploads";
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);

        var fileNameAlias = path + $@"\{md5FileContent}" + fileExtension;
        using (FileStream fs = System.IO.File.Create(fileNameAlias))
        {
          file.CopyTo(fs);
          fs.Flush();
        }
      }

      return Json(new
      {
        success = true,
        message = "文件成功上传",
        data = new
        {
          FileName = Path.GetFileNameWithoutExtension(fileName),
          FileExtension = fileExtension,
          FileLength = fileLength,
          URL = "http://" + Request.Host.ToString() + "/uploads/" + md5FileContent + fileExtension
        }
      });
    }

    /// <summary>
    /// 编辑会议
    /// </summary>
    /// <param name="param"></param>
    [HttpPut]
    public async Task<JsonResult> Put([FromBody]Params param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.dataService.Update(param.meetingInfo);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.meetingInfo.UpdateUser, "编辑会议"));

      return Json(new { success = true, message = "" });
    }

    /// <summary>
    /// 删除会议
    /// </summary>
    /// <param name="token"></param>
    /// <param name="uuid"></param>
    /// <param name="user"></param>
    [HttpDelete]
    public async Task<JsonResult> Delete(string token, string uuid, string user)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.dataService.Delete(uuid);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(user, "删除会议"));

      return Json(new { success = true, message = "" });
    }

    /// <summary>
    /// 签到
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="uuid">UUID</param>
    /// <param name="mobile">手机号</param>
    /// <param name="signInAddr">签到地址</param>
    /// <returns>签到成功返回值1，data：排名，签到时间，总人数</returns>
    [Route("signin")]
    [HttpPut]
    public async Task<JsonResult> SignIn(string token, string uuid, string mobile, string signInAddr)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 查询会议信息
      var meeting = await this.dataService.FindOne(uuid);
      List<MeetingMember> members = meeting.MeetingMember;
      // 编辑参会人员签到信息
      int maxIndex = 0;
      foreach (MeetingMember member in members)
      {
        if (member.SignInIndex > maxIndex)
          maxIndex = member.SignInIndex;
      }
      string signInTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      foreach (MeetingMember member in meeting.MeetingMember)
      {
        if (member.Mobile.Equals(mobile))
        {
          member.SignInAddr = signInAddr;
          member.SignInFlag = 1;
          member.SignInTime = signInTime;
          member.SignInIndex = maxIndex + 1;
          break;
        }
      }

      var updateResult = await this.dataService.SignIn(uuid, meeting.MeetingMember);

      // 更新失败,返回0
      if (updateResult.ModifiedCount == 0)
        return Json(new { success = false, message = "签到失败" });
      // 更新成功，返回1 排名，签到时间，总人数
      return Json(new
      {
        success = true,
        message = "签到成功",
        data = new
        {
          signInIndex = maxIndex + 1,
          signInTime = signInTime,
          memberTotal = meeting.MeetingMember.Count
        }
      });
    }

    /// <summary>
    /// 签到详情
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="uuid">UUID</param>
    /// <param name="mobile">手机号</param>
    /// <returns></returns>
    [Route("signin")]
    [HttpGet]
    public async Task<JsonResult> GetSignInInfo(string token, string uuid, string mobile)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      if (uuid is null || mobile is null)
        return Json(new { success = false, message = "参数不能为空" });

      // 查询会议信息
      var meeting = await this.dataService.FindOne(uuid);
      MeetingMember meetingMember = new MeetingMember();
      int maxIndex = 0;
      foreach (MeetingMember member in meeting.MeetingMember)
      {
        // 当前用户签到信息
        if (member.Mobile.Equals(mobile))
          meetingMember = member;
        // 查找已签到人数
        if (member.SignInIndex > maxIndex)
          maxIndex = member.SignInIndex;
      }

      // 返回签到详情
      return Json(new
      {
        success = true,
        message = "",
        data = new
        {
          signInIndex = meetingMember.SignInIndex,
          signInTime = meetingMember.SignInTime,
          signInAddr = meetingMember.SignInAddr,
          signInTotal = maxIndex,
          memberTotal = meeting.MeetingMember.Count
        }
      });
    }

    /// <summary>
    /// 数据统计
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="uuid">UUID</param>
    /// <returns></returns>
    [Route("signInList")]
    [HttpGet]
    public async Task<JsonResult> GetSignInList(string token, string uuid)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      if (uuid is null)
        return Json(new { success = false, message = "参数不能为空" });

      // 查询会议信息
      var meeting = await this.dataService.FindOne(uuid);

      List<MeetingMember> list = meeting.MeetingMember;
      // 查找已签到人数
      var indexList = list.OrderByDescending(a => a.SignInIndex);
      // 对要显示的列表排序：已签到排前，签到先后顺序排
      var sortedList = list.OrderByDescending(a => a.SignInFlag).ThenBy(a => a.SignInIndex);

      // 返回签到详情
      return Json(new
      {
        success = true,
        message = "",
        data = new
        {
          signInTotal = indexList.First().SignInIndex,
          memberTotal = meeting.MeetingMember.Count,
          members = sortedList.ToList()
        }
      });
    }

    /// <summary>
    /// 根据UUID获取会议资料
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="uuid">UUID</param>
    /// <param name="flag">区分附件还是记录</param>
    /// <returns></returns>
    [Route("files")]
    [HttpGet]
    public async Task<JsonResult> GetAttachments(string token, string uuid, int flag)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var files = await this.fileDataService.Find(uuid, flag);
      return Json(new { success = true, message = "", data = files });
    }

    /// <summary>
    /// 更新下载次数
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="fileUUID">文件UUID</param>
    /// <param name="downloadUser">下载人</param>
    [Route("download")]
    [HttpPut]
    public async Task<JsonResult> UpdateTimes(string token, string fileUUID, string downloadUser)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 查询单个文件信息
      var file = await this.fileDataService.FindOne(fileUUID);
      // 添加下载人信息
      List<Download> list = file.Download;
      string downloadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      Download temp = list.Find(e => e.DownloadUser.Equals(downloadUser));
      if (temp is null)
        list.Add(new Download { DownloadUser = downloadUser, DownloadTime = downloadTime });
      else
        temp.DownloadTime = downloadTime;

      var updateResult = await this.fileDataService.Update(fileUUID, list);

      // 更新失败,返回0
      if (updateResult.ModifiedCount == 0)
        return Json(new { success = false, message = "下载次数更新失败" });
      // 更新成功，返回1 
      return Json(new { success = true, message = "下载次数更新成功" });
    }

    /// <summary>
    /// 获取组织树
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("branchs-users")]
    public async Task<JsonResult> GetBranchAndUserList(string token)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var result = new List<ZtreeNodeModel>();
      // 获取部门信息
      var resBranch = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/AllBranchs?serverID=" + setting.Value.serverID + "&parnetID=" + 0, null).Result;
      string branchs = "";
      if (resBranch.IsSuccessStatusCode == true)
        branchs = resBranch.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取部门信息失败" });

      // 获取用户信息
      var resUsers = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/AllUsers?serverID=" + setting.Value.serverID + "&branchID=" + 0, null).Result;
      string users = "";
      if (resUsers.IsSuccessStatusCode == true)
        users = resUsers.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取用户信息失败" });

      result.AddRange(JsonConvert.DeserializeObject<OuterApiResult<BranchUserInfo>>(branchs).data.Select(b => new ZtreeNodeModel()
      {
        ID = b.ID.ToString(),
        Name = b.BranchName,
        IDKey = "d" + b.ID,
        ParentIDKey = "d" + b.ParentID,
        ParentID = b.ParentID,
        Type = BranchNodeType.Branch.ToString(),
        Sort = b.Position
      }));

      result.AddRange(JsonConvert.DeserializeObject<OuterApiResult<User>>(users).data.Select(b => new ZtreeNodeModel()
      {
        ID = b.ID.ToString(),
        Name = b.DisplayName,
        IDKey = "u" + b.ID,
        ParentIDKey = "d" + b.BranchID,
        ParentID = b.BranchID,
        Type = BranchNodeType.Leaf.ToString(),
        Sort = b.Position
      }));

      return Json(new { success = true, message = "", data = result });
    }

    /// <summary>
    /// 获取所有用户列表
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("allusers")]
    public async Task<JsonResult> GetAllUsers(string token)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 获取用户信息
      var resUsers = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/AllUsers?serverID=" + setting.Value.serverID + "&branchID=" + 0, null).Result;
      string users = "";
      if (resUsers.IsSuccessStatusCode == true)
        users = resUsers.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取用户信息失败" });

      return Json(new { success = true, message = "", data = JsonConvert.DeserializeObject<OuterApiResult<User>>(users).data });
    }

    /// <summary>
    /// 获取部门下的用户列表
    /// </summary>
    /// <param name="token"></param>
    /// <param name="branchID"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("users")]
    public async Task<JsonResult> GetUsers(string token, int branchID)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 获取用户信息
      var resUsers = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/Users?serverID=" + setting.Value.serverID + "&branchID=" + branchID, null).Result;
      string users = "";
      if (resUsers.IsSuccessStatusCode == true)
        users = resUsers.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取用户信息失败" });

      return Json(new { success = true, message = "", data = JsonConvert.DeserializeObject<OuterApiResult<User>>(users).data });
    }

    /// <summary>
    /// 获取所有部门列表
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("allbranchs")]
    public async Task<JsonResult> GetAllBranchs(string token)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 获取部门列表
      var resBranchs = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/AllBranchs?serverID=" + setting.Value.serverID + "&parnetID=" + 0, null).Result;
      string branchs = "";
      if (resBranchs.IsSuccessStatusCode == true)
        branchs = resBranchs.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取部门列表失败" });

      return Json(new { success = true, message = "", data = JsonConvert.DeserializeObject<OuterApiResult<BranchUserInfo>>(branchs).data });
    }

    /// <summary>
    /// 获取子部门列表
    /// </summary>
    /// <param name="token"></param>
    /// <param name="parentID"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("branchs")]
    public async Task<JsonResult> GetBranchs(string token, int parentID)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 获取部门列表
      var resBranchs = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/Branchs?serverID=" + setting.Value.serverID + "&parentID=" + parentID, null).Result;
      string branchs = "";
      if (resBranchs.IsSuccessStatusCode == true)
        branchs = resBranchs.Content.ReadAsStringAsync().Result;
      else
        return Json(new { success = false, message = "获取部门列表失败" });

      return Json(new { success = true, message = "", data = JsonConvert.DeserializeObject<OuterApiResult<BranchUserInfo>>(branchs).data });
    }

    /// <summary>
    /// 创建模板
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("template")]
    public async Task<JsonResult> CreateTemplate([FromBody]Params<Template> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 生成UUID
      string uuid = Guid.NewGuid().ToString();

      param.value.TemplateID = uuid;
      param.value.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      // 插入模板
      await this.templateDataService.Insert(param.value);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.user, "新增短信模板"));

      return Json(new { success = true, message = "template create success." });
    }

    /// <summary>
    /// 更新模板
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("template")]
    public async Task<JsonResult> UpdateTemplate([FromBody]Params<Template> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 更新模板
      await this.templateDataService.Update(param.value);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.user, "更新短信模板"));

      return Json(new { success = true, message = "template update success." });
    }


    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="token"></param>
    /// <param name="templateID"></param>
    /// <param name="user"></param>
    [HttpDelete]
    [Route("template")]
    public async Task<JsonResult> DeleteTemplate(string token, string templateID, string user)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.templateDataService.Delete(templateID);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(user, "删除短信模板"));

      return Json(new { success = true, message = "template delete success." });
    }

    /// <summary>
    /// 批量删除模板
    /// </summary>
    /// <param name="token"></param>
    /// <param name="templateIDs"></param>
    /// <param name="user"></param>
    [HttpDelete]
    [Route("template/batchDelete")]
    public async Task<JsonResult> DeleteTemplates(string token, string templateIDs, string user)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.templateDataService.MultiDelete(templateIDs);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(user, "批量删除短信模板"));

      return Json(new { success = true, message = "templates delete success." });
    }

    /// <summary>
    /// 获取模板列表
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="searchKey">查询条件</param>
    /// <returns></returns>
    [HttpGet]
    [Route("template")]
    public async Task<JsonResult> GetTemplates(string token, string searchKey)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var templates = await this.templateDataService.Find(searchKey);
      return Json(new { success = true, message = "", data = templates });
    }

    /// <summary>
    /// 根据模板ID获取模板详细
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="templateID">模板ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route("template/detail")]
    public async Task<JsonResult> GetTemplate(string token, string templateID)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var template = await this.templateDataService.FindOne(templateID);
      return Json(new { success = true, message = "", data = template });
    }

    /// <summary>
    /// 创建分组
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("group")]
    public async Task<JsonResult> CreateGroup([FromBody]Params<Group> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 生成UUID
      string uuid = Guid.NewGuid().ToString();

      param.value.GroupID = uuid;
      param.value.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      // 插入分组
      await this.groupDataService.Insert(param.value);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.user, "新增分组"));

      return Json(new { success = true, message = "group create success." });
    }

    /// <summary>
    /// 更新分组
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("group")]
    public async Task<JsonResult> UpdateGroup([FromBody]Params<Group> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 更新分组
      await this.groupDataService.Update(param.value);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.user, "更新分组"));

      return Json(new { success = true, message = "group update success." });
    }

    /// <summary>
    /// 更新分组人员
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("group/updateGroupMember")]
    public async Task<JsonResult> UpdateGroupMember([FromBody]Params<Group> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 更新分组
      await this.groupDataService.UpdateMembers(param.value);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(param.user, "更新分组人员"));

      return Json(new { success = true, message = "group update success." });
    }


    /// <summary>
    /// 删除分组
    /// </summary>
    /// <param name="token"></param>
    /// <param name="groupID"></param>
    /// <param name="user"></param>
    [HttpDelete]
    [Route("group")]
    public async Task<JsonResult> DeleteGroup(string token, string groupID, string user)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.groupDataService.Delete(groupID);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(user, "删除分组"));

      return Json(new { success = true, message = "group delete success." });
    }

    /// <summary>
    /// 批量删除分组
    /// </summary>
    /// <param name="token"></param>
    /// <param name="groupIDs"></param>
    /// <param name="user"></param>
    [HttpDelete]
    [Route("group/batchDelete")]
    public async Task<JsonResult> DeleteGroups(string token, string groupIDs, string user)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      await this.groupDataService.MultiDelete(groupIDs);
      // 插入日志
      await this.systemLogDataService.Insert(CreateLogInfo(user, "批量删除分组"));

      return Json(new { success = true, message = "groups delete success." });
    }

    /// <summary>
    /// 获取分组列表
    /// </summary>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    [HttpGet]
    [Route("group")]
    public async Task<JsonResult> GetGroups(string token)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var groups = await this.groupDataService.Find();
      return Json(new { success = true, message = "", data = groups });
    }

    /// <summary>
    /// 根据分组ID获取分组详细
    /// </summary>
    /// <param name="token">令牌</param>
    /// <param name="groupID">分组ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route("group/detail")]
    public async Task<JsonResult> GetGroup(string token, string groupID)
    {
      // 验证令牌
      string strMsg = await CheckToken(token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      var group = await this.groupDataService.FindOne(groupID);
      return Json(new { success = true, message = "", data = group });
    }


    /// <summary>
    /// 获取系统日志列表
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("logs")]
    public async Task<JsonResult> GetLogs(Params<SearchKeys> param)
    {
      // 验证令牌
      string strMsg = await CheckToken(param.token);
      if (!String.IsNullOrEmpty(strMsg))
        return Json(new { success = false, message = strMsg });

      // 指定部门查询条件的话，先查询出该部门下的所有用户
      IEnumerable<User> userList = null;
      if (!String.IsNullOrEmpty(param.value.BranchID))
      {
        // 获取部门列表
        var resUsers = client.PostAsync(setting.Value.outerAPIUrl + "/api/Structure/Users?serverID=" + setting.Value.serverID + "&branchID=" + param.value.BranchID, null).Result;
        string users = "";
        if (resUsers.IsSuccessStatusCode == true)
          users = resUsers.Content.ReadAsStringAsync().Result;
        else
        {
          return Json( new { success = false, message = "部门下人员查询失败" });
        }

        userList = JsonConvert.DeserializeObject<OuterApiResult<User>>(users).data;

        // 筛选出指定部门下的用户日志信息
        // result = logs.Where(e => userList.Where(u => u.LoginName.Equals(e.User)).Count() > 0);
      }

      var logs = await this.systemLogDataService.Find(param.value, userList);
      param.value.page = 0;
      param.value.pagesize = 0;
      var total = await this.systemLogDataService.Find(param.value, userList);
      return Json(new { success = true, message = "", data = new { total = total.Count(), data = logs}});
    }

    /// <summary>
    /// 生成日志信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="logInfo"></param>
    /// <returns></returns>
    private SystemLogInfo CreateLogInfo(string user, string logInfo)
    {
      SystemLogInfo log = new SystemLogInfo()
      {
        User = user,
        LogTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        LogInfo = logInfo
      };
      return log;
    }
  }
}
