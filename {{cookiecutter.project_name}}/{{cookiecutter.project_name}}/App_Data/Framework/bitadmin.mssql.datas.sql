delete SysDepartment where departmentId<>'2379788E-45F0-417B-A103-0B6440A9D55D'
delete SysUser where departmentId<>'2379788E-45F0-417B-A103-0B6440A9D55D'

delete SysRole where id<>'3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B'
delete SysRoleUser where roleId<>'3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B'
delete SysRoleOperatePower where roleId<>'3F9578C5-C0A2-4C7A-B0FD-C93FAE47194B'

delete FlowMain where id<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'
delete FlowStep where mainId<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'
delete FlowStepPath where mainId<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'

GO
INSERT [dbo].[SysDepartment] ([departmentId], [parentId], [departmentCode], [departmentName], [departmentFullName], [orderNo], [createTime], [createBy], [updateTime], [pdateBy]) VALUES (N'2379788e-45f0-417b-a103-0b6440a9d55d', NULL, N'root', N'公司部门', N'公司部门', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SysUser] ([userId], [departmentId], [userCode], [userName], [userPassword], [idCard], [mobile], [email], [post], [gender], [birthday], [extendId], [userStatus], [orderNo], [createBy], [createTime], [updateBy], [updateTime]) VALUES (N'5eeea4ce-71ab-4464-b72f-17f5163ee944', N'2379788e-45f0-417b-a103-0b6440a9d55d', N'admin', N'管理员', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, N'13800138000', NULL, NULL, NULL, NULL, NULL, N'work', NULL, NULL, NULL, N'5eeea4ce-71ab-4464-b72f-17f5163ee944', NULL)
GO
INSERT [dbo].[SysRole] ([id], [roleName]) VALUES (N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'系统管理员')
INSERT [dbo].[SysRoleUser] ([id], [roleId], [userId]) VALUES (N'ea757a2b-a0c2-48a7-9373-5d58972aba7a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5eeea4ce-71ab-4464-b72f-17f5163ee944')
GO
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'A', N'A1', N'A1', NULL, 1)
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'A', N'A2', N'A2', NULL, 2)
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'B', N'B1', N'B1', NULL, 1)
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'B', N'B2', N'B2', NULL, 2)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'ffeb6542-565b-48b7-bbd9-ae87c7398886', N'query', N'访问', NULL, NULL, 1)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'3e10d841-f9ea-4960-b6a1-864de55ef0f2', N'add', N'新增', NULL, NULL, 2)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'0a75b779-82ea-4f07-a718-75a231d1b211', N'save', N'保存', NULL, NULL, 3)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'9045a8a8-850d-40a2-a961-7c1a9dc357d5', N'delete', N'删除', NULL, NULL, 4)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'9e88c097-ce66-4f5b-ad45-f39dd321b281', N'import', N'导入', NULL, NULL, 5)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'bf988b55-25b0-4ecb-9e02-8b8e65ea8776', N'export', N'导出', NULL, NULL, 6)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'032a9c34-caf0-43f1-be56-681dee530dd6', N'download', N'下载', NULL, NULL, 7)
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'f515f1b4-9f4e-4f4c-90b1-9cbf02b356bf', N'back', N'返回', NULL, NULL, 8)
GO
--流程相关
INSERT [dbo].[FlowMain] ([id], [code], [name], [description]) VALUES (N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'999', N'示例流程', NULL)
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤五', 10, NULL, NULL, N'select', N'5', N'5', NULL, N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', NULL, NULL, NULL, NULL, N'新增步骤', N'left: 438px; top: 510px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'd96e156a-eb1c-6020-d955-375393111a8c', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'结束', 100, NULL, NULL, N'auto', N'100', NULL, NULL, N'Roles', N'0', NULL, NULL, NULL, NULL, N'结束', N'left: 435.22px; top: 596.17px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'6041143c-4c88-3810-911e-3f49cc2291d2', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤四', 10, NULL, NULL, N'select', N'4', N'4', NULL, N'startuser', NULL, NULL, NULL, NULL, NULL, N'新增步骤', N'left: 436px; top: 416px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤一', 10, N'Agency', N'Circularize', N'auto', N'1', N'0,1', 100, N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', NULL, NULL, N'新增步骤', N'left: 436px; top: 138px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'34137caa-ef96-9158-e1cf-89ab1418de5d', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'起始', 1, N'Agency', NULL, N'auto', N'0', N'0', NULL, N'Users', N'5eeea4ce-71ab-4464-b72f-17f5163ee944', NULL, NULL, NULL, NULL, N'起始', N'left: 435.22px; top: 53.32px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'268aa12c-69b6-ae58-de7c-95aaef411003', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤三', 10, N'Agency', N'Circularize', N'select', N'3', N'3', NULL, N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', NULL, NULL, NULL, NULL, N'新增步骤', N'left: 436px; top: 326px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStep] ([stepId], [mainId], [stepName], [stepStatus], [agency], [circularize], [runMode], [linkCode], [showTabIndex], [reminderTimeout], [auditNorm], [auditId], [auditNormRead], [auditIdRead], [smsTemplateToDo], [smsTemplateRead], [description], [style]) VALUES (N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤二', 10, NULL, N'Circularize', N'select', N'2', N'1,2', NULL, N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', NULL, NULL, NULL, NULL, N'新增步骤', N'left: 441px; top: 223px; color: rgb(14, 118, 168);')
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'14154379-310b-4a22-91d7-0adcdeb21e6d', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'不同意', 1, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'fec120fe-0c24-424b-81df-9f3cc6ff9600', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', N'd96e156a-eb1c-6020-d955-375393111a8c', N'同意', 6, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'2dc63476-c1c8-43aa-874b-a7b33d94ec09', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'34137caa-ef96-9158-e1cf-89ab1418de5d', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'发起', 1, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'831107d6-423b-4413-a8ca-b21d2008a6a6', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'同意', 2, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'5c111793-a2a4-41fd-826d-b3a43f0870db', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'268aa12c-69b6-ae58-de7c-95aaef411003', N'6041143c-4c88-3810-911e-3f49cc2291d2', N'同意', 4, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'c714ee01-df70-43a3-821a-c413864362b7', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'6041143c-4c88-3810-911e-3f49cc2291d2', N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', N'同意', 5, NULL, NULL)
INSERT [dbo].[FlowStepPath] ([id], [mainId], [startStepId], [stopStepId], [nikename], [condition], [expression], [description]) VALUES (N'05345a49-e6b0-4f38-a527-f530adb34c02', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'268aa12c-69b6-ae58-de7c-95aaef411003', N'同意', 3, NULL, NULL)
GO
--模块
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', NULL, N'工作台', N'fa-code', NULL, 1)
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'系统管理', N'fa-cog', NULL, 91)
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'5dfab181-a524-44b2-9e6a-273fc2d2e272', NULL, N'开发管理', N'fa-code', NULL, 92)
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'20256a37-fead-4d11-8052-f0df33738c3b', NULL, N'示例功能', NULL, NULL, 93)
--页面、页面操作、页面权限
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'193495e7-d975-4d54-99ea-06b205ecdfee', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Log', N'日志管理', N'fa-anchor', N'/system/log', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'20256a37-fead-4d11-8052-f0df33738c3b', N'ExampleRedirect', N'表单示例2', NULL, N'/templates/exampleredirect', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'ToRead', N'我的待阅', N'fa-anchor', N'/workbench/toread', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Role', N'角色权限', N'fa-address-card-o', N'/system/role', N'', 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5246eaba-c184-46f0-bd78-37ac309fec39', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Operation', N'页面操作', N'fa-edit', N'/develop/operation', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'e2041824-a541-478d-977a-51935c2fa74a', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Dictionary', N'数据字典', N'fa-book', N'/develop/dictionary', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'Read', N'我的已阅', N'fa-anchor', N'/workbench/read', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'20256a37-fead-4d11-8052-f0df33738c3b', N'ExampleTree', N'表单示例3', NULL, N'/templates/exampletree', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Generating', N'生成代码', NULL, N'/develop/generating', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'ToDo', N'我的待办', N'fa-anchor', N'/workbench/todo', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Department', N'部门信息', N'fa-group', N'/system/department', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'PrototypingExample', N'原型示例', NULL, N'/develop/prototypingexample', NULL, 5)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'b3818125-8874-4413-8b0e-da5627181ae9', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'User', N'用户信息', N'fa-user-circle', N'/system/user', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Menu', N'系统菜单', N'fa-reorder', N'/develop/menu', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'20256a37-fead-4d11-8052-f0df33738c3b', N'ExampleOne', N'表单示例1', NULL, N'/templates/exampleone', N'功能实现', 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'Done', N'我的已办', N'fa-anchor', N'/workbench/done', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'FlowSetting', N'流程配置', N'fa-user-circle', N'/system/flowsetting', NULL, 5)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'63661fc7-7fba-4f64-be39-fb34021f2242', N'20256a37-fead-4d11-8052-f0df33738c3b', N'WorkflowExample', N'流程示例', NULL, N'/workflowexample/example', N'流程功能实现', 4)
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f9d0015c-5143-4a67-9935-019906751f6a', N'e2041824-a541-478d-977a-51935c2fa74a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'dac6a751-52c4-481a-a3e9-01d36b1a1298', N'92a37fab-14e5-4f99-818d-213e27079e68', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'66a8a698-23bb-4698-ae29-0222f63c3c9e', N'27e63604-61b1-4429-80b2-c9d623403182', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3ee9fc15-8298-463b-b3e1-0324c22c5a09', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd39df24e-5455-4489-b85c-034ad116e4ad', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'12f6f26e-5f68-4608-af12-087ee2c37d9c', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b8d7c638-9bd2-43d7-b60e-0a0c93e7875d', N'b3818125-8874-4413-8b0e-da5627181ae9', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'1964452b-bb86-4580-af44-0f54029957c0', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'851fa846-bd8d-40ef-9d0d-10425901e05c', N'bdfcb2eb-728c-48e0-b7a3-5e2f0810346a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'20a55951-aa60-498d-af62-18fd6f5f8b4d', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'88def3d1-0b7d-4aa5-bc3b-1d7573352ad7', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'faa0435c-e166-45ba-89f3-22eb285ddb3f', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2f14db59-d1b6-434e-be44-29262ac03cf4', N'e2041824-a541-478d-977a-51935c2fa74a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'87c0914b-3826-45a7-8257-2ab77ceb08c9', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'159cc640-d283-413b-b9ac-2c7c2a2fd62a', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'23768c99-a54e-407f-857a-307ac84ff35f', N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'fe0fdff9-dbd3-4d4d-8d94-31e3d7267682', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cdcbdf1f-e393-40a3-ae88-3390a6a90885', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'40d724b7-5ea6-4223-8205-3f9a03442338', N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'c1cbd3c5-3c02-4bc4-89e5-4028c4b09434', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f0f6e831-2789-4c7b-939f-434ac301eb7e', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'9f7e5ec5-d85f-4529-9aea-439802d9fc09', N'92a37fab-14e5-4f99-818d-213e27079e68', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'abcde15c-2df3-47c2-afa2-4a96a6598019', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'dfdfd61a-6273-469b-8d87-53173e917bf0', N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0b2d551c-4c03-4292-b216-55c266581b73', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'c60e61af-436c-4a8e-b5cf-59c32b590eb3', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4afd81ad-8580-45cd-8fe2-651c9ba6944a', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5d2981c7-e966-4d0c-93a9-67db861b2acc', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5d09ecb4-ba4f-4be4-bd47-6b91c39292c7', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'bf396d7e-c0c4-4251-87e8-6f7ff57a3793', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e5b3c1a1-aea3-4c37-857d-6fbfdd74aabf', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8610cf54-88f0-4503-a211-73fb321298f7', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'78ce802e-46e2-41f9-b1cf-76b57c0e9caa', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'070659cc-bb90-4027-9b3c-7ac979b0e5f7', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4bce5686-54c1-4057-9a18-7d5baa895ba2', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'70aafba1-b7cc-4d24-a8ac-807bdac69ec0', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'bd94c312-f8e2-4860-8c44-80fa88c2cc21', N'92a37fab-14e5-4f99-818d-213e27079e68', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b3d673f6-ad9a-403a-a8e2-819c1b545dbb', N'b3818125-8874-4413-8b0e-da5627181ae9', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'02d4104d-10c2-43de-8ac5-82e700e4ada8', N'e2041824-a541-478d-977a-51935c2fa74a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b94667dd-e299-4afd-a098-8468663dede7', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b44c7b07-d876-470d-acb6-84fe602114de', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'95e87edd-b857-444b-9ab1-859e991c117b', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'71d0da3c-4f7f-4d57-895a-8b4a103ba1ab', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'003e752f-7d91-483b-b546-8fadc53c00f7', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ef4915c7-bd87-4d6e-9b8d-906c6f48ff11', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8f07f590-1c78-491a-95ea-914c158e16e8', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'87566bac-ea8c-4982-91ce-93401333b8e6', N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4e1703b0-77d8-48bb-b2e5-97785408dacc', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3efe7e3c-3598-49f4-96d7-a2942ac8290a', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cd2c7e4c-4e9e-4199-aec6-a71a4437751e', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'288aca88-5db7-4625-bbb2-b0806677b4e6', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f6537403-e7dc-43d9-aabc-b126b2f20b96', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'1358fd6c-4bed-4300-a1a2-b4acb9a72b07', N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'54e5a90a-f790-4167-93d0-b4b3b89f1fb1', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2fce2a51-cf4f-4e4b-974b-bab3ab57acbc', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'6c6ee224-199d-403d-8306-bbe3e8385f6d', N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cb979c90-2889-4a16-a9ac-bcb08451619b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'78072739-f2ad-47d3-aaba-bccf1fac3f12', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'96b18cb4-88d4-4d2c-82de-bd35e5f1695d', N'b3818125-8874-4413-8b0e-da5627181ae9', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'03e035e1-ff90-4ef5-9e6b-bf3923f4c66d', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd5d37669-808a-4436-b91d-c9aa28e88e3c', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5083a42d-23f8-49cb-90af-c9dd7630a5b1', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'9d993408-05f5-4fbe-a3be-ccc45819016d', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f6cf5fd8-250e-460a-8c24-ce67e26e64f8', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'fc292266-e341-45e5-a75a-d0449ce51d39', N'92a37fab-14e5-4f99-818d-213e27079e68', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ef9a467c-0893-4943-bd28-d214ddf7a83a', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'37e75426-ba62-4d09-ad4b-d479e1e52d11', N'92a37fab-14e5-4f99-818d-213e27079e68', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'675e0a09-7e59-4e65-ac12-d49faf086ae6', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4c5912e9-028e-4eac-80c0-d6c6c71dc3f0', N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'fe1e8296-bd00-4d5f-ab8b-d80c2c119b96', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7d349399-4134-4014-996a-d82931415e3a', N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd29d3875-a735-46a2-96f1-d88a66eb6e6f', N'b3818125-8874-4413-8b0e-da5627181ae9', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5d79e3b6-abeb-4b60-b32d-de14dc306d53', N'e2041824-a541-478d-977a-51935c2fa74a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a87d9bfa-5e80-4c98-b857-df312eb41581', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd73ef19d-8ac2-49b6-903b-e7c00e8d180a', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b264ae8b-343a-48e6-90d1-e9464443be03', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e70b4703-1042-4afa-be2f-ea4d26959bc5', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'63ab25c6-a3c3-46dc-8033-eabf4044c2dc', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'66be12ae-539c-40d1-9712-f0565b6c7d6c', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd98a4041-81a0-4438-8b5b-f36c2aa58c4b', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'963cbb6b-2874-4747-af26-f5c71304b77d', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'09faf111-92c9-4aa5-9349-f91d40f63311', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'eb83bb87-8529-4b5d-bcfc-f9e60e89a32f', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'export')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'645a0958-0d03-4adb-a1db-019b4772c12a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'42395b55-bdde-41d6-809e-019b6162ac7d', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b3818125-8874-4413-8b0e-da5627181ae9', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a85fbd20-f382-4e49-b2fa-08920add3963', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'20256a37-fead-4d11-8052-f0df33738c3b', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'0dd347f0-d9dd-4af4-a8cc-0cd0e08594e6', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a4cf72d4-2515-434a-bfdd-0e153dd24337', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'dc321206-2338-46e1-8cd7-353b382b2247', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'b30cf2fc-0174-4b3d-94f8-354937e39db0', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a909e265-c481-46dd-9d55-4c33418ace59', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'692d0563-b53c-4c5f-b855-53a26331d014', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a5f90500-9384-4656-bd34-54035b396d45', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'e6d6e17f-7ec3-4fed-9cf9-5514d854b120', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'9ae66e1d-ce67-4462-8b45-6a80e62ce781', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'7debfdb0-8816-460c-b36f-8ecdf84a1561', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'e2041824-a541-478d-977a-51935c2fa74a', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'4d94ddfc-a117-425d-8df5-a749ecc89786', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a9b09202-cf07-442a-90b9-ade56a448632', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'b0463dc4-1705-4b9c-a76a-af778b5e8a9b', NULL, N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'f5a698a8-a734-40f9-9a76-baa08e1d24c2', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'1b435459-3e31-4e94-81d7-d9b4742f346a', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'5620db15-421a-4b3f-8f47-c3bb51576c8e', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'6e16c53c-53ee-4c13-b096-a44458901b2e', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'229d5407-114f-4feb-b13f-c8d813cc0738', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'0038fa03-06c6-49f4-a21a-ca2ce265e153', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'cfcdc86d-3546-4578-9e1a-b5c210cbf2f5', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'9e1155e5-e254-44de-87e9-cb5a49fa5122', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'949ff2eb-ecc1-4d19-9d59-d3c82876f94f', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'e7c9042b-db34-4368-8dcc-d5b50f1ee53c', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'2f709875-7508-4e41-9827-dfe36bb0e87a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'bf4c80f2-8a7b-4374-a109-e2d02068dbb1', NULL, N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'ca098361-ab37-4b18-af9e-ef8e8b3d2ae0', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'1b435459-3e31-4e94-81d7-d9b4742f346a', N'query,add,save,delete,import,export,download')
GO
