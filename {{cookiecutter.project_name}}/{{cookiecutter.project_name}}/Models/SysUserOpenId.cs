using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysUserOpenId
    {
        public string OpenId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? BindTime { get; set; }
    }
}
