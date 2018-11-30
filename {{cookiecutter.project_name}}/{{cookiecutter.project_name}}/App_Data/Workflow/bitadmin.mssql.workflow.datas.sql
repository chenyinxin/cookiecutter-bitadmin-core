
delete FlowMain where id<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'
delete FlowStep where mainId<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'
delete FlowStepPath where mainId<>'B6CB2B24-7378-4671-B2D0-4DBB20F08D3E'

--示例流程
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

--功能模块
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', NULL, N'工作台', N'fab fa-slack-hash', NULL, 1)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'5df0e325-9b8e-4d9f-81b4-abd8cd384ed7', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'todo', N'我的待办', N'far fa-check-circle', N'/workbench/todo', NULL, 1)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'4bee24e5-fa75-4573-8de6-e30597a10d16', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'done', N'我的已办', N'fas fa-check-circle', N'/workbench/done', NULL, 2)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'69714b72-95ed-45c3-88f7-19a25b3f2e0c', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'toread', N'我的待阅', N'far fa-user-circle', N'/workbench/toread', NULL, 3)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'56461589-3ad7-4f92-8bc1-62b2473daa78', N'b27cf48f-e85a-4cb2-afc6-cdefd75b3754', N'read', N'我的已阅', N'fas fa-user-circle', N'/workbench/read', NULL, 4)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'bd9f3eba-d177-4d73-ad37-ef5fefc044d0', N'52ee24b7-c2ff-44d0-9fee-3897d609d554', N'flowsetting', N'流程配置', N'fas fa-shekel-sign', N'/system/flowsetting', NULL, 5)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'63661fc7-7fba-4f64-be39-fb34021f2242', N'20256a37-fead-4d11-8052-f0df33738c3b', N'workflowexample', N'流程示例', N'fas fa-shekel-sign', N'/workflowexample/example', N'流程功能实现', 4)

--初始化权限
declare @roleId uniqueidentifier
set @roleId='3f9578c5-c0a2-4c7a-b0fd-c93fae47194b'

declare mcursor cursor scroll for select id,moduleId from SysModulePage where id not in (select pageId from SysPageOperation) 
open mcursor
declare @id uniqueidentifier
declare @pid uniqueidentifier
declare @sign nvarchar(512)
fetch next from mcursor into @id,@pid
while @@FETCH_STATUS=0
begin
	INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'query')
	set @sign='query'

	if @id='bd9f3eba-d177-4d73-ad37-ef5fefc044d0' or @id='63661fc7-7fba-4f64-be39-fb34021f2242'
	begin
		INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'add')
		INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'save')
		INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'delete')

		set @sign='query,add,save,delete'
	end
	
	INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (newid(), @roleId, @id, @pid, @sign)
    fetch next from mcursor into @id,@pid
end 
close mcursor
deallocate mcursor

INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) select newid(), @roleId, moduleId, parentId, 'query' from SysModule where moduleId not in (select modulePageId from SysRoleOperatePower)
