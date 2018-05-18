/***********************
 * BitAdmin2.0框架文件
 ***********************/
using StackExchange.Redis;

namespace {{cookiecutter.project_name}}.Helpers
{
    public sealed class RedisHelper
    {
        public static string ConnectionString { get { return HttpContextCore.Configuration["ConnectionStrings:Redis"]; } }

        public static IDatabase Connection { get; set; }
        static RedisHelper()
        {
            Connection = ConnectionMultiplexer.Connect(ConnectionString).GetDatabase();
        }

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string key, string value)
        {
            return Connection.StringSet(key, value);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Connection.StringGet(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteKey(string key)
        {
            return Connection.KeyDelete(key);
        }
    }
}
