# 框架使用说明

## 新建项目
* 创建项目数据库，分别初始化 bitadmin.mssql.tables.sql(表结构)、bitadmin.mssql.datas.sql（初始数据）数据库脚本。
* 运行以下命令生成实体:<br>
  `Scaffold-DbContext “data source=.;initial catalog=BitAdminCore;user id=sa;password=123456;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DataContext -Force`<br>
  可能出现`Build failed.`原因我也不知道，新建一个net core项目，生成之后，把实体文件拷贝过来即可。<br>
* F5运行项目
