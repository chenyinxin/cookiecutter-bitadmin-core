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
        public string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", "mch_id", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        public string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        public string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
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
                if(string.IsNullOrEmpty(productId))
                    return Json(new { Code = 1, Msg = "产品不能为空！" });

                WxPayData data = new WxPayData();
                data.SetValue("appid", appid);
                data.SetValue("mch_id", mch_id);
                data.SetValue("time_stamp", GenerateTimeStamp());
                data.SetValue("nonce_str", GenerateNonceStr());
                data.SetValue("product_id", productId);
                data.SetValue("sign", data.MakeSign());

                return Json(new { Code = 0, Msg = "weixin://wxpay/bizpayurl?" + data.ToUrl(true) });
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
        public ActionResult NativePayTow(string productId,string body,string attach,string goods_tag,string total_fee)
        {
            try
            {
                if (string.IsNullOrEmpty(body))
                    return Json(new { Code = 1, Msg = "商品描述不能为空！" });
                if (string.IsNullOrEmpty(total_fee))
                    return Json(new { Code = 1, Msg = "金额不能为空！" });

                WxPayData data = new WxPayData();
                data.SetValue("trade_type", "NATIVE");              //交易类型
                data.SetValue("out_trade_no", GenerateOutTradeNo());//随机字符串
                data.SetValue("body", body);                        //商品描述
                data.SetValue("total_fee", total_fee);              //总金额
                data.SetValue("goods_tag", goods_tag);
                data.SetValue("product_id", productId);
                data.SetValue("attach", attach);
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));

                WxPayData result = UnifiedOrder(data);              //统一下单
                string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
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
            WxPayData data = new WxPayData();
            data.SetValue("auth_code", auth_code);  //用户出示授权码
            data.SetValue("body", body);            //商品描述
            data.SetValue("total_fee", int.Parse(total_fee));   //总金额
            data.SetValue("out_trade_no", GenerateOutTradeNo());//产生随机的商户订单号 
            data.SetValue("spbill_create_ip", ip);  //终端ip
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            data.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            data.SetValue("sign", data.MakeSign());//签名
            string xml = data.ToXml();

            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            string response = HttpService.Post(xml, url, false, 10);//调用HTTP通信接口以提交数据到API
            

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            
            //如果提交被扫支付接口调用失败，则抛异常
            if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
            {
                string returnMsg = result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "";
                throw new Exception("Micropay API interface call failure, return_msg : " + returnMsg);
            }

            //签名验证
            result.CheckSign();

            //刷卡支付直接成功
            if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return Content(result.ToPrintStr());
            }

            /******************************************************************
             * 剩下的都是接口调用成功，业务失败的情况
             * ****************************************************************/
            //1）业务结果明确失败
            if (result.GetValue("err_code").ToString() != "USERPAYING" &&
            result.GetValue("err_code").ToString() != "SYSTEMERROR")
            {
                return Content(result.ToPrintStr());
            }

            //2）不能确定是否失败，需查单
            //用商户订单号去查单
            string out_trade_no = data.GetValue("out_trade_no").ToString();

            //确认支付是否成功,每隔一段时间查询一次订单，共查询10次
            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                WxPayData queryResult = MicroPayQuery(out_trade_no, out int succResult);
                //如果需要继续查询，则等待2s后继续
                if (succResult == 2)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                else if (succResult == 1)
                {
                    //查询成功,返回订单查询接口返回的数据
                    return Content(queryResult.ToPrintStr());
                }
                else
                {
                    //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                    return Content(result.ToPrintStr());
                }
            }

            //确认失败，则撤销订单
            if (!MicroPayCancel(out_trade_no))
            {
                throw new Exception("Reverse order failure！");
            }

            return Content(result.ToPrintStr());
        }

        /**
	    * 
	    * 查询订单情况
	    * @param string out_trade_no  商户订单号
	    * @param int succCode         查询订单结果：0表示订单不成功，1表示订单成功，2表示继续查询
	    * @return 订单查询接口返回的数据，参见协议接口
	    */
        private WxPayData MicroPayQuery(string out_trade_no, out int succCode)
        {
            WxPayData queryOrderInput = new WxPayData();
            queryOrderInput.SetValue("out_trade_no", out_trade_no);
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            string appid = "WeChat_APPID";
            string MCHID = "WeChat_MCHID";

            queryOrderInput.SetValue("appid", appid);//公众账号ID
            queryOrderInput.SetValue("mch_id", MCHID);//商户号
            queryOrderInput.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            queryOrderInput.SetValue("sign", queryOrderInput.MakeSign());//签名

            string xml = queryOrderInput.ToXml();
            
            string response = HttpService.Post(xml, url, false, 10);//调用HTTP通信接口提交数据

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            if (result.GetValue("return_code").ToString() == "SUCCESS"
                && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //支付成功
                if (result.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    succCode = 1;
                    return result;
                }
                //用户支付中，需要继续查询
                else if (result.GetValue("trade_state").ToString() == "USERPAYING")
                {
                    succCode = 2;
                    return result;
                }
            }

            //如果返回错误码为“此交易订单号不存在”则直接认定失败
            if (result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
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

            WxPayData reverseInput = new WxPayData();
            reverseInput.SetValue("out_trade_no", out_trade_no);
            WxPayData result = Reverse(reverseInput);

            //接口调用失败
            if (result.GetValue("return_code").ToString() != "SUCCESS")
            {
                return false;
            }

            //如果结果为success且不需要重新调用撤销，则表示撤销成功
            if (result.GetValue("result_code").ToString() != "SUCCESS" && result.GetValue("recall").ToString() == "N")
            {
                return true;
            }
            else if (result.GetValue("recall").ToString() == "Y")
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
        public string JsApiPayOrder(string openid,int total_fee)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "test");
            data.SetValue("attach", "test");
            data.SetValue("out_trade_no", GenerateOutTradeNo());
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                throw new Exception("UnifiedOrder response error!");
            }

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", result.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + result.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            return JsonConvert.SerializeObject(jsApiParam.GetValues());
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
            WxPayData signData = new WxPayData();
            signData.SetValue("appid", appid);
            signData.SetValue("url", url);
            signData.SetValue("timestamp", GenerateTimeStamp());
            signData.SetValue("noncestr", GenerateNonceStr());
            signData.SetValue("accesstoken", access_token);
            string addrSign = EncryptHelper.SHA1(signData.ToUrl());

            //获取收货地址js函数入口参数
            WxPayData afterData = new WxPayData();
            afterData.SetValue("appId", appid);
            afterData.SetValue("scope", "jsapi_address");
            afterData.SetValue("signType", "sha1");
            afterData.SetValue("addrSign", addrSign);
            afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
            afterData.SetValue("nonceStr", signData.GetValue("noncestr"));
            
            return JsonConvert.SerializeObject(afterData.GetValues());
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
        public WxPayData Reverse(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new Exception("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, true, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }


        /**
        * 
        * 申请退款
        * @param WxPayData inputObj 提交给申请退款API的参数
        * @param int timeOut 超时时间
        * @throws Exception
        * @return 成功时返回接口调用结果，其他抛异常
        */
        public static WxPayData Refund(WxPayData inputObj, int timeOut = 30)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new Exception("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new Exception("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new Exception("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new Exception("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new Exception("退款申请接口中，缺少必填参数op_user_id！");
            }

            inputObj.SetValue("appid", "WeChat_APPID");//公众账号ID
            inputObj.SetValue("mch_id", "WeChat_MCHID");//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            string response = HttpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
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
        public WxPayData RefundQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            //检测必填参数
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new Exception("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }


        /**
        * 下载对账单
        * @param WxPayData inputObj 提交给下载对账单API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public WxPayData DownloadBill(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            //检测必填参数
            if (!inputObj.IsSet("bill_date"))
            {
                throw new Exception("对账单接口中，缺少必填参数bill_date！");
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API

            WxPayData result = new WxPayData();
            //若接口调用失败会返回xml格式的结果
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //接口调用成功则返回非xml格式的数据
            else
                result.SetValue("result", response);

            return result;
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
        public WxPayData ShortUrl(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            //检测必填参数
            if (!inputObj.IsSet("long_url"))
            {
                throw new Exception("需要转换的URL，签名用原串，传输需URL encode！");
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串	
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            string response = HttpService.Post(xml, url, false, timeOut);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }


        /**
        * 
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new Exception("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new Exception("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new Exception("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new Exception("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new Exception("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new Exception("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", "http://url");//异步通知url
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("spbill_create_ip", ip);//终端ip	  	    
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign());
            string response = HttpService.Post(inputObj.ToXml(), url, false, timeOut);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }


        /**
        * 
        * 关闭订单
        * @param WxPayData inputObj 提交给关闭订单API的参数
        * @param int timeOut 接口超时时间
        * @throws Exception
        * @return 成功时返回，其他抛异常
        */
        public WxPayData CloseOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new Exception("关闭订单接口中，out_trade_no必填！");
            }

            inputObj.SetValue("appid", appid);//公众账号ID
            inputObj.SetValue("mch_id", mch_id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串		
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            string response = HttpService.Post(xml, url, false, timeOut);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }

    }


    /// <summary>
    /// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
    /// 在调用接口之前先填充各个字段的值，然后进行接口通信，
    /// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
    /// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
    /// </summary>
    public class WxPayData
    {

        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        /**
        * 设置某个字段的值
        * @param key 字段名
         * @param value 字段值
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws Exception
        **/
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                throw new Exception("WxPayData数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
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

        /**
        * @将xml转为WxPayData对象并返回对象内部的数据
        * @param string 待转换的xml串
        * @return 经转换得到的Dictionary
        * @throws Exception
        */
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new Exception("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            try
            {
                //2015-06-29 错误是没有签名
                if (m_values["return_code"] != "SUCCESS")
                {
                    return m_values;
                }
                CheckSign();//验证签名,不通过会抛异常
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return m_values;
        }

        /**
        * @Dictionary格式转化成url参数格式
        * @ return url格式串, 该串不包含sign字段值
        */
        public string ToUrl(bool hasSign = false)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if ((pair.Key != "sign" || hasSign) && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        /**
        * @values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
        */
        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    throw new Exception("WxPayData内部含有值为null的字段!");
                }

                str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
            }
            return str;
        }

        /**
        * @生成签名，详见签名生成算法
        * @return 签名, sign字段不参加签名
        */
        public string MakeSign()
        {
            string appid = "WeChat_APIKEY";
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + appid;
            //MD5加密
            return EncryptHelper.MD5(str).ToUpper();
        }        

        /**
        * 
        * 检测签名是否正确
        * 正确返回true，错误抛异常
        */
        public bool CheckSign()
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                throw new Exception("WxPayData签名存在但不合法!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                throw new Exception("WxPayData签名存在但不合法!");
            }

            //获取接收到的签名
            string return_sign = GetValue("sign").ToString();

            //在本地计算新的签名
            string cal_sign = MakeSign();

            if (cal_sign == return_sign)
            {
                return true;
            }
            
            throw new Exception("WxPayData签名验证错误!");
        }

        /**
        * @获取Dictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }
    }
    /**
    * 	配置账号信息
    */

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
