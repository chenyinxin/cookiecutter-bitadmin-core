/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace {{cookiecutter.project_name}}.Controllers
{
    /// <summary>
    /// 支付服务：(有点复杂，暂时没有测试账号，后续完善)
    /// 微信支付（JsApi：公众号支付；Native：扫码支付；MicroPay：刷卡支付；APP：APP支付（未实现）；）
    /// 官方文档：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=2_1
    /// </summary>
    public class PayController : Controller
    {

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * appid：绑定支付的APPID（必须配置）
        * mch_id：商户号（必须配置）
        * key：商户支付密钥，参考开户邮件设置（必须配置）
        * secret：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        string appid = "appid";
        string mch_id = "mch_id";
        string key = "key";
        string secret = "secret";

        string ip = "8.8.8.8";

        /// <summary>
        /// 扫码支付（模式一）
        /// 官方文档：https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=6_1
        /// </summary>
        /// <returns></returns>
        public ActionResult NativePayOne(string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(productId))
                    return Json(new { Code = 1, Msg = "产品不能为空！" });

                SortedDictionary<string, object> data = new SortedDictionary<string, object>
                {
                    { "appid", appid },
                    { "mch_id", mch_id },
                    { "time_stamp", PayHelper.Time_stamp },
                    { "nonce_str", PayHelper.Nonce_str },
                    { "product_id", productId }
                };
                data.Add("sign", PayHelper.MakeSign(data));

                return Json(new { Code = 0, Msg = "weixin://wxpay/bizpayurl?" + PayHelper.ToUrl(data) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 微信支付NativePayOne回调地址
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult NativePayOneCallback(string productId)
        {
            try
            {
                return Json(new { Code = 0, Msg = "成功" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 扫码支付（模式二）
        /// 官方文档：https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=6_1
        /// </summary>
        /// <returns></returns>
        public ActionResult NativePayTow(string productId, string body, string attach, string goods_tag, string total_fee)
        {
            try
            {
                if (string.IsNullOrEmpty(body))
                    return Json(new { Code = 1, Msg = "商品描述不能为空！" });
                if (string.IsNullOrEmpty(total_fee))
                    return Json(new { Code = 1, Msg = "金额不能为空！" });

                SortedDictionary<string, object> data = new SortedDictionary<string, object>
                {
                    { "trade_type", "NATIVE" },                     //交易类型
                    { "out_trade_no", PayHelper.Out_trade_no },     //随机字符串
                    { "body", body },                               //商品描述
                    { "total_fee", total_fee },                     //总金额
                    { "goods_tag", goods_tag },
                    { "product_id", productId },
                    { "attach", attach },
                    { "time_start", DateTime.Now.ToString("yyyyMMddHHmmss") },
                    { "time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss") }
                };

                var result = UnifiedOrder(data);                //统一下单
                string url = result["code_url"].ToString();     //获得统一下单接口返回的二维码链接
                return Json(new { Code = 0, Msg = url });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /**
        * 刷卡支付完整业务流程逻辑
        * @param body 商品描述
        * @param total_fee 总金额
        * @param auth_code 支付授权码
        * @throws WxPayException
        * @return 刷卡支付结果
        */
        public ActionResult MicroPay(string body, string total_fee, string auth_code)
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>
            {
                { "auth_code", auth_code },                 //用户出示授权码
                { "body", body },                           //商品描述
                { "total_fee", int.Parse(total_fee) },      //总金额
                { "out_trade_no", PayHelper.Out_trade_no }, //产生随机的商户订单号 
                { "spbill_create_ip", ip },                 //终端ip
                { "appid", appid },
                { "mch_id", mch_id },
                { "nonce_str", PayHelper.Nonce_str }//随机字符串
            };
            data.Add("sign", PayHelper.MakeSign(data));//签名

            string xml = PayHelper.ToXml(data);
            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            string response = HttpService.Post(xml, url, false, 10);//调用HTTP通信接口以提交数据到API


            //将xml格式的结果转换为对象以返回
            SortedDictionary<string, object> result = PayHelper.FromXml(response);

            //如果提交被扫支付接口调用失败，则抛异常
            if (!result.ContainsKey("return_code") || result["return_code"].ToString() == "FAIL")
            {
                string returnMsg = result.ContainsKey("return_msg") ? result["return_msg"].ToString() : "";
                throw new Exception("Micropay API interface call failure, return_msg : " + returnMsg);
            }

            //签名验证
            result.CheckSign();

            //刷卡支付直接成功
            if (result["return_code"].ToString() == "SUCCESS" &&
                result["result_code"].ToString() == "SUCCESS")
            {
                return Json(result);
            }

            /******************************************************************
             * 剩下的都是接口调用成功，业务失败的情况
             * ****************************************************************/
            //1）业务结果明确失败
            if (result["err_code"].ToString() != "USERPAYING" &&
            result["err_code"].ToString() != "SYSTEMERROR")
            {
                return Json(result);
            }

            //2）不能确定是否失败，需查单
            //用商户订单号去查单
            string out_trade_no = data["out_trade_no"].ToString();

            //确认支付是否成功,每隔一段时间查询一次订单，共查询10次
            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                SortedDictionary<string, object> queryResult = MicroPayQuery(out_trade_no, out int succResult);
                //如果需要继续查询，则等待2s后继续
                if (succResult == 2)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                else if (succResult == 1)
                {
                    //查询成功,返回订单查询接口返回的数据
                    return Json(queryResult);
                }
                else
                {
                    //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                    return Json(result);
                }
            }

            //确认失败，则撤销订单
            if (!MicroPayCancel(out_trade_no))
            {
                throw new Exception("Reverse order failure！");
            }

            return Json(result);
        }

        /**
	    * 
	    * 查询订单情况
	    * @param string out_trade_no  商户订单号
	    * @param int succCode         查询订单结果：0表示订单不成功，1表示订单成功，2表示继续查询
	    * @return 订单查询接口返回的数据，参见协议接口
	    */
        private SortedDictionary<string, object> MicroPayQuery(string out_trade_no, out int succCode)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            string appid = "WeChat_APPID";
            string MCHID = "WeChat_MCHID";

            SortedDictionary<string, object> data = new SortedDictionary<string, object>
            {
                { "out_trade_no", out_trade_no },
                { "appid", appid },//公众账号ID
                { "mch_id", MCHID },//商户号
                { "nonce_str", PayHelper.Nonce_str }//随机字符串
            };
            data.Add("sign", PayHelper.MakeSign(data));//签名

            string xml = data.ToXml();
            string response = HttpService.Post(xml, url, false, 10);//调用HTTP通信接口提交数据

            //将xml格式的数据转化为对象以返回
           var result = PayHelper.FromXml(response);


            if (result["return_code"].ToString() == "SUCCESS"&& result["result_code"].ToString() == "SUCCESS")
            {
                //支付成功
                if (result["trade_state"].ToString() == "SUCCESS")
                {
                    succCode = 1;
                    return result;
                }
                //用户支付中，需要继续查询
                else if (result["trade_state"].ToString() == "USERPAYING")
                {
                    succCode = 2;
                    return result;
                }
            }

            //如果返回错误码为“此交易订单号不存在”则直接认定失败
            if (result["err_code"].ToString() == "ORDERNOTEXIST")
            {
                succCode = 0;
            }
            else
            {
                //如果是系统错误，则后续继续
                succCode = 2;
            }
            return result;
        }


        /**
	    * 
	    * 撤销订单，如果失败会重复调用10次
	    * @param string out_trade_no 商户订单号
	    * @param depth 调用次数，这里用递归深度表示
        * @return false表示撤销失败，true表示撤销成功
	    */
        private bool MicroPayCancel(string out_trade_no, int depth = 0)
        {
            if (depth > 10)
            {
                return false;
            }

            SortedDictionary<string, object> reverseInput = new SortedDictionary<string, object>();
            reverseInput.Add("out_trade_no", out_trade_no);
            SortedDictionary<string, object> result = Reverse(reverseInput);

            //接口调用失败
            if (result["return_code"].ToString() != "SUCCESS")
            {
                return false;
            }

            //如果结果为success且不需要重新调用撤销，则表示撤销成功
            if (result["result_code"].ToString() != "SUCCESS" && result["recall"].ToString() == "N")
            {
                return true;
            }
            else if (result["recall"].ToString() == "Y")
            {
                return MicroPayCancel(out_trade_no, ++depth);
            }
            return false;
        }

        /**
       * 
       * 网页授权获取用户基本信息的全部过程
       * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
       * 第一步：利用url跳转获取code
       * 第二步：利用code去获取openid和access_token
       * 
       */
        public ActionResult JsApiPayToken(string code)
        {
            //网页授权获取code的url
            //string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect", appid, "JsApiPayToken");
            if (string.IsNullOrEmpty(code))
                return Json(new { Code = 1, Msg = "参数错误！" });
            WebClient wc = new WebClient();
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code);
            string result = wc.DownloadString(url);
            var json = JObject.Parse(result);

            string access_token = (string)json["access_token"];
            string openid = (string)json["openid"];

            return Redirect("/pages/account/bind.html?sign=wx&openid=" + openid);
        }

        /// <summary>
        /// 统一下单，返回微信浏览器支付参数
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="total_fee">金额</param>
        /// <returns></returns>
        public SortedDictionary<string, object> JsApiPayOrder(string openid, int total_fee)
        {
            //统一下单
            SortedDictionary<string, object> data = new SortedDictionary<string, object>
            {
                { "body", "test" },
                { "attach", "test" },
                { "out_trade_no", PayHelper.Out_trade_no },
                { "total_fee", total_fee },
                { "time_start", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss") },
                { "goods_tag", "test" },
                { "trade_type", "JSAPI" },
                { "openid", openid }
            };

            var result = UnifiedOrder(data);
            if (!result.ContainsKey("appid") || !result.ContainsKey("prepay_id") || result["prepay_id"].ToString() == "")
            {
                throw new Exception("UnifiedOrder response error!");
            }

            SortedDictionary<string, object> jsApiParam = new SortedDictionary<string, object>
            {
                { "appId", result["appid"] },
                { "timeStamp", PayHelper.Time_stamp },
                { "nonceStr", PayHelper.Nonce_str },
                { "package", "prepay_id=" + result["prepay_id"] },
                { "signType", "MD5" }
            };
            jsApiParam.Add("paySign", PayHelper.MakeSign(jsApiParam));

            return jsApiParam;
        }

        /**
	    * 
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */
        public string JsApiPayAddress(string access_token)
        {
            //参与签名的是网页授权获取用户信息时微信后台回传的完整url
            string url = "http://redirect_uri";

            //构造需要用SHA1算法加密的数据
            SortedDictionary<string, object> data = new SortedDictionary<string, object>
            {
                { "appid", appid },
                { "url", url },
                { "timestamp", PayHelper.Time_stamp },
                { "noncestr", PayHelper.Nonce_str },
                { "accesstoken", access_token }
            };
            string addrSign = EncryptHelper.SHA1(PayHelper.ToUrl(data));

            //获取收货地址js函数入口参数
            SortedDictionary<string, object> afterData = new SortedDictionary<string, object>
            {
                { "appId", appid },
                { "scope", "jsapi_address" },
                { "signType", "sha1" },
                { "addrSign", addrSign },
                { "timeStamp", data["timestamp"] },
                { "nonceStr", data["noncestr"] }
            };

            return JsonConvert.SerializeObject(afterData);
        }
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

        /**
        * 
        * 撤销订单API接口
        * @param WxPayData inputObj 提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回API调用结果，其他抛异常
        */
        public SortedDictionary<string, object> Reverse(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("out_trade_no") && !inputObj.ContainsKey("transaction_id"))
            {
                throw new Exception("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("nonce_str", PayHelper.Nonce_str);//随机字符串
            inputObj.Add("sign", PayHelper.MakeSign(inputObj));//签名
            string xml = inputObj.ToXml();            
            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/secapi/pay/reverse", true, timeOut);            

            return PayHelper.FromXml(response);
        }


        /**
        * 
        * 申请退款
        * @param WxPayData inputObj 提交给申请退款API的参数
        * @param int timeOut 超时时间
        * @throws Exception
        * @return 成功时返回接口调用结果，其他抛异常
        */
        public static SortedDictionary<string, object> Refund(SortedDictionary<string, object> inputObj, int timeOut = 30)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("out_trade_no") && !inputObj.ContainsKey("transaction_id"))
            {
                throw new Exception("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.ContainsKey("out_refund_no"))
            {
                throw new Exception("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.ContainsKey("total_fee"))
            {
                throw new Exception("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.ContainsKey("refund_fee"))
            {
                throw new Exception("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.ContainsKey("op_user_id"))
            {
                throw new Exception("退款申请接口中，缺少必填参数op_user_id！");
            }

            inputObj.Add("appid", "WeChat_APPID");//公众账号ID
            inputObj.Add("mch_id", "WeChat_MCHID");//商户号
            inputObj.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.Add("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/secapi/pay/refund", true, timeOut);//调用HTTP通信接口提交数据到API

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时
            
            return PayHelper.FromXml(response);
        }


        /**
        * 
        * 查询退款
        * 提交退款申请后，通过该接口查询退款状态。退款有一定延时，
        * 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        * out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
        * @param WxPayData inputObj 提交给查询退款API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public SortedDictionary<string, object> RefundQuery(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("out_refund_no") && !inputObj.ContainsKey("out_trade_no") &&
                !inputObj.ContainsKey("transaction_id") && !inputObj.ContainsKey("refund_id"))
            {
                throw new Exception("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("nonce_str", PayHelper.Nonce_str);//随机字符串
            inputObj.Add("sign", PayHelper.MakeSign(inputObj));//签名

            string xml = inputObj.ToXml();   
            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/pay/refundquery", false, timeOut);//调用HTTP通信接口以提交数据到API

            return PayHelper.FromXml(response);
        }


        /**
        * 下载对账单
        * @param WxPayData inputObj 提交给下载对账单API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public SortedDictionary<string, object> DownloadBill(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("bill_date"))
            {
                throw new Exception("对账单接口中，缺少必填参数bill_date！");
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("nonce_str", PayHelper.Nonce_str);//随机字符串
            inputObj.Add("sign", PayHelper.MakeSign(inputObj));//签名

            string xml = inputObj.ToXml();
            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/pay/downloadbill", false, timeOut);//调用HTTP通信接口以提交数据到API
            
            if (response.Substring(0, 5) == "<xml>")
                return PayHelper.FromXml(response);
            else
                return new SortedDictionary<string, object>() { { "result", response } };

        }


        /**
        * 
        * 转换短链接
        * 该接口主要用于扫码原生支付模式一中的二维码链接转成短链接(weixin://wxpay/s/XXXXXX)，
        * 减小二维码数据量，提升扫描速度和精确度。
        * @param WxPayData inputObj 提交给转换短连接API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public SortedDictionary<string, object> ShortUrl(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("long_url"))
            {
                throw new Exception("需要转换的URL，签名用原串，传输需URL encode！");
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("nonce_str", PayHelper.Nonce_str);//随机字符串	
            inputObj.Add("sign", PayHelper.MakeSign(inputObj));//签名
            string xml = inputObj.ToXml();
            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/tools/shorturl", false, timeOut);
            return PayHelper.FromXml(response); ;
        }


        /**
        * 
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public SortedDictionary<string, object> UnifiedOrder(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("out_trade_no"))
            {
                throw new Exception("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.ContainsKey("body"))
            {
                throw new Exception("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.ContainsKey("total_fee"))
            {
                throw new Exception("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.ContainsKey("trade_type"))
            {
                throw new Exception("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj["trade_type"].ToString() == "JSAPI" && !inputObj.ContainsKey("openid"))
            {
                throw new Exception("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj["trade_type"].ToString() == "NATIVE" && !inputObj.ContainsKey("product_id"))
            {
                throw new Exception("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.ContainsKey("notify_url"))
            {
                inputObj.Add("notify_url", "http://url");//异步通知url
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("spbill_create_ip", ip);//终端ip	  	    
            inputObj.Add("nonce_str", PayHelper.Nonce_str);//随机字符串

            inputObj.Add("sign", PayHelper.MakeSign(inputObj));
            string response = HttpService.Post(PayHelper.ToXml(inputObj), "https://api.mch.weixin.qq.com/pay/unifiedorder", false, timeOut);            

            return PayHelper.FromXml(response);
        }


        /**
        * 
        * 关闭订单
        * @param WxPayData inputObj 提交给关闭订单API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public SortedDictionary<string, object> CloseOrder(SortedDictionary<string, object> inputObj, int timeOut = 6)
        {
            //检测必填参数
            if (!inputObj.ContainsKey("out_trade_no"))
            {
                throw new Exception("关闭订单接口中，out_trade_no必填！");
            }

            inputObj.Add("appid", appid);//公众账号ID
            inputObj.Add("mch_id", mch_id);//商户号
            inputObj.Add("nonce_str", PayHelper.Nonce_str); //随机字符串		
            inputObj.Add("sign", PayHelper.MakeSign(inputObj));//签名

            string xml = PayHelper.ToXml(inputObj);
            string response = HttpService.Post(xml, "https://api.mch.weixin.qq.com/pay/closeorder", false, timeOut);

            return PayHelper.FromXml(response);
        }
    }

    public static class PayHelper
    {
        public static string Out_trade_no => string.Format("{0}{1}{2}", "mch_id", DateTime.Now.ToString("yyyyMMddHHmmss"), new Random().Next(999));
        public static string Time_stamp => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
        public static string Nonce_str => Guid.NewGuid().ToString().Replace("-", "");

        public static string MakeSign(this SortedDictionary<string, object> data)
        {
            return EncryptHelper.MD5(ToUrl(data)).ToUpper();
        }
        public static string ToUrl(SortedDictionary<string, object> data)
        {            
            string buff = "";
            foreach (var pair in data)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            return buff.Trim('&');
        }
        public static string ToXml(this SortedDictionary<string, object> data)
        {
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in data)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    throw new Exception("WxPayData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    throw new Exception("WxPayData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }
        public static SortedDictionary<string, object> FromXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNodeList nodes = xmlDoc.FirstChild.ChildNodes;

            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                data[xe.Name] = xe.InnerText;
            }

            if (data["return_code"].ToString() != "SUCCESS")
                return data;

            if (!CheckSign(data)) throw new Exception("签名较验不通过：" + JsonConvert.SerializeObject(data));
            return data;
        }
        public static bool CheckSign(this SortedDictionary<string, object> data)
        {
            if (!data.ContainsKey("sign"))
                return false;
            if (string.IsNullOrEmpty(data["sign"].ToString()))
                return false;
            
            return MakeSign(data) == data["sign"].ToString();
        }
    }

    public class HttpService
    {
        public static string Post(string xml, string url, bool isUseCert, int timeout)
        {            
            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = timeout * 1000;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "text/xml";
            byte[] data = Encoding.UTF8.GetBytes(xml);
            request.ContentLength = data.Length;

            if (isUseCert)
            {
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cert\\apiclient_cert.p12");
                X509Certificate2 cert = new X509Certificate2(file, "1480670932");
                request.ClientCertificates.Add(cert);
            }

            var reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            //获取服务端返回
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();            
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return sr.ReadToEnd().Trim();
            }
        }
    }
}
