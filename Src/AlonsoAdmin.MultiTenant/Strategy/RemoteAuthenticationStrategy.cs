using AlonsoAdmin.MultiTenant.Error;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class RemoteAuthenticationStrategy : IMultiTenantStrategy
    {
        private readonly ILogger<RemoteAuthenticationStrategy> logger;

        public RemoteAuthenticationStrategy()
        {
        }

        public RemoteAuthenticationStrategy(ILogger<RemoteAuthenticationStrategy> logger)
        {
            this.logger = logger;
        }

        public async virtual Task<string> GetIdentifierAsync(object context)
        {
            if (!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException($"\"{nameof(context)}\" 必须是HttpContext类型", nameof(context)));

            var httpContext = context as HttpContext;

            var schemes = httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();

            foreach (var scheme in await schemes.GetRequestHandlerSchemesAsync())
            {
                var optionType = scheme.HandlerType.GetProperty("Options").PropertyType;

                // Skip if this is not a compatible type of authentication.
                if (!typeof(OAuthOptions).IsAssignableFrom(optionType) &&
                    !typeof(OpenIdConnectOptions).IsAssignableFrom(optionType))
                {
                    continue;
                }

                // RequestHandlers have a method, ShouldHandleRequestAsync, which would be nice here,
                // but instantiating the handler internally caches an Options instance... which is bad because
                // we don't know the tenant yet. Thus we will get the Options ourselves with reflection,
                // and replicate the ShouldHandleRequestAsync logic.

                var optionsMonitorType = typeof(IOptionsMonitor<>).MakeGenericType(optionType);
                var optionsMonitor = httpContext.RequestServices.GetRequiredService(optionsMonitorType);
                var options = optionsMonitorType.GetMethod("Get").Invoke(optionsMonitor, new[] { scheme.Name }) as RemoteAuthenticationOptions;

                if (options.CallbackPath == httpContext.Request.Path)
                {
                    try
                    {
                        string state = null;

                        if (string.Equals(httpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
                        {
                            state = httpContext.Request.Query["state"];
                        }
                        else if (string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
                            && httpContext.Request.HasFormContentType
                            && httpContext.Request.Body.CanRead)
                        {
                            var formOptions = new FormOptions { BufferBody = true };
                            var form = await httpContext.Request.ReadFormAsync(formOptions);
                            state = form.Where(i => i.Key.ToLowerInvariant() == "state").Single().Value;
                        }

                        var oAuthOptions = options as OAuthOptions;
                        var openIdConnectOptions = options as OpenIdConnectOptions;

                        var properties = oAuthOptions?.StateDataFormat.Unprotect(state) ??
                                         openIdConnectOptions?.StateDataFormat.Unprotect(state);

                        if (properties == null)
                        {
                            logger.LogWarning("A tenant could not be determined because no state paraameter passed with the remote authentication callback.");
                            return null;
                        }

                        if (properties.Items.Keys.Contains("tenantId"))
                        {
                            return properties.Items["tenantId"] as string;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new MultiTenantException("Error occurred resolving tenant for remote authentication.", e);
                    }
                }
            }

            return null;
        }
    }
}
