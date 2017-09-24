using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DataContract]
    public class UserInfo
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
        /// 登录账号
        /// </summary>
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [DataMember(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [DataMember(Name = "Avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
