using Senparc.Weixin.MP;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.CommonAPIs;
using Newtonsoft;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;
using System.Collections.Generic;
using System.Text;

namespace UIShell.WeChatProxyPlugin
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            string inputXml = string.Empty;
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                inputXml = sr.ReadToEnd();
                inputXml = HttpUtility.UrlDecode(inputXml);
            }

            string updateMenu = Request["updatemenu"];

            if (!string.IsNullOrWhiteSpace(updateMenu))
            {
                foreach (var menu in Activator.WeChatMenuContainer.WeChatMenus)
                {
                    try
                    {
                        if (!AccessTokenContainer.CheckRegistered(menu.AppId))
                        {
                            AccessTokenContainer.Register(menu.AppId, menu.Secret);
                        }

                        AccessTokenResult tokenRes = null;
                        try
                        {
                            tokenRes = AccessTokenContainer.GetTokenResult(menu.AppId); //CommonAPIs.CommonApi.GetToken(appId, appSecret);
                            WriteContent(string.Format("获取到 token 为：{0}, 有效时间为 {1} 秒。", tokenRes.access_token, tokenRes.expires_in));

                            //var menuRes = CommonApi.GetMenu(tokenRes.access_token);
                        }
                        catch
                        {
                            WriteContent(string.Format("获取到 token 失败， appid: {0}，secret: {1}。", menu.AppId, menu.Secret));
                        }

                        try
                        {
                            if (tokenRes != null)
                            {
                                //重新整理按钮信息
                                ButtonGroup bg = new ButtonGroup();
                                foreach (var menuButton in menu.MenuButtons)
                                {
                                    BaseButton but = null;
                                    switch (menuButton.Type)
                                    {
                                        case ButtonType.Click:
                                            but = new SingleClickButton() { name = menuButton.Name, key = menuButton.Key, type = "click" };
                                            break;
                                        case ButtonType.View:
                                            but = new SingleViewButton() { name = menuButton.Name, url = menuButton.Url, type = "view" };
                                            break;
                                        case ButtonType.SubButton:
                                            List<SingleButton> subButtons = new List<SingleButton>();

                                            foreach (var subBut in menuButton.MenuSubButtons)
                                            {
                                                SingleButton singleBut = null;
                                                switch (subBut.Type)
                                                {
                                                    case ButtonType.Click:
                                                        singleBut = new SingleClickButton() { name = subBut.Name, key = subBut.Key, type = "click" };
                                                        break;
                                                    case ButtonType.View:
                                                        singleBut = new SingleViewButton() { name = subBut.Name, url = subBut.Url, type = "view" };
                                                        break;
                                                }

                                                if (singleBut != null)
                                                    subButtons.Add(singleBut);
                                            }

                                            but = new SubButton() { name = menuButton.Name, sub_button = subButtons };
                                            break;
                                    }

                                    if (but != null)
                                        bg.button.Add(but);
                                }

                                var result = CommonApi.CreateMenu(tokenRes.access_token, bg);
                                WriteContent(string.Format("创建结果信息：{0}, 返回值 {1} （{2}）。", result.errmsg, (int)result.errcode, result.errcode.ToString()));
                            }
                        }
                        catch
                        {
                            WriteContent("创建菜单失败！");
                        }
                    }
                    catch (Exception)
                    {
                        //TODO:为简化代码，这里不处理异常（如Token过期）
                        WriteContent("执行过程发生错误！");
                    }
                }
            }

            foreach (var proxy in Activator.WeChatProxyContainer.WeChatProxies)
            {
                string token = proxy.Token;

                if (Request.HttpMethod == "GET")
                {
                    //get method - 仅在微信后台填写URL验证时触发
                    if (CheckSignature.Check(signature, timestamp, nonce, token))
                    {
                        WriteContent(echostr); //返回随机字符串则表示验证通过
                        //如果有多个相同的Token，则第一个验证通过就返回
                        break;
                    }
                    else
                    {
                        //WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, token) + "。" +
                        //            "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                        //如果失败应该不返回任何东西，以便循环校验下一个Token
                        continue;
                    }
                }
                else
                {
                    //post method - 当有用户想公众账号发送消息时触发
                    if (!CheckSignature.Check(signature, timestamp, nonce, token))
                    {
                        WriteContent("参数错误！");
                        continue;
                    }

                    //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                    var maxRecordCount = 10;

                    //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                    //var messageHandler = new CustomMessageHandler(Request.InputStream, maxRecordCount);

                    IMessageHandler messageHandler = null;
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputXml)))
                    {
                        Type type = proxy.Bundle.LoadClass(proxy.Handler);
                        var parameters = new object[] { stream, token, proxy.AppId, proxy.Secret, maxRecordCount };
                        messageHandler = System.Activator.CreateInstance(type, parameters) as IMessageHandler;
                    }

                    try
                    {
                        if (messageHandler != null)
                        {
                            //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                            messageHandler.RequestDocument.Save(
                                Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                                               messageHandler.RequestMessage.FromUserName + ".txt"));
                            //执行微信处理过程
                            messageHandler.Execute();
                            //测试时可开启，帮助跟踪数据
                            messageHandler.ResponseDocument.Save(
                                Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" +
                                               messageHandler.ResponseMessage.ToUserName + ".txt"));
                            WriteContent(messageHandler.ResponseDocument.ToString());

                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                        {
                            tw.WriteLine(ex.Message);
                            tw.WriteLine(ex.InnerException.Message);

                            if (messageHandler.ResponseDocument != null)
                            {
                                tw.WriteLine(messageHandler.ResponseDocument.ToString());
                            }

                            tw.Flush();
                            tw.Close();
                        }

                        WriteContent("");
                    }
                }
            }

            Response.End();
        }

        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }

    }
}
