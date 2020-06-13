using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Services.System.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Services.System.Response;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Repository.System.Interface;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Common.RequestEntity;
using Microsoft.AspNetCore.Http;
using FileInfo = AlonsoAdmin.Common.File.FileInfo;
using System.IO;
using System.Threading;
using AlonsoAdmin.Common.Upload;
using Microsoft.Extensions.Options;
using AlonsoAdmin.Common.Configs;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysUserService : ISysUserService
    {
        private readonly IAuthUser _authUser;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _sysUserRepository;
        private readonly IOptionsMonitor<SystemConfig> _systemConfig;
        private readonly IUploadTool _uploadTool;


        public SysUserService(
            IAuthUser authUser,
            ICache cache,
            IMapper mapper,
            ISysUserRepository sysUserRepository,
            IOptionsMonitor<SystemConfig> systemConfig,
            IUploadTool uploadTool
            )
        {
            _authUser = authUser;
            _cache = cache;
            _mapper = mapper;
            _sysUserRepository = sysUserRepository;
            _systemConfig = systemConfig;
            _uploadTool = uploadTool;
           

        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(UserAddRequest req)
        {
            var item = _mapper.Map<SysUserEntity>(req);
            item.Password= MD5Encrypt.Encrypt32(item.Password);
            var result = await _sysUserRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(UserEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }
            var entity = await _sysUserRepository.GetAsync(req.Id);
            if (entity==null|| entity.Id =="")
            {
                return ResponseEntity.Error("用户不存在!");
            }

            if (entity.PermissionId != req.PermissionId) {
                //清除权限岗的资源缓存
                await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);
                //清除权限岗的数据归属组缓存
                await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionGroupList);
            }

            _mapper.Map(req, entity);
            //var entity = _mapper.Map<SysUserEntity>(req);
            await _sysUserRepository.UpdateAsync(entity);

            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysUserRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysUserRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysUserRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysUserRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysUserRepository.GetAsync(id);
            var data = _mapper.Map<UserForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<UserFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysUserRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.UserName.Contains(key) || a.DisplayName.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Include(a=>a.Permission)
                .Count(out var total)
                .OrderBy(true, a => a.CreatedTime)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new ResponsePageEntity<UserForListResponse>()
            {
                List = _mapper.Map<List<UserForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(UserFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysUserRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.UserName.Contains(key) || a.DisplayName.Contains(key) ))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Include(a => a.Permission)
                .ToListAsync();

            var result = _mapper.Map<List<UserForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        /// <summary>
        /// 更新指定用户的密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> UserChangePasswordAsync(UserChangePasswordRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            if (req.Password != req.ConfirmPassword) {
                return ResponseEntity.Error("两次密码不一致，请重新输入");
            }
            var password = MD5Encrypt.Encrypt32(req.Password);
            var item = new SysUserEntity() { Id = req.Id, Revision = req.Revision };
            _sysUserRepository.Attach(item); //此时快照 item
            item.Password = password;
            await _sysUserRepository.UpdateAsync(item); //对比快照时的变化


            return ResponseEntity.Ok("更新成功");
        }

        /// <summary>
        /// 修改当前登录人的密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> ChangePasswordAsync(ChangePasswordRequest req)
        {
            var oldPassword = MD5Encrypt.Encrypt32(req.OldPassword);
            var item = _sysUserRepository.Where(a => a.Id == _authUser.Id && a.Password == oldPassword).First();
            if (item == null) {
                return ResponseEntity.Error("旧密码错误，请输入正确的旧密码");
            }
            if (req.NewPassword != req.ConfirmPassword)
            {
                return ResponseEntity.Error("两次密码不一致，请重新输入");
            }
            var newPassword = MD5Encrypt.Encrypt32(req.NewPassword);   
            item.Password = newPassword;
            await _sysUserRepository.UpdateAsync(item); 
            return ResponseEntity.Ok("更新成功");
        }

 

        public async Task<IResponseEntity<FileInfo>> UploadAvatarAsync(IFormFile file)
        {
            
    
            var res = await _uploadTool.UploadAvatarAsync(file);

            var item = _sysUserRepository.Where(a => a.Id == _authUser.Id).First(); //此时快照 item
            if (item == null)
            {
                return new ResponseEntity<FileInfo>().Error("找不到指定用户");
            }
            item.Avatar = res.Data.FileRelativePath;
            await _sysUserRepository.UpdateAsync(item); //对比快照时的变化

            return res;
        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> UpdateUserInfo(UserEditRequest req)
        {
            if (req == null )
            {
                return ResponseEntity.Error("更新的实体丢失");
            }

            //req.Id = _authUser.Id;

            var entity = await _sysUserRepository.GetAsync(_authUser.Id);
            if (entity == null || entity.Id == "")
            {
                return ResponseEntity.Error("用户不存在!");
            }

            entity.DisplayName = req.DisplayName;
            entity.Mobile = req.Mobile;
            entity.Mail = req.Mail;
            entity.Description = req.Description;   

            await _sysUserRepository.UpdateAsync(entity);

            return ResponseEntity.Ok(entity);
        }

        #endregion
    }
}
