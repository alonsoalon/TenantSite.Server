using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Services.Dictionary.Request;
using AlonsoAdmin.Services.Dictionary.Response;
using AutoMapper;
namespace AlonsoAdmin.Services.Dictionary
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            #region Dictionary 字典头 映射
            // 创建 用到的映射 (DTO -> DO)
            CreateMap<DictionaryHeaderAddRequest, DictionaryHeaderEntity>();
            // 更新 用到的映射 (DTO -> DO)
            CreateMap<DictionaryHeaderEditRequest, DictionaryHeaderEntity>();
            // 查询列表 用到的映射 (DO -> DTO)
            CreateMap<DictionaryHeaderEntity, DictionaryHeaderForListResponse>();
            // 查询单条明细 用到的映射 (DO -> DTO)
            CreateMap<DictionaryHeaderEntity, DictionaryHeaderForItemResponse>();
            #endregion

            #region DictionaryEntry 字典条目 映射
            // 创建 用到的映射 (DTO -> DO)
            CreateMap<DictionaryEntryAddRequest, DictionaryEntryEntity>();
            // 更新 用到的映射 (DTO -> DO)
            CreateMap<DictionaryEntryEditRequest, DictionaryEntryEntity>();
            // 查询列表 用到的映射 (DO -> DTO)
            CreateMap<DictionaryEntryEntity, DictionaryEntryForListResponse>();
            // 查询单条明细 用到的映射 (DO -> DTO)
            CreateMap<DictionaryEntryEntity, DictionaryEntryForItemResponse>();
            #endregion

        }
    }
}
