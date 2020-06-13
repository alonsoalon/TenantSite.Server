using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.File;
using AlonsoAdmin.Common.ResponseEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlonsoAdmin.Common.Upload
{
    public interface IUploadTool
    {

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IResponseEntity<FileInfo>> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken = default);



        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IResponseEntity<FileInfo>> UploadFileAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default);






    }
}
