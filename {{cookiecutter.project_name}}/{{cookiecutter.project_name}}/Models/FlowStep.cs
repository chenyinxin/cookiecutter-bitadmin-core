using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowStep
    {
        public Guid StepId { get; set; }
        public Guid? MainId { get; set; }
        public string StepName { get; set; }
        public int StepStatus { get; set; }
        public string Agency { get; set; }
        public string Circularize { get; set; }
        public string RunMode { get; set; }
        public string LinkCode { get; set; }
        public string ShowTabIndex { get; set; }
        public long? ReminderTimeout { get; set; }
        public string AuditNorm { get; set; }
        public string AuditId { get; set; }
        public string AuditNormRead { get; set; }
        public string AuditIdRead { get; set; }
        public string SmsTemplateToDo { get; set; }
        public string SmsTemplateRead { get; set; }
        public string Description { get; set; }
        public string Style { get; set; }

        public FlowMain Main { get; set; }
    }
}
