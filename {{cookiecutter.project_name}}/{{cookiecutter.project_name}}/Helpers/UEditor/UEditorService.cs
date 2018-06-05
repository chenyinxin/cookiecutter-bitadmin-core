using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using {{cookiecutter.project_name}}.UEditor.Handlers;
using System;

namespace {{cookiecutter.project_name}}.UEditor
{
    public class UEditorService
    {
        private UEditorActionCollection actionList;

        public UEditorService(IHostingEnvironment env, UEditorActionCollection actions)
        {
            Config.WebRootPath = env.WebRootPath;
            //Config.WebRootPath = AppDomain.CurrentDomain.BaseDirectory;
            
            actionList = actions;
        }

        public void DoAction(HttpContext context)
        {
            var action = context.Request.Query["action"];
            if (actionList.ContainsKey(action))
                actionList[action].Invoke(context);
            else
                new NotSupportedHandler(context).Process();
        }
    }
}
