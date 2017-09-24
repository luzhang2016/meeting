namespace Meeting.Admin.Models
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        public string connectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string dbName { get; set; }

        /// <summary>
        /// 获取组织树的API地址
        /// </summary>
        public string outerAPIUrl { get; set; }

        /// <summary>
        /// serverID
        /// </summary>
        public string serverID { get; set; }
    }
}
