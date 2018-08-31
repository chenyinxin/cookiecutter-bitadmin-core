using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowStepPath
    {
        public Guid Id { get; set; }
        public Guid? MainId { get; set; }
        public Guid? StartStepId { get; set; }
        public Guid? StopStepId { get; set; }
        public string Nikename { get; set; }
        public int Condition { get; set; }
        public string Expression { get; set; }
        public string Description { get; set; }
    }
}
