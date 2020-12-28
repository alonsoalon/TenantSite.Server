# 介绍
> Base on netcore3.x，建议开发IDE Visual Studio 2019 <br />

企业应用管理系统，定位于企业应用的SaaS服务框架，企业云端应用的基础开发框架（当然也可以部署于本地），系统被设计用于多租户，采用前端后端完全分离技术方案。
抽离企业应用软件研发公共部分，让研发人员有条件聚焦在业务研发，实现了用于权限管理的基础数据维护，权限赋权，缓存，上传等常规功能。 <br />

UI项目见：
* github:[https://github.com/alonsoalon/TenantSite.UI](https://github.com/alonsoalon/TenantSite.UI)
* gitee:[https://gitee.com/alonsoalon/TenantSite.UI](https://gitee.com/alonsoalon/TenantSite.UI)

## Docs 
* 更多文档见：[www.iusaas.com](http://www.iusaas.com)

## DEMO
[租户1示例](http://tenant1.iusaas.com/login) <br />
[租户2示例](http://tenant2.iusaas.com/login) <br />

![image](http://www.iusaas.com/intro1.jpg)
![image](http://www.iusaas.com/intro3.jpg)

## 目录结构
解决方案目录结构如下：
```
TenantSite.Server 
├── 1-Presentation
│   ├── AlonsoAdmin.HttpApi // TenantSite.UI 的后台API项目
│   └── AlonsoAdmin.HttpApi.Init // TenantSite.Developer.Tools 的后台API项目。
├── 2-Application
│   ├── AlonsoAdmin.Services // 服务层
├── 3-Domain
│   └── AlonsoAdmin.Domain // 领域层，用于编写针对领域的逻辑函数（如事务处理等）
└── 4-Infrastructure
    ├── AlonsoAdmin.Common // Common 常用工具、用户、配置、前后端规约实体等核心类库项目
    ├── AlonsoAdmin.Entities // 数据库实体类库项目
    ├── AlonsoAdmin.MultiTenant // 多租户类库项目
    └── AlonsoAdmin.Repository // 数据仓储类库项目

```

## 配置说明
> 建议刚上手时使用开发人员工具进行配置。

HttpApi项目配置文件及说明如下：
```javascript
{
  // 启动参数配置，更改后需重启程序生效
  "Startup": {
    // 缓存
    "Cache": {      
      "Type": 0, // 0:内存 1：Redis
      "Redis": {
        "ConnectionString": "127.0.0.1:6379,password=,defaultDatabase=2"
      }
    },
    // 日志
    "Log": {
      "Operation": true // API访问日志开关
    },
    "TenantRouteStrategy": 0 // 租户策略，0：Route模式 1：Host模式
  },
  // 系统配置，支持热更新，修改后立即生效，无需重新启动程序
  "System": {
    // 监控CURD动作得SQL语句，开启将在日志窗口中显示执行的SQL语句
    "WatchCurd": true, 
    // 用于雪花算法，数据中心ID
    "DataCenterId": 5, 
    // 用于雪花算法，工作站ID
    "WorkId": 20, 
    // 是否启用API权限验证，如果启用API需与资源绑定，否则将不允许访问API
    "EnableApiAccessControl": true, 
    // 头像上次参数
    "uploadAvatar": {
      "uploadPath": "D:/upload/avatar", // 上传根路径
      "requestPath": "/upload/avatar", // 相对根路径
      "maxSize": 1048576, 
      "contentType": [
        "image/jpg",
        "image/png",
        "image/jpeg",
        "image/gif"
      ]
    }
  },
  // 租户配置，目前采样配置方式，应该改为从总控平台获取
  "Tenants": [
    {
      "Id": "1", // 租户唯一标识
      "Code": "Tenant1",// 租户Code(唯一标识),如果为Host模式将作为租户的二级域名
      "Name": "租户1", // 租户名称
      // 当前租户的数据库参数配置，支持多数据库，不同应用可指向不同的数据库
      "DbOptions": [
        {
          // 应用数据库key
          "Key": "system", 
          // 数据库类型 MySql = 0, SqlServer = 1, PostgreSQL = 2, Oracle = 3, Sqlite = 4 
          // 更多支持：https://github.com/dotnetcore/FreeSql/wiki/入门
          "DbType": "0",
          "ConnectionStrings": [
            {
              // 连接字符
              "ConnectionString": "Server=localhost; Port=3306; Database=tenant1db; Uid=root; Pwd=000000; Charset=utf8mb4;", 
              // 0主库 1为从库             
              "UseType": 0 
            },
            {
              "ConnectionString": "Server=localhost; Port=3306; Database=Tenant1db; Uid=root; Pwd=000000; Charset=utf8mb4;",
              "UseType": 1 
            }
          ]
        },
        // 多数据库支持，blog为第二个应用库配置示例，与system配置结构一致
        {
          "Key": "blog",
          "DbType": "0",
          "ConnectionStrings": [
            {
              "ConnectionString": "Server=localhost; Port=3306; Database=Tenant1db; Uid=root; Pwd=000000; Charset=utf8mb4;",
              "UseType": 0
            },
            {
              "ConnectionString": "Server=localhost; Port=3306; Database=Tenant1db; Uid=root; Pwd=000000; Charset=utf8mb4;",
              "UseType": 1
            }
          ]
        }
      ],
      // 当前租户 JWT Token 配置参数
      "Items": {
        "Audience": "https://www.xxxx.com/", 
        "ExpirationMinutes": "1000", 
        "Issuer": "https://www.xxxx.com/", 
        "Secret": "2qtiOLpT7mJQx239e2kgMheAH7B9lGQJnoxYRCb7KX3x1ogDEd55I7dJ1ziYptiTF"
      }
    },
    // 复制租户1可进行租户2的配置
  ]
}

```
## 鉴权用户使用
> 构造函数依赖注入IAuthUser即可使用。
```csharp
 public class DemoService 
    {
        private readonly IAuthUser _authUser;
        public DemoService(IAuthUser authUser)
        {
            _authUser = authUser;
        }

        public async Task<IResponseEntity> Test()
        {
            // 通过 _authUser 可获取到当前用户包括所属租户在内的信息
        }
    }
```

## 缓存使用
> 构造函数依赖注入ICache即可使用。
```csharp
 public class DemoService 
    {
        private readonly ICache _cache;
        public DemoService(ICache cache)
        {
            _cache = cache;
        }

        public async Task<IResponseEntity> Test()
        {
            // 通过 _cache 可对缓存操作
        }
    }
```


## 新增应用接口
> 首先保证初始化工作完成。

待续...

## ORM 使用

系统采用 Freesql 作为系统ORM，更多ORM文档见：[FreeSql](https://github.com/dotnetcore/FreeSql/wiki/入门?_blank)