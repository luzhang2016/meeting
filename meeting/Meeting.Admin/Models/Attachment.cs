using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DataContract]
    public class Attachment
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
        /// FileUUID
        /// </summary>
        [DataMember(Name = "FileUUID")]
        public string FileUUID { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember(Name = "FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [DataMember(Name = "FileExtension")]
        public string FileExtension { get; set; }

        /// <summary>
        /// 文件Url
        /// </summary>
        [DataMember(Name = "Url")]
        public string Url { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        [DataMember(Name = "UploadUser")]
        public string UploadUser { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [DataMember(Name = "UploadTime")]
        public string UploadTime { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember(Name = "FileLength")]
        public int FileLength { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        [DataMember(Name = "DownloadTimes")]
        public int DownloadTimes { get; set; }

        /// <summary>
        /// 会议记录标志
        /// </summary>
        [DataMember(Name = "Flag")]
        public int Flag { get; set; }

        /// <summary>
        /// 下载记录
        /// </summary>
        [DataMember(Name = "Download")]
        public List<Download> Download { get; set; }
    }

    /// <summary>
    /// 下载记录
    /// </summary>
    public class Download
    {
        /// <summary>
        /// 下载人
        /// </summary>
        [DataMember(Name = "DownloadUser")]
        public string DownloadUser { get; set; }

        /// <summary>
        /// 下载时间
        /// </summary>
        [DataMember(Name = "DownloadTime")]
        public string DownloadTime { get; set; }
    }
}
