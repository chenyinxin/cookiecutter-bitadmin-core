using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class GeneralExample
    {
        public Guid ExampleId { get; set; }
        public string ExampleName { get; set; }
        public string PlainText { get; set; }
        public string RequiredText { get; set; }
        public string RepeatText { get; set; }
        public string MobileText { get; set; }
        public string EmailText { get; set; }
        public int? PlainInt { get; set; }
        public decimal? PlainDecimal { get; set; }
        public DateTime? DateTimePicker { get; set; }
        public DateTime? DateTimePickerDate { get; set; }
        public DateTime? DateTimePickerYymm { get; set; }
        public DateTime? DateTimePickerFormatter { get; set; }
        public string UserPicker { get; set; }
        public string OuPicker { get; set; }
        public string ExampleText { get; set; }
        public string ExampleRadio { get; set; }
        public string ExampleCheckbox { get; set; }
        public string ExampleSelect { get; set; }
        public string ExamplePhone { get; set; }
        public DateTime? ExampleTime { get; set; }
        public string ExampleUser { get; set; }
        public string Department { get; set; }
        public string AutoComplete { get; set; }
        public string AutoComSelectText { get; set; }
        public string AutoComSelect { get; set; }
        public string LinkageSelectA { get; set; }
        public string LinkageSelectB { get; set; }
        public string LinkageSelectC { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LinkageSelectD { get; set; }
        public string LinkageSelectE { get; set; }
        public string LinkageSelectF { get; set; }
        public Guid? ParentId { get; set; }
    }
}
