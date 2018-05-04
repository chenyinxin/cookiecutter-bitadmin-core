using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysRoleUser
    {
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? UserId { get; set; }
    }
}
