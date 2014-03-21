WeiXinMP-OSGi.NET
=================

基于微信公众平台（WeiXinMPSDK）的OSGi.NET插件化版

WeiXinMPSDK： https://github.com/JeffreySu/WeiXinMPSDK/

OSGi.NET： http://www.iopenworks.com/


说明
---------

本程序只是通过OSGi.NET免费插件化开发框架将 WeiXinMPSDK 做了简单封装，没有更改任何 WeiXinMPSDK 中的代码，实际上只是引用了它的 Senparc.Weixin.MP.dll。

这样可以通过简单的xml配置，来适应不同场景的逻辑需求。达到可复用，易部署，快速组合的目的。

非常感谢原作者的努力和无私贡献！


使用方法
---------

可参考 UIShell.WeChatShell\Plugins\UIShell.iOpenWorksHelpPlugin 的实例

Manifest.xml

```xml
	......
	
	<Extension Point="UIShell.WeChatProxy">
		<Proxy Name="iOpenWorksHelp" Token="iopenworks_help" Handler="UIShell.iOpenWorksHelpPlugin.CustomMessageHandler.CustomMessageHandler" />
	</Extension>

	......
```

Token 则为微信的Token名

CustomMessageHandler 继承自 MessageHandler<CustomMessageContext>，其中 CustomMessageContext 继承自 MessageContext

更多关于 OSGi.NET 的用法请参考官网帮助文档： http://www.iopenworks.com/Documents/DocumentsList