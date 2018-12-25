using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysLog
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Description { get; set; }
    }
}
