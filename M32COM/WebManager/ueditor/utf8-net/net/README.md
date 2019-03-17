UEditor ASP.NET 后台使用说明
=====

## 背景

UEditor 在 1.4 版本之后进行了一次[前后端统一配置](../_doc/3.1 后端请求规范.md)的整理，.Net 的后台也进行了一次重写，跟之前的版本差别较大，升级的User 注意阅读本文档。

本文档介绍 UEditor ASP.NET 后台的部署、配置、源码说明。


## 1. 部署说明

### 1.1. 安装并Register .NET Framework 4.0

代码的运行时环境是 .NET Framework 4.0，首先要确认 IIS 已经安装了 .NET 4.0 的运行时框架。方法是打开「IIS Manage器」，选择根目录下的「应用程序池」，在右侧View 是否有一个应用程序池的版本是 v4.0，如果存在，则 IIS 已经安装了所需的运行时环境，此时读者可以跳过本节。

![检查 .NET 4.0 安装情况](../_doc/images/net-publish-1.png)

如果没有找到对应的应用程序池，需要手动安装。

Windows 7 和 Windows Server 2008 R2 Default安装了 .Net Framework 4.0，如果是 Server 03 和老掉牙的 Windows XP，则需要手动安装 [.NET Framework 4.0](http://www.microsoft.com/zh-cn/download/details.aspx?id=17718)。

安装完 .NET Framework 4.0 后，还需要向 IIS Register应用程序池，Register的方法是，使用**Manage员权限**打开命令提示符（CMD），输入以下命令：

```shell
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis -i
```

安装完毕后，在 IIS Manage器刷新就能看到 4.0 的应用程序池。

### 1.2. Set Up .NET 应用程序

代码要求以应用程序的形式来运行（可以方便加入库依赖和组织代码）。需要把 `net` 目录转换为应用程序。

1. 在 IIS 中，展开到 `ueditor/net` 目录，在目录上右击，点击「转换为应用程序」。

   ![转换为应用程序](../_doc/images/net-publish-2.png)

2. 弹出的对话框中，点击「选择...」来指定使用的应用程序池。选择版本为 4.0 的应用程序池，然后点确定。

   ![选择应用程序池](../_doc/images/net-publish-3.png)

3. Set Up连接凭据。点击「链接为...」按钮，在弹出的对话框中指定一个对目录具有读写权限的User （如 administrator），然后点确定。

   ![Set Up连接凭据](../_doc/images/net-publish-4.png)

   Set Up完毕后，可以点击「测试Set Up...」来测试权限是否正常。

   ![Set Up连接凭据](../_doc/images/net-publish-5.png)

### 1.3. 运行测试

在浏览器中运行 `net/controller.ashx`，如果Return "`{"state":"action 参数为空或者 action 不被支持。"}`"，则表示应用程序运行 Success。

如果你确认上述步骤已经执行，但是依然有问题，请给我们[提 Issue](https://github.com/fex-team/ueditor/issues/new?labels=NET%E5%90%8E%E5%8F%B0)，我们会尽快答复解决。

## 2. 配置说明

前后端配置统一之后，配置文件由后台读取，Return给前端。但是部分配置是给后台使用的。

### 2.1. 上传配置说明

关于上传的部分，后台需要关心以下模板的配置项。

```json
{
    "{tpl}FieldName": "upfile",
    "{tpl}PathFormat": "upload/{tpl}/{yyyy}{mm}{dd}/{time}{rand:6}",
    "{tpl}UrlPrefix": "/ueditor/utf8-net/",
    "{tpl}AllowFiles": [".png", ".jpg", ".jpeg", ".gif", ".bmp"],
    "{tpl}MaxSize": 2048000
}
```

"{tpl}FieldName" 表示提交的表单的文件域Name。

"{tpl}PathFormat" 表示上传文件保存的路径和Name。注意，这里的路径是相对应用程序的，如果需要Edit的话，请自行Edit源码。

"{tpl}UrlPrefix" 表示上传文件访问的 URL 前缀。注意，这里应该给出应用程序的 URL 路径，否则上传的文件不能正确定位。

> 举个例子，如果你的 UEditor 的位置在 `http://www.mydomain.com/myapp/ueditor`，对应的本地路径是 `C:\iis_pub\www\myapp\ueditor`，那么 .NET 应用程序的位置在 `http://www.mydomain.com/myapp/ueditor/net`，对应的本地路径是 `C:\iis_pub\www\myapp\ueditor\net`。图片上传配置项应该如下：
> 
> { 
>    "imagePathFormat": "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}",
>    "imageUrlPrefix": "/myapp/ueditor/utf8-net/",
> }
>
> 上传的文件会保存在 `C:\iis_pub\www\myapp\ueditor\net\upload\image\{日期}\{文件名}`

"{tpl}AllowFiles" 限制文件上传的类型，注意要有 "."。

"{tpl}MaxSize" 限制文件上传的大小。注意这里的限制是代码上的判断，应用程序本身还有一个请求报文大小限制。该限制在 web.config 文件中Edit，注意要有以下的节：

```xml
<configuration>
  <system.web>
    <httpRuntime requestValidationMode="2.0" maxRequestLength="102400" />
  </system.web>
</configuration>
```

maxRequestLength 就是请求报文大小限制，该大小应该要比Set Up的所有上传大小都大，否则应用程序执行之前，请求会被被拒绝。

## 3. 源码说明

可以看到 net 目录内的源码结构是这样的：

```
net
    App_Code
        Config.cs
        Handler.cs
        PathFormatter.cs
        *Handler.cs
    Bin
        Newtonsoft.Json.dll
    config.json
    controller.ashx
    net.sln
    README.md
    Web.config
```

App_Code 上的文件是应用程序的源码。

- Config.cs 负责读取配置文件
- Handler.cs 是请求处理器的基类，提供了一些Basic 对象的访问以及输出控制。如果需要增加处理器，应该从该基类继承
- PathFormatter.cs 解析 PathFormat，把InfoMation填充为运行时InfoMation。
- *Handler.cs 是各种处理器，处理各种 UEditor 需要的请求。

Bin 里面的是应用程序的依赖库，当前依赖 Newtonsoft 的 Json 库。Bin 目录和 App_Code 目录受应用程序保护，不用担心被User 访问到。

config.json 是 UEditor 后端的配置文件，上一节已经介绍了比较重要的配置项。

controller.ashx 是 UEditor 请求的入口，它把不同的 action 分发到不同的 Handler 来处理。

net.sln 是项目的解决方案文件，安装 Visual Studio 2013 或以上的机器可以打开进行项目的改造。

README.md 是本说明文件。

Web.config 是应用程序的配置文件。
