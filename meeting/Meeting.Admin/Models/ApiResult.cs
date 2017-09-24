using System.Runtime.Serialization;

namespace Meeting.Admin.Models
{
    /// <summary>
    /// Api返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class ApiResult<T>
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Create(bool success, string message, T data)
        {
            return new ApiResult<T>() { Success = success, Message = message, Data = data };
        }

        /// <summary>
        /// 创建默认成功实例，Success=True,Message = "ok"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Create(T data)
        {
            return Create(true, "ok", data);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<T> Create(bool success, string message)
        {
            return new ApiResult<T>() { Message = message, Success = success };
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember(Name = "data")]
        public T Data { get; set; }
    }
}
