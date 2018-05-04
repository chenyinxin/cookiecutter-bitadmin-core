using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowStepPath
    {
        public Guid Id { get; set; }
        public string MainId { get; set; }
        public string UpStep { get; set; }
        public string NextStep { get; set; }
        public int Condition { get; set; }
        public string Expression { get; set; }
        public string Description { get; set; }
        public string BtnName { get; set; }
    }
}
