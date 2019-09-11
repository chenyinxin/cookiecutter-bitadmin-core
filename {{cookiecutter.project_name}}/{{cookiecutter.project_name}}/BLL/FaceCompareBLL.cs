using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace {{cookiecutter.project_name}}.BLL
{
    /// <summary>
    /// 人脸识别业务逻辑
    /// </summary>
    public class FaceCompareBLL
    {
        #region 更新人脸特征
        /// <summary>
        /// 添加人脸特征，更新客户端用户人脸特征库
        /// </summary>
        public static void AddUserFace(SysUser user)
        {
            DataContext dbContext = new DataContext();

            var faceFeature = FaceCompareHelper.ExtractFeature(HttpContextCore.MapPath(user.UserImage));
            if (string.IsNullOrEmpty(faceFeature))
                return;

            //保存人脸特征
            var feature = dbContext.Set<SysUserFaceFeature>().FirstOrDefault(x => x.UserId == user.UserId);
            if (feature == null)
            {
                feature = new SysUserFaceFeature
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserType = "User",
                    FaceImage = user.UserImage,
                    FaceFeature = faceFeature,
                    FaceFeatureType = "",
                    FaceTimeOut = DateTime.Now.AddYears(100),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                dbContext.Set<SysUserFaceFeature>().Add(feature);
            }
            else
            {
                feature.UserName = user.UserName;
                feature.FaceImage = user.UserImage;
                feature.FaceFeature = faceFeature;
                feature.UpdateTime = DateTime.Now;
            }

            //更新客户端队列（根据业务修改）
            var data = new Dictionary<string, string>
            {
                ["ActionName"] = "AddFace",
                ["Id"] = user.UserId.ToString(),
                ["Name"] = user.UserName,
                ["ImageUrl"] = user.UserImage,
                ["FaceFeature"] = faceFeature,
                ["TimeOut"] = DateTime.Now.AddYears(100).ToString()
            };

            SysQueue queue = new SysQueue
            {
                Id = Guid.NewGuid(),
                ClientId = user.DepartmentId.ToString(),
                ActionName = "AddFace",
                ActionObjectId = user.UserId.ToString(),
                ActionData = JsonConvert.SerializeObject(data),
                CreateTime = DateTime.Now
            };

            dbContext.Set<SysQueue>().Add(queue);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除人脸特征，更新客户端用户人脸特征库
        /// </summary>
        /// <param name="user"></param>
        public static void DeleteUserFace(SysUser user)
        {
            DataContext dbContext = new DataContext();

            //更新客户端队列（根据业务修改）
            var data = new Dictionary<string, string>
            {
                ["ActionName"] = "DeleteFace",
                ["Id"] = user.UserId.ToString()
            };

            SysQueue queue = new SysQueue
            {
                Id = Guid.NewGuid(),
                ClientId = user.DepartmentId.ToString(),
                ActionName = "DeleteFace",
                ActionObjectId = user.UserId.ToString(),
                ActionData = JsonConvert.SerializeObject(data),
                CreateTime = DateTime.Now
            };

            dbContext.Set<SysQueue>().Add(queue);
            dbContext.SaveChanges();
        } 
        #endregion
    }
}
