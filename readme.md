WeiXinMP-OSGi.NET
=================

基于微信公众平台（WeiXinMPSDK）的 OSGi.NET 插件化版

WeiXinMPSDK： https://github.com/JeffreySu/WeiXinMPSDK/

OSGi.NET： http://www.iopenworks.com/


说明
---------

本程序只是通过 OSGi.NET 免费插件化开发框架将 WeiXinMPSDK 做了简单封装，没有更改任何 WeiXinMPSDK 中的代码，实际上只是引用了它的 Senparc.Weixin.MP.dll。

这样可以通过简单的xml配置，来适应不同场景的逻辑需求。达到可复用，易部署，快速组合的目的。

非常感谢原作者的努力和无私贡献！

运行 OSGi.NET 需要安装 iOpenWorks SDK，可从这里下载安装 http://iopenworks.com/Products/SDKDownload （程序的 bin 目录已经带有所需 dll，也可以自行更改引用）。


使用方法
---------

1、如何通过基础接口，使得公众号能够接收用户消息，并按照自己的需要向其回复消息？

可参考 UIShell.WeChatShell\Plugins\UIShell.iOpenWorksHelpPlugin 的实例

Manifest.xml

```xml
	......
	
	<Extension Point="UIShell.WeChatProxy">
		<Proxy Name="iOpenWorksHelp" Token="iopenworks_help" 
		Handler="UIShell.iOpenWorksHelpPlugin.CustomMessageHandler.CustomMessageHandler" 
		AppId="YOUR_APPID" Secret="YOUR_SECRET"/>
	</Extension>

	......
```

Token 则为服务器配置用于接收用户信息的 Token 值

CustomMessageHandler（以及CustomMessageHandler_Events） 则为实际的处理逻辑模块。CustomMessageHandler 继承自 MessageHandler<CustomMessageContext>，其中 CustomMessageContext 继承自 MessageContext。

将 AppId 和 Secret 替换为自己的。

2、如何添加公众号底部的自定义菜单？

可参考 UIShell.WeChatShell\Plugins\UIShell.iOpenWorksHelpPlugin 的实例

Manifest.xml

```xml
	......
	
	<Extension Point="UIShell.WeChatMenu">
		<Menu Name="iOpenWorksHelp" AppId="YOUR_APPID" Secret="YOUR_SECRET">
			<Button Type="Click" Name="今日歌曲" Key="V1001_TODAY_MUSIC" />
			<Button Type="Click" Name="歌手简介" Key="V1001_TODAY_SINGER" />
			<Button Name="菜单">
				<Button Type="View" Name="搜索" Url="http://www.soso.com/" />
				<Button Type="View" Name="视频" Url="http://v.qq.com/" />
				<Button Type="Click" Name="赞一下我们" Key="V1001_GOOD" />
			</Button>
		</Menu>
	</Extension>

	......
```

将 AppId 和 Secret 替换为自己的，然后根据需要编辑下面的 Button 节点定义，注意格式是否正确，并在首页传递参数 updatemenu，例如 updatemenu=1，完整 URL 如 http://localhost/?updatemenu=1，可实现自动更新菜单。

更多关于 OSGi.NET 的用法请参考官网帮助文档： http://www.iopenworks.com/Documents/DocumentsList 