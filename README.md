# 框架使用说明

*1、创建项目数据库，分别初始化 bitadmin.mssql.tables.sql(表结构)、bitadmin.mssql.datas.sql（初始数据）数据库脚本。
*2、运行以下命令生成实体
Scaffold-DbContext “data source=bitdao.cn,1011;initial catalog=BitAdminCore;user id=BitAdminDev;password=BitAdminDev;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DataContext -Force
*3、编译运行访问
