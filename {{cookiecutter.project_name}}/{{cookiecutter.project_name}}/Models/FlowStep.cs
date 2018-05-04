using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class FlowStep
    {
        public Guid Id { get; set; }
        public string MainId { get; set; }
        public string StepName { get; set; }
        public string AuditNorm { get; set; }
        public string AuditId { get; set; }
        public int Auditors { get; set; }
        public int StepStatus { get; set; }
        public string Description { get; set; }
        public string DeployInfo { get; set; }
        public string OpenChoices { get; set; }
        public string Power { get; set; }
        public string RunMode { get; set; }
        public string JoinMode { get; set; }
        public string ExamineMode { get; set; }
        public string RelationStepKey { get; set; }
        public int Percentage { get; set; }
        public string Function { get; set; }
        public string LinkCode { get; set; }
        public string AuditLinkCode { get; set; }
        public string ShowTabIndex { get; set; }
        public string Circularize { get; set; }
        public long? ReminderTimeout { get; set; }
        public string SmsTemplateToDo { get; set; }
        public string SmsTemplateRead { get; set; }
        public string AuditNormRead { get; set; }
        public string AuditIdRead { get; set; }
    }
}
