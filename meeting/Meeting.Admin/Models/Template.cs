using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
    /// <summary>
    /// 模板数据信息
    /// </summary>
    [DataContract]
    public class Template
    {
        /// <summary>
        /// _id
        /// </summary>
        [IgnoreDataMember]
        public MongoDB.Bson.ObjectId _id { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember(Name = "TemplateID")]
        public string TemplateID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Name = "CreateTime")]
        public string CreateTime { get; set; }
  }
}
