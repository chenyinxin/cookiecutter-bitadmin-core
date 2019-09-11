using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysModulePage
    {
        public Guid Id { get; set; }
        public Guid? ModuleId { get; set; }
        public string PageSign { get; set; }
        public string PageName { get; set; }
        public string PageIcon { get; set; }
        public string PageUrl { get; set; }
        public string Description { get; set; }
        public int? OrderNo { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
