using System;

namespace AlonsoAdmin.MultiTenant.Error
{
    public class MultiTenantException: Exception
    {
        public MultiTenantException(string message, Exception innerException = null): base(message, innerException) 
        { 
        
        }
    }
}
