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
GO
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'A', N'A1', N'A1', NULL, 1)
GO
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'A', N'A2', N'A2', NULL, 2)
GO
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'B', N'B1', N'B1', NULL, 1)
GO
INSERT [dbo].[SysDictionary] ([type], [member], [memberName], [description], [orderNo]) VALUES (N'B', N'B2', N'B2', NULL, 2)
GO
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'5dfab181-a524-44b2-9e6a-273fc2d2e272', NULL, N'开发管理', N'fa-code', NULL, 92)
GO
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'系统管理', N'fa-cog', NULL, 91)
GO
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', NULL, N'工作台', N'fa-code', NULL, 1)
GO
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'20256a37-fead-4d11-8052-f0df33738c3b', NULL, N'示例功能', NULL, NULL, 93)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'193495e7-d975-4d54-99ea-06b205ecdfee', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Log', N'日志管理', N'fa-anchor', N'/system/log', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'20256a37-fead-4d11-8052-f0df33738c3b', N'CompleteRedirect', N'BitAdmin示例2', NULL, N'/Templates/ExampleRedirect', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'ToReadList', N'我的待阅', N'fa-anchor', N'/Workbench/ToReadList', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Role', N'角色权限', N'fa-address-card-o', N'/system/role', N'', 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5246eaba-c184-46f0-bd78-37ac309fec39', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Operation', N'页面操作', N'fa-edit', N'/system/operation', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'e2041824-a541-478d-977a-51935c2fa74a', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Dictionary', N'数据字典', N'fa-book', N'/system/dictionary', NULL, 3)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'ReadEndList', N'我的已阅', N'fa-anchor', N'/Workbench/ReadEndList', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'20256a37-fead-4d11-8052-f0df33738c3b', N'CompleteTree', N'BitAdmin示例3', NULL, N'/Templates/ExampleTree', NULL, 4)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'ToDoList', N'我的代办', N'fa-anchor', N'/Workbench/ToDoList', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Department', N'部门信息', N'fa-group', N'/system/department', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'b3818125-8874-4413-8b0e-da5627181ae9', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'Personnel', N'用户信息', N'fa-user-circle', N'/system/user', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'Menu', N'系统菜单', N'fa-reorder', N'/system/menu', NULL, 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'20256a37-fead-4d11-8052-f0df33738c3b', N'Complete', N'BitAdmin示例', NULL, N'/Templates/Example', N'BitAdmin规范功能实现', 1)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'DoneList', N'我的已办', N'fa-anchor', N'/Workbench/DoneList', NULL, 2)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'MainFlow', N'流程配置', N'fa-user-circle', N'/Workflow/Flow', NULL, 5)
GO
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'63661fc7-7fba-4f64-be39-fb34021f2242', N'20256a37-fead-4d11-8052-f0df33738c3b', N'WorkflowExample', N'Workflow示例', NULL, N'/WorkflowExample/Example', N'Workflow功能实现', 2)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'032a9c34-caf0-43f1-be56-681dee530dd6', N'download', N'下载', NULL, NULL, 7)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'0a75b779-82ea-4f07-a718-75a231d1b211', N'save', N'保存', NULL, NULL, 3)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'9045a8a8-850d-40a2-a961-7c1a9dc357d5', N'delete', N'删除', NULL, NULL, 4)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'3e10d841-f9ea-4960-b6a1-864de55ef0f2', N'add', N'新增', NULL, NULL, 2)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'bf988b55-25b0-4ecb-9e02-8b8e65ea8776', N'export', N'导出', NULL, NULL, 6)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'f515f1b4-9f4e-4f4c-90b1-9cbf02b356bf', N'back', N'返回', NULL, NULL, 8)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'ffeb6542-565b-48b7-bbd9-ae87c7398886', N'query', N'访问', NULL, NULL, 1)
GO
INSERT [dbo].[SysOperation] ([id], [operationSign], [operationName], [createBy], [createTime], [orderNo]) VALUES (N'9e88c097-ce66-4f5b-ad45-f39dd321b281', N'import', N'导入', NULL, NULL, 5)
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7b1c345c-40c0-4355-b831-07344c74139c', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'12f6f26e-5f68-4608-af12-087ee2c37d9c', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cf6d3e43-d492-4512-82ef-09505808347a', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd13df55a-065d-4e75-902f-0bbc05e0f8a1', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'67edaa12-02c8-45bb-82a7-0bebe5dd84fc', N'92a37fab-14e5-4f99-818d-213e27079e68', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f72d69d3-cd1b-4138-b98f-0f830d3f0d0c', N'e2041824-a541-478d-977a-51935c2fa74a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'476556d0-5e95-43b8-85b1-1025ea6351bb', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2ccd18a0-31d5-40cf-9b82-1116783f6f94', N'b3818125-8874-4413-8b0e-da5627181ae9', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'6ba6099e-6eb4-4313-be95-12a1d131d4a8', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4331072c-7e15-4c3b-92cc-177da34cd58c', N'92a37fab-14e5-4f99-818d-213e27079e68', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'aa1d2e0c-6f5d-475c-bd2e-19d76266d9f9', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'deaddd9f-45c7-4677-9b0a-1ba93cb362ed', N'e2041824-a541-478d-977a-51935c2fa74a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'32f139e7-9b22-4cb1-8623-1d657e217e82', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'010b5098-48d1-47c4-ae1b-1e3201980b7d', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'faa0435c-e166-45ba-89f3-22eb285ddb3f', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4823d3e9-a377-4931-ad3f-24e5ccaab242', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'1cc11f45-51f6-4aeb-842f-260bb5687936', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'41270077-4883-4c1c-b6db-2a9232004214', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8f910046-0693-416f-92a2-2e14e0e1e29c', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'37defda3-aa80-42a7-846b-2e31910d7d90', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3cf0f358-59c3-4336-8313-2f5df2d4c0f3', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2a3581a7-1ddb-4c0e-96df-38394f618c4e', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'6ebe3a3e-ef99-46e0-9eb4-38dbf4d7fce8', N'e2041824-a541-478d-977a-51935c2fa74a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'244897f4-353b-4787-b087-3af2c367bb4d', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd2c26518-cd34-4d5b-86af-3b6e7051ed46', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'df48718e-077c-4ea4-8bd8-3b8da9e616c5', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'80435d4e-f660-4eff-bedd-3bf742f623b4', N'b3818125-8874-4413-8b0e-da5627181ae9', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0acd44c5-749c-4e1f-9098-3c6b0e20eda4', N'b3818125-8874-4413-8b0e-da5627181ae9', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'55093bb9-0c20-4515-b1d3-3de1275aee64', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4601503f-e9e5-4266-919b-3ee77d6940d8', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2ddde584-4bc8-44d9-83a8-3f72a50291c4', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a8251e65-684c-4b2b-8f65-42d1d346f6eb', N'b3818125-8874-4413-8b0e-da5627181ae9', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3a0dbc62-3c91-49eb-9547-4b81fb69f89b', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b62439c9-2078-4919-837a-4be2e2358848', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5ac2c9fc-690a-422e-984c-4d5c6e2e3fa2', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7a1afa0b-262f-4f76-b163-4ea95c88acac', N'e2041824-a541-478d-977a-51935c2fa74a', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'57a911df-8982-4d85-bfc5-4f38f27ad52e', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7d434b44-d787-4df5-bf16-5354baa99e09', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5fd0cfb6-2aff-4966-bcf8-53ff46c86c1f', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f3390676-8d82-4abb-b7c4-556933457535', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0b2d551c-4c03-4292-b216-55c266581b73', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0819f577-5171-49cb-b6ff-564073e86fe8', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f426e662-1048-414e-9a05-596a7a081fef', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'088b1eae-bf3e-4fe2-8031-5c91cde5e74e', N'92a37fab-14e5-4f99-818d-213e27079e68', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e49688c8-b9d2-4f26-9c59-6360d3dde49e', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4afd81ad-8580-45cd-8fe2-651c9ba6944a', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b394b802-a929-43e1-9084-6552513f4e6d', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd98e4aa0-c275-4b01-a3f8-68d0c9fadb6c', N'b3818125-8874-4413-8b0e-da5627181ae9', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'9ee7182b-85f9-4702-85d0-68fdadebb10c', N'e2041824-a541-478d-977a-51935c2fa74a', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b027f3be-1d84-4a61-90ad-697b0c7aad89', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'621d4dd7-b5c5-4d67-a57d-6ab4ee357fcc', N'92a37fab-14e5-4f99-818d-213e27079e68', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8fb5114b-c66a-4f98-877b-6c59f1a0ae4d', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f61abed1-71a5-4dab-900e-6dd1011208b2', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'c6c8039b-9721-41fd-b573-6e25e3bbf85b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'505d3396-5efb-4e41-a861-6e4b8f6e019f', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e5b3c1a1-aea3-4c37-857d-6fbfdd74aabf', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ebf53679-df9b-46bc-9271-71aedefe2f29', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'9824051b-c7aa-4653-9140-71b5cadcf876', N'92a37fab-14e5-4f99-818d-213e27079e68', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'aa4ab1e9-31ae-4889-82d7-71c760e58aac', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4d9a2019-1eda-4ef1-9eab-72b29b0c2278', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7f852024-45ed-4853-a71a-76cd0e6f00fc', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8af7c49d-43f5-49b6-94f0-7836c6966204', N'92a37fab-14e5-4f99-818d-213e27079e68', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'799e2a4e-4ee0-4339-9fa5-78fc9ba3d980', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'96fe5726-b58f-4822-a413-7beec93b176c', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'feecec25-7728-4935-be7e-7f45cad9bb9f', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4b79483d-e973-4643-9ce0-7fcb389ac1e0', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7b32c6ac-4e2d-46b7-bf5d-823ef0255509', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a744e77a-8caf-4d78-ad82-85ea36b16719', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'003e752f-7d91-483b-b546-8fadc53c00f7', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ef4915c7-bd87-4d6e-9b8d-906c6f48ff11', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e2bccee1-b5e3-4814-8c2f-922915f7b3bd', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd223fffc-0160-46f3-98d6-9271e3f45aec', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4239c3cd-27b3-49c1-9373-94986e9aa8c8', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'776335d6-229f-46de-8973-9578e40d1bbb', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7ca9a5dc-5a6d-458e-bca3-9ab022621603', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a4975f72-a357-41c1-aa79-9abe30b24e55', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'b6a25edf-4d23-4b7f-8d21-9da64bddf322', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'96cf13e4-a361-4c46-ae3b-9e355aa8abf2', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'9f6bedf7-5249-4669-b81c-9f4d007c5869', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'85283974-16ab-45c6-ad4e-a1e1030f412f', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3efe7e3c-3598-49f4-96d7-a2942ac8290a', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3cfc3df5-bf46-4e0f-9e40-a3d5cc7614b2', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'36236f9d-65e7-43cd-a663-a581c0d3da6a', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'703c190f-018e-4aeb-a963-a675127128ff', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'58db022e-a2c4-4ff2-8fb7-a7855752f22a', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'fa307827-d0c3-428d-b153-a89b3f867476', N'92a37fab-14e5-4f99-818d-213e27079e68', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'171e9fcd-22fa-467e-a109-abbdd5e25b4a', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8f113187-209d-4181-8285-adf45b4f8995', N'92a37fab-14e5-4f99-818d-213e27079e68', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0cdceddd-1d1a-47f8-87c5-ae49cadb6d89', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'4066506f-9b39-4391-8f25-b058c7a8cb64', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ced8aa00-8a49-4a29-bdbe-b0b55f7f7b07', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'61b4fb21-a32e-40c5-9dd3-b1e08d9cd60c', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'76241bab-73b5-4dad-a0fe-b26638442b36', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cfdf093d-682c-4862-b135-b2de94ee4ec2', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e2abb8fa-11f9-4247-b34b-b52b30da0a69', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ac4dbc9d-4bb7-4e46-b388-b5d7c64f1564', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'afd0710e-101b-4814-95da-b5f5c88a95fa', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7e228d0d-f309-41cc-b834-b998ecc53feb', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2fce2a51-cf4f-4e4b-974b-bab3ab57acbc', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd53bdfa9-85e7-4ee2-93a1-bc084325bc9b', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5b248f22-fe58-4e8e-96f4-bc5ac6fb7258', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'78072739-f2ad-47d3-aaba-bccf1fac3f12', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'5860a6f0-6daa-49fb-834c-bd238cda0ec1', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'c16743df-2bd7-4d31-97c6-bdf4e1e4e86b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'1c752607-1df3-4c45-b469-be5acc675d3a', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd744d252-d6dc-4dea-b8c5-be6f0ddffd2b', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'836a069e-3d80-43a5-9b33-bf2623e98c11', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a96715f9-cd5d-49f8-9f2d-c323eeb8a21c', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'c6ce8478-5229-455c-803e-c4c409c5a0c4', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'35a90eb1-ef26-4f9f-ac13-c860391ddf3b', N'b3818125-8874-4413-8b0e-da5627181ae9', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'ca104b1e-96b3-489e-981b-ceace49fbada', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'query')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'16bd708d-b644-4ed7-b9ca-cf412068838c', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f9fbd2ef-0c0a-4d4a-8dbf-d0bd64ca7cbd', N'e2041824-a541-478d-977a-51935c2fa74a', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'675e0a09-7e59-4e65-ac12-d49faf086ae6', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'16be9e44-1251-4005-b29c-d6c4c6bc5e6e', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'554bb02e-81f6-4212-90bc-d731ccad327b', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'3436b690-2645-48ca-b403-d7e2fff5318b', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e31b7429-5a7f-40d7-bdaa-db65672f97ba', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'745ed489-897c-4c5e-b74d-dc8691b338f8', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'8fa7c69c-db20-49c0-ad13-dd96e647d92b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'280874f5-94ba-4263-9be7-ddb17714c9e0', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'2d43e65b-8f7c-4dff-a829-de55b5abfa4a', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'7c77b1a8-4155-40d8-ab93-e0b4566ac679', N'e2041824-a541-478d-977a-51935c2fa74a', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e5983be4-0606-4fb1-9c25-e54b9fd2925e', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'77338ef2-62ca-43e9-84ca-e636cb9fef68', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'delete')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'bbfe4a65-3536-4b47-859b-e9d1fc036620', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'e70b4703-1042-4afa-be2f-ea4d26959bc5', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'63ab25c6-a3c3-46dc-8033-eabf4044c2dc', N'a957e1b0-2545-4648-8c75-3e040ec5893a', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'cc8445aa-f03a-4195-9087-eac111cace37', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'import')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'a0158f6b-e103-4733-b73f-ec37056540f0', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'97d2a968-bc33-401f-ad6e-ee0ccc5695ea', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'd98a4041-81a0-4438-8b5b-f36c2aa58c4b', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'save')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'38920bbf-6114-4cd4-8c07-f97c7e4e4d50', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'add')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'eb83bb87-8529-4b5d-bcfc-f9e60e89a32f', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'export')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'f0239bca-4421-41a0-948a-fdd57fd44657', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'back')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'0f7555e1-3677-4d9e-a35e-fdf5f5fbe871', N'b3818125-8874-4413-8b0e-da5627181ae9', N'download')
GO
INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (N'49874a96-bfda-4b4e-b2e0-ff490478cecb', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'export')
GO
INSERT [dbo].[SysRole] ([id], [roleName]) VALUES (N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'系统管理员')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'b3151b12-2198-46c0-b806-04ad832c1217', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'4ffac124-61ee-4c08-9406-0514fcaa2827', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'898b93a2-acba-4ae7-aa31-0d527742506e', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'a93a433d-85e7-4019-b7bf-e0f87429b7f4', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'c0eef335-7796-489b-b209-2008287f5862', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b3818125-8874-4413-8b0e-da5627181ae9', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'3b23b7d0-77ac-40b2-9917-43e01f2a0f59', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5246eaba-c184-46f0-bd78-37ac309fec39', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'6c110ede-25b8-410a-856a-478f1dea9f71', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'e2041824-a541-478d-977a-51935c2fa74a', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'1f296475-e97e-4e23-8459-52f3e7e9c7d4', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a5f90500-9384-4656-bd34-54035b396d45', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'b2acc77f-7b82-48d2-9f32-6dadfb10406a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'18546097-d4b2-4161-87f8-7eeb4b1c24c3', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'7a90de2b-0663-4611-9c48-aea881cd4f9a', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'018be04c-2113-4c3b-a4d1-8523c4394b22', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'193495e7-d975-4d54-99ea-06b205ecdfee', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'a2b680b4-c141-437f-8329-8c29016ebb61', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'ab1d894d-72bf-4286-93c6-8ea59b46f405', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5a4bd0dc-ed8b-42a0-9801-1040f71aa606', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'82d1ac42-dbbc-4ce0-b3ca-964bc5b3c6d8', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'e5a79796-419e-4271-845c-aca6b1251e09', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'861eb706-f261-4b5e-85f0-dea5bf37848f', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'823add62-6589-4b79-b1c3-acf912dea7a4', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'20256a37-fead-4d11-8052-f0df33738c3b', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'b0463dc4-1705-4b9c-a76a-af778b5e8a9b', NULL, N'52ee24b7-c2ff-44d0-9fee-3897d609d554', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'f5a698a8-a734-40f9-9a76-baa08e1d24c2', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'1b435459-3e31-4e94-81d7-d9b4742f346a', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'c4cc9890-4e48-415a-a1e8-c0f8e46ad1e8', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'94a9c1a1-45a8-4ec5-bf0d-764331878542', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'1cee77f4-0a76-418e-9027-dee6a8a6d8a0', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'63661fc7-7fba-4f64-be39-fb34021f2242', N'20256a37-fead-4d11-8052-f0df33738c3b', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'2f709875-7508-4e41-9827-dfe36bb0e87a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5dfab181-a524-44b2-9e6a-273fc2d2e272', NULL, N'query')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'bf4c80f2-8a7b-4374-a109-e2d02068dbb1', NULL, N'92a37fab-14e5-4f99-818d-213e27079e68', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'ca098361-ab37-4b18-af9e-ef8e8b3d2ae0', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'b916ab03-f679-4b65-9131-b21d7a2ad108', N'1b435459-3e31-4e94-81d7-d9b4742f346a', N'query,add,save,delete,import,export,download')
GO
INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (N'e00e44f2-4691-42c8-a77a-f4b67a5411e7', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'query,add,save,delete,import,export,download,back')
GO
INSERT [dbo].[SysRoleUser] ([id], [roleId], [userId]) VALUES (N'ea757a2b-a0c2-48a7-9373-5d58972aba7a', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'5eeea4ce-71ab-4464-b72f-17f5163ee944')
GO
INSERT [dbo].[SysRoleUser] ([id], [roleId], [userId]) VALUES (N'b0d4c87d-b7d5-46e2-a713-ffa0ca354bb1', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', N'f44e2994-8103-4e3a-b1e5-78e93b514e55')
GO
INSERT [dbo].[SysUser] ([userId], [departmentId], [userCode], [userName], [userPassword], [idCard], [mobile], [email], [post], [gender], [birthday], [extendId], [userStatus], [orderNo], [createBy], [createTime], [updateBy], [updateTime]) VALUES (N'5eeea4ce-71ab-4464-b72f-17f5163ee944', N'2379788e-45f0-417b-a103-0b6440a9d55d', N'admin', N'管理员', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, N'13800138000', NULL, NULL, NULL, NULL, NULL, N'work', NULL, NULL, NULL, N'5eeea4ce-71ab-4464-b72f-17f5163ee944', NULL)
GO
INSERT [dbo].[FlowMain] ([id], [code], [name], [description]) VALUES (N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'999', N'示例1', N'测试流程')
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤五', N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', 1, 10, N'新增步骤', N'{"id":"539c567e-e218-4dd5-dc69-22ec3b90d8b0","StepName":"步骤五","process_name":"步骤五","conditions":"d96e156a-eb1c-6020-d955-375393111a8c:1:同意:","process_to":"d96e156a-eb1c-6020-d955-375393111a8c","AuditNorm":"Roles","style":"left: 438px; top: 510px; color: rgb(14, 118, 168);","AuditId":"3f9578c5-c0a2-4c7a-b0fd-c93fae47194b","StepStatus":10,"Description":"新增步骤","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":null,"Power":null,"RunMode":"select","JoinMode":null,"ExamineMode":null,"Percentage":null,"Function":null,"LinkCode":"5","ShowTabIndex":"5","Circularize":null,"ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'select', NULL, NULL, NULL, 0, NULL, N'5', NULL, N'5', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'd96e156a-eb1c-6020-d955-375393111a8c', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'结束', N'Roles', N'0', 1, 100, N'结束', N'{"id":"d96e156a-eb1c-6020-d955-375393111a8c","StepName":"结束","process_name":"结束","conditions":null,"process_to":"","AuditNorm":"Roles","style":"left: 435.22px; top: 596.17px; color: rgb(14, 118, 168);","AuditId":"0","StepStatus":100,"Description":"结束","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":"close","Power":"readonly","RunMode":"auto","JoinMode":"select","ExamineMode":"onlyone","Percentage":null,"Function":null,"LinkCode":"100","ShowTabIndex":null,"Circularize":null,"ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'auto', NULL, NULL, NULL, 0, NULL, N'100', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'6041143c-4c88-3810-911e-3f49cc2291d2', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤四', N'startuser', NULL, 1, 10, N'新增步骤', N'{"id":"6041143c-4c88-3810-911e-3f49cc2291d2","StepName":"步骤四","process_name":"步骤四","conditions":"539c567e-e218-4dd5-dc69-22ec3b90d8b0:1:同意:","process_to":"539c567e-e218-4dd5-dc69-22ec3b90d8b0","AuditNorm":"startuser","style":"left: 436px; top: 416px; color: rgb(14, 118, 168);","AuditId":null,"StepStatus":10,"Description":"新增步骤","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":null,"Power":null,"RunMode":"select","JoinMode":null,"ExamineMode":null,"Percentage":null,"Function":null,"LinkCode":"4","ShowTabIndex":"4","Circularize":null,"ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'select', NULL, NULL, NULL, 0, NULL, N'4', NULL, N'4', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤一', N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', 1, 10, N'新增步骤', N'{"id":"9cc2eafe-4abe-3de3-5567-40258f7d6483","StepName":"步骤一","process_name":"步骤一","conditions":"f3996e42-601e-f1fc-4d54-97d8bbe8a475:1:同意:","process_to":"f3996e42-601e-f1fc-4d54-97d8bbe8a475","AuditNorm":"Roles","style":"left: 436px; top: 138px; color: rgb(14, 118, 168);","AuditId":"3f9578c5-c0a2-4c7a-b0fd-c93fae47194b","StepStatus":10,"Description":"新增步骤","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":null,"Power":null,"RunMode":"auto","JoinMode":null,"ExamineMode":null,"Percentage":null,"Function":"Agency","LinkCode":"1","ShowTabIndex":"0,1","Circularize":"Circularize","ReminderTimeout":100,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":"Roles","AuditIdRead":"3f9578c5-c0a2-4c7a-b0fd-c93fae47194b"}', NULL, NULL, N'auto', NULL, NULL, NULL, 0, N'Agency', N'1', NULL, N'0,1', N'Circularize', 100, NULL, NULL, N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b')
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'34137caa-ef96-9158-e1cf-89ab1418de5d', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'起始', N'Users', N'5eeea4ce-71ab-4464-b72f-17f5163ee944', 1, 1, N'起始', N'{"id":"34137caa-ef96-9158-e1cf-89ab1418de5d","StepName":"起始","process_name":"起始","conditions":"9cc2eafe-4abe-3de3-5567-40258f7d6483:1:同意:","process_to":"9cc2eafe-4abe-3de3-5567-40258f7d6483","AuditNorm":"Users","style":"left: 435.22px; top: 53.32px; color: rgb(14, 118, 168);","AuditId":"5eeea4ce-71ab-4464-b72f-17f5163ee944","StepStatus":1,"Description":"起始","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":"close","Power":"readonly","RunMode":"auto","JoinMode":"select","ExamineMode":"onlyone","Percentage":null,"Function":"Agency","LinkCode":"0","ShowTabIndex":"0","Circularize":null,"ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'auto', NULL, NULL, NULL, 0, N'Agency', N'0', NULL, N'0', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'268aa12c-69b6-ae58-de7c-95aaef411003', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤三', N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', 1, 10, N'新增步骤', N'{"id":"268aa12c-69b6-ae58-de7c-95aaef411003","StepName":"步骤三","process_name":"步骤三","conditions":"6041143c-4c88-3810-911e-3f49cc2291d2:1:同意:","process_to":"6041143c-4c88-3810-911e-3f49cc2291d2","AuditNorm":"Roles","style":"left: 436px; top: 326px; color: rgb(14, 118, 168);","AuditId":"3f9578c5-c0a2-4c7a-b0fd-c93fae47194b","StepStatus":10,"Description":"新增步骤","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":null,"Power":null,"RunMode":"select","JoinMode":null,"ExamineMode":null,"Percentage":null,"Function":"Agency","LinkCode":"3","ShowTabIndex":"3","Circularize":"Circularize","ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'select', NULL, NULL, NULL, 0, N'Agency', N'3', NULL, N'3', N'Circularize', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStep] ([id], [mainId], [stepName], [auditNorm], [auditId], [auditors], [stepStatus], [description], [deployInfo], [openChoices], [power], [runMode], [joinMode], [examineMode], [relationStepKey], [percentage], [function], [linkCode], [auditLinkCode], [showTabIndex], [circularize], [reminderTimeout], [smsTemplateToDo], [smsTemplateRead], [auditNormRead], [auditIdRead]) VALUES (N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'步骤二', N'Roles', N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b', 1, 10, N'新增步骤', N'{"id":"f3996e42-601e-f1fc-4d54-97d8bbe8a475","StepName":"步骤二","process_name":"步骤二","conditions":"268aa12c-69b6-ae58-de7c-95aaef411003:1:同意:,9cc2eafe-4abe-3de3-5567-40258f7d6483:10:驳回:","process_to":"268aa12c-69b6-ae58-de7c-95aaef411003,9cc2eafe-4abe-3de3-5567-40258f7d6483","AuditNorm":"Roles","style":"left: 441px; top: 223px; color: rgb(14, 118, 168);","AuditId":"3f9578c5-c0a2-4c7a-b0fd-c93fae47194b","StepStatus":10,"Description":"新增步骤","RelationStepKey":null,"Auditors":1,"MainId":"b6cb2b24-7378-4671-b2d0-4dbb20f08d3e","OpenChoices":null,"Power":null,"RunMode":"select","JoinMode":null,"ExamineMode":null,"Percentage":null,"Function":null,"LinkCode":"2","ShowTabIndex":"1,2","Circularize":"Circularize","ReminderTimeout":null,"SMSTemplateToDo":null,"SMSTemplateRead":null,"AuditNormRead":null,"AuditIdRead":null}', NULL, NULL, N'select', NULL, NULL, NULL, 0, NULL, N'2', NULL, N'1,2', N'Circularize', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'2f61d4fe-a970-41f0-b251-09390e539bf6', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'6041143c-4c88-3810-911e-3f49cc2291d2', N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', 1, N'', N'', N'同意')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'cdccab71-9c3b-4c39-9db7-22714c311761', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', 10, N'', N'', N'驳回')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'6dfacd08-7f31-48bf-98f4-2edab3dbc0bf', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'268aa12c-69b6-ae58-de7c-95aaef411003', N'6041143c-4c88-3810-911e-3f49cc2291d2', 1, N'', N'', N'同意')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'3dd0a72e-2d4e-4f93-bbf4-457172ae756e', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'34137caa-ef96-9158-e1cf-89ab1418de5d', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', 1, N'', N'', N'同意')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'f048c1b4-2a69-4fe0-9901-8974b50415d6', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'9cc2eafe-4abe-3de3-5567-40258f7d6483', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', 1, N'', N'', N'同意')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'aa1963b5-e011-45a5-8795-8f92ac0c94bd', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'f3996e42-601e-f1fc-4d54-97d8bbe8a475', N'268aa12c-69b6-ae58-de7c-95aaef411003', 1, N'', N'', N'同意')
GO
INSERT [dbo].[FlowStepPath] ([id], [mainId], [upStep], [nextStep], [condition], [expression], [description], [btnName]) VALUES (N'64877b41-e1ec-4277-9eea-dbbbe192d798', N'b6cb2b24-7378-4671-b2d0-4dbb20f08d3e', N'539c567e-e218-4dd5-dc69-22ec3b90d8b0', N'd96e156a-eb1c-6020-d955-375393111a8c', 1, N'', N'', N'同意')
GO
