
--模块、页面
INSERT [dbo].[SysModule] ([moduleId], [parentId], [moduleName], [moduleIcon], [description], [orderNo]) VALUES (N'60f51c70-f468-4aae-9c6c-31a90bb937eb', NULL, N'项目管理', N'fa-file-powerpoint-o', NULL, 11)

INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'a4cf48ea-8287-4810-9842-0f8d6c8a9670', N'60f51c70-f468-4aae-9c6c-31a90bb937eb', N'project', N'项目管理', N'fa-list-alt', N'/pm/project', NULL, 1)
INSERT [dbo].[SysModulePage] ([id], [moduleId], [pageSign], [pageName], [pageIcon], [pageUrl], [description], [orderNo]) VALUES (N'2ec373e9-29f3-4050-ae7f-e3737ece1234', N'60f51c70-f468-4aae-9c6c-31a90bb937eb', N'task', N'任务管理', N'fa-list-ul', N'/pm/task', NULL, 2)

--初始化权限
declare @roleId uniqueidentifier
set @roleId=N'3f9578c5-c0a2-4c7a-b0fd-c93fae47194b'

declare mcursor cursor scroll for select id,moduleId from SysModulePage where id not in (select pageId from SysPageOperation) 
open mcursor
declare @id uniqueidentifier
declare @pid uniqueidentifier
declare @sign nvarchar(512)
fetch next from mcursor into @id,@pid
while @@FETCH_STATUS=0
begin
	INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'query')
	INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'add')
	INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'save')
	INSERT [dbo].[SysPageOperation] ([id], [pageID], [operationSign]) VALUES (newid(),@id, N'delete')
	set @sign='query,add,save,delete'
	
	INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) VALUES (newid(), @roleId, @id, @pid, @sign)
    fetch next from mcursor into @id,@pid
end 
close mcursor
deallocate mcursor

INSERT [dbo].[SysRoleOperatePower] ([id], [roleId], [modulePageId], [moduleParentId], [operationSign]) select newid(), @roleId, moduleId, parentId, 'query' from SysModule where moduleId not in (select modulePageId from SysRoleOperatePower)
