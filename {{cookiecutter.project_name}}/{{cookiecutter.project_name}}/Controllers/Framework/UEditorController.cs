/***********************
 * BitAdmin2.0框架文件
 ***********************/
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UEditorNetCore;

namespace {{cookiecutter.project_name}}.Controllers
{
    [Route("[controller]")]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        public void Do()
        {
            ue.DoAction(HttpContext);
        }
    }
}
