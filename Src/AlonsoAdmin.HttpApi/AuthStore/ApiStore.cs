using AlonsoAdmin.Services.System;
using AlonsoAdmin.Services.System.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.AuthStore
{
    public class ApiStore
    {
        private ISysApiService SysApiService { get; }
        public ApiStore(ISysApiService sysApiService)
        {
            SysApiService = sysApiService;
        }

        private static readonly List<ApiItem> _apis = new List<ApiItem>()
        {
            new ApiItem {
                Api="authtest/tenant",
                Roles=new List<string>(new string[]{"admin","test1" })
            }
        };

        public List<ApiItem> GetAll()
        {
            return _apis;
        }

        public ApiItem Find(string id)
        {
            
            var a = SysApiService.GetList();
            return _apis.Find(_ => _.Api == id);
        }
    }

    public class ApiItem
    {

        public string Api { get; set; }
        public IEnumerable<string> Roles { get; set; }
        //Roles=new List<string>(new string[]{"admin","test" }) 
    }
}
