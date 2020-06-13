using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.File;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = AlonsoAdmin.Common.File.FileInfo;

namespace AlonsoAdmin.Common.Upload
{
    public class UploadTool:IUploadTool
    {
        private readonly IOptionsMonitor<SystemConfig> _systemConfig;
        private readonly IAuthUser _authUser;
        public UploadTool(IOptionsMonitor<SystemConfig> systemConfig, IAuthUser authUser)
        {
            _systemConfig = systemConfig;
            _authUser = authUser;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
        }

        



        public async Task<IResponseEntity<FileInfo>> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var res = new ResponseEntity<FileInfo>();
            var config = _systemConfig.CurrentValue.UploadAvatar;

            if (file == null || file.Length < 1)
            {
                return res.Error("请上传文件！");
            }

            //格式限制
            if (!config.ContentType.Contains(file.ContentType))
            {
                return res.Error("文件格式错误");
            }

            //大小限制
            if (!(file.Length <= config.MaxSize))
            {
                return res.Error("文件过大");
            }

            var fileInfo = new File.FileInfo(file.FileName, file.Length);
            fileInfo.UploadPath = config.UploadPath;
            fileInfo.RequestPath = config.RequestPath;
            fileInfo.RelativePath = _authUser.Tenant.Id;
            fileInfo.SaveName = $"{_authUser.Id}.{fileInfo.Extension}";

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }

            await SaveAsync(file, fileInfo.FilePath, cancellationToken);
            return res.Ok(fileInfo);
        }

        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponseEntity<FileInfo>> UploadFileAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
        {
            var res = new ResponseEntity<FileInfo>();

            if (file == null || file.Length < 1)
            {
                return res.Error("请上传文件！");
            }

            //格式限制
            if (!config.ContentType.Contains(file.ContentType))
            {
                return res.Error("文件格式错误");
            }

            //大小限制
            if (!(file.Length <= config.MaxSize))
            {
                return res.Error("文件过大");
            }

            var fileInfo = new File.FileInfo(file.FileName, file.Length)
            {
                UploadPath = config.UploadPath,
                RequestPath = config.RequestPath
            };

            var dateTimeFormat = config.DateTimeFormat.IsNotNull() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
            var format = config.Format.IsNotNull() ? StringHelper.Format(config.Format, args) : "";
            fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).ToPath();

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }



            var dataCenterId = _systemConfig.CurrentValue?.DataCenterId ?? 5;
            var workId = _systemConfig.CurrentValue?.WorkId ?? 20;

            fileInfo.SaveName = $"{IdHelper.GenSnowflakeId(dataCenterId, workId)}.{fileInfo.Extension}";

            await SaveAsync(file, fileInfo.FilePath, cancellationToken);

            return res.Ok(fileInfo);
        }




    }
}
