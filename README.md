# 框架使用说明
`BitAdminCore`是基于`net core`的`管理应用``快速开发框架`，为管理应用开发提供必要的`基础功能`。<br>
示例地址：http://bit.bitdao.cn <br>
功能介绍：详见示例 <br>
QQ交流群：202426919

## 创建项目
* 使用`Visual Studio 2017` 的 `Cookiecutter` 创建项目。
* 创建项目数据库，分别初始化脚本:`bitadmin.mssql.tables.sql`(表结构)、`bitadmin.mssql.datas.sql`（数据）。
* 运行以下命令生成实体(`注意修改数据库连接`):<br>
  `Scaffold-DbContext “data source=.;initial catalog=BitAdminCore;user id=sa;password=123456;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DataContext -Force`<br>
* 将 `DataContext.cs` 文件中的连接串改为 `optionsBuilder.UseSqlServer(SqlHelper.connectionString);`。
* 修改`appsettings.json` 的 `数据库连接串`。
* `F5`运行项目。

