using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class PmProjectUser
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string UserRole { get; set; }
    }
}
