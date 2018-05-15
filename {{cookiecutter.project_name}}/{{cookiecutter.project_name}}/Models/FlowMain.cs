using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowMain
    {
        public FlowMain()
        {
            FlowStep = new HashSet<FlowStep>();
            FlowStepPath = new HashSet<FlowStepPath>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<FlowStep> FlowStep { get; set; }
        public ICollection<FlowStepPath> FlowStepPath { get; set; }
    }
}
