using AlonsoAdmin.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlonsoAdmin.Common.Configs
{
    public class SystemConfig
    {
        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public bool WatchCurd { get; set; }

        /// <summary>
        /// 用于雪花算法ID生成 - 数据中心取值范围为0-31
        /// </summary>
        public uint DataCenterId { get; set; }

        /// <summary>
        /// 用于雪花算法ID生成 - 机器码取值范围为0-31
        /// </summary>
        public uint WorkId { get; set; }

        /// <summary>
        /// 启用Api访问控制
        /// </summary>
        public bool EnableApiAccessControl { get; set; }

        /// <summary>
        /// 头像上传配置参数
        /// </summary>
        public FileUploadConfig UploadAvatar { get; set; }

    }

    /// <summary>
    /// 文件上传配置
    /// </summary>
    public class FileUploadConfig
    {
        private string _uploadPath;
        /// <summary>
        /// 上传路径
        /// </summary>
        public string UploadPath
        {
            get
            {
                if (_uploadPath.IsNull())
                {
                    _uploadPath = Path.Combine(AppContext.BaseDirectory, "upload").ToPath();
                }

                if (!Path.IsPathRooted(_uploadPath))
                {
                    _uploadPath = Path.Combine(AppContext.BaseDirectory, _uploadPath).ToPath();
                }

                return _uploadPath;
            }
            set => _uploadPath = value;
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// 路径格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 路径日期格式
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// 文件大小 10M = 10 * 1024 * 1024
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// 最大允许上传个数 -1不限制
        /// </summary>
        public int Limit { get; set; } = -1;

        /// <summary>
        /// 文件格式
        /// </summary>
        public string[] ContentType { get; set; }
    }
}
