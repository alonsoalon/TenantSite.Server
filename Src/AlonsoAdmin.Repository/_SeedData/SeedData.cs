using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Repository._SeedData
{
    public static class SeedData
    {

        

        public static List<SysUserEntity> UserEntities() {

            List<SysUserEntity> list = new List<SysUserEntity>();
            list.Add(new SysUserEntity {
                Id = "6650943897923698688",
                UserName = "system",
                Password = "96E79218965EB72C92A549DD5A33112",
                UserType = UserType.TenantAdmin,
                Avatar = "",


                CreatedBy = "6650943897923698688",
                CreatedByName = "INSTALL",
                DisplayName = "种子数据",
                IsDeleted = false,
                IsDisabled=false,

            });
            return list;
        }
        public static object[] Get() {

            


            return UserEntities().ToArray();
        }



    }
}
