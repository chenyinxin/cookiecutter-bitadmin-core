/***********************
 * BitAdmin2.0框架文件
 ***********************/
using StackExchange.Redis;

namespace {{cookiecutter.project_name}}.Helpers
{
    public sealed class RedisHelper
    {
        public static string connectionString { get { return HttpContextCore.Configuration["ConnectionStrings:Redis"]; } }

        public static IDatabase redis { get; set; }
        static RedisHelper()
        {
            redis = ConnectionMultiplexer.Connect(connectionString).GetDatabase();
        }

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string key, string value)
        {
            return redis.StringSet(key, value);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return redis.StringGet(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteKey(string key)
        {
            return redis.KeyDelete(key);
        }
    }
}
