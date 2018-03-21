# 框架使用说明

## 新建项目
* 使用`Visual Studio 2017` 的 `Cookiecutter` 创建项目。
* 创建项目数据库，分别初始化 bitadmin.mssql.tables.sql(表结构)、bitadmin.mssql.datas.sql（初始数据）数据库脚本。
* 运行以下命令生成实体(注意修改数据库连接):<br>
  `Scaffold-DbContext “data source=.;initial catalog=BitAdminCore;user id=sa;password=123456;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DataContext -Force`<br>
  可能出现`Build failed.`原因我也不知道，问`微软`或`想办法`。<br>
  `EFCore`技术相关问题，从官方获得支持。<br>
* 修改`appsettings.json`和`appsettings.Development.json`的`数据库连接`。
* `F5`运行项目。

