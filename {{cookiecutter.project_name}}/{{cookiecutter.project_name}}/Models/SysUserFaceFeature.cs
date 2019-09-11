using System;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Models
{
    public partial class SysUserFaceFeature
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string FaceImage { get; set; }
        public string FaceFeature { get; set; }
        public string FaceFeatureType { get; set; }
        public DateTime? FaceTimeOut { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
