using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
    /// <summary>
    /// 临时数据信息
    /// </summary>
    [DataContract]
    public class TempInfo
    {
        /// <summary>
        /// _id
        /// </summary>
        [IgnoreDataMember]
        public MongoDB.Bson.ObjectId _id { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [DataMember(Name = "Timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        [DataMember(Name = "Random")]
        public int Random { get; set; }
    }
}
