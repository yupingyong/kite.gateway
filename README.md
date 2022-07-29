# Kite Api网关

#### 介绍
基于微软反向代理组件Yarp开发的Api网关


#### 软件架构
开发框架 .Net 6 + Abp Vnext 5.3.3 + Blazor(UI框架:BootstrapBlazor)
![输入图片说明](%E5%9B%BE%E7%89%87.png)
admin 目录为网关管理后台
simples 目录为测试服务
src 目录为网关项目


Kite.Gateway.Admin : 
    后台管理项目,实现对网关服务节点的管理以及网关配置数据的管理,支持像多个节点同步刷新配置数据 

Kite.Gateway.Hosting : 
    网关启动项目,定义了网关过滤器以及中间件,依赖于领域服务层以及仓储实现层

Kite.Gateway.Application : 
    应用服务层,组合业务逻辑层业务,提交数据库保存,依赖于领域服务层以及应用服务公共连接层

Kite.Gateway.Application.Contracts : 
    应用服务公共连接层,定义应用服务层接口,DTO对象

Kite.Gateway.Domain: 
    领域服务层,业务逻辑处理核心层,依赖于领域服务共享层

Kite.Gateway.Domain.Shared : 
    领域服务共享层,定义公共的枚举,通用工具类等

Kite.Gateway.EntityFrameworkCore : 
    仓储实现层,依赖于领域服务,基于EF Core实现 

#### 使用说明

#### 参与贡献

1.  Fork 本仓库
2.  新建 dev_xxx 分支
3.  提交代码
4.  新建 Pull Request

