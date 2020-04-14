using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Auth
{
    /// <summary>
    /// 实现类 <see cref="IAuthenticationSchemeProvider"/>.
    /// </summary>
    internal class MultiTenantAuthenticationSchemeProvider : IAuthenticationSchemeProvider
    {
        private readonly IOptions<AuthenticationOptions> _optionsProvider;
        private readonly object _lock = new object();

        private readonly IDictionary<string, AuthenticationScheme> _schemes;
        private readonly List<AuthenticationScheme> _requestHandlers;


        public MultiTenantAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options)
            : this(options, new Dictionary<string, AuthenticationScheme>(StringComparer.Ordinal))
        {
        }

        protected MultiTenantAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, IDictionary<string, AuthenticationScheme> schemes)
        {
            _optionsProvider = options;

            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _requestHandlers = new List<AuthenticationScheme>();

            foreach (var builder in _optionsProvider.Value.Schemes)
            {
                var scheme = builder.Build();
                AddScheme(scheme);
            }
        }

        /// <summary>
        /// 返回该租户的默认方案 
        /// </summary>
        /// <returns></returns>
        private Task<AuthenticationScheme> GetDefaultSchemeAsync()
            => _optionsProvider.Value.DefaultScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultScheme)
            : Task.FromResult<AuthenticationScheme>(null);

        /// <summary>
        /// 返回该租户的Authenticate方案 <see cref="IAuthenticationService.AuthenticateAsync(HttpContext, string)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync()
            => _optionsProvider.Value.DefaultAuthenticateScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultAuthenticateScheme)
            : GetDefaultSchemeAsync();

        /// <summary>
        /// 返回该租户的Challenge方案 <see cref="IAuthenticationService.ChallengeAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync()
            => _optionsProvider.Value.DefaultChallengeScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultChallengeScheme)
            : GetDefaultSchemeAsync();

        /// <summary>
        /// 返回该租户的Forbid方案，<see cref="IAuthenticationService.ForbidAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetDefaultForbidSchemeAsync()
            => _optionsProvider.Value.DefaultForbidScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultForbidScheme)
            : GetDefaultChallengeSchemeAsync();

        /// <summary>
        /// 返回该租户的SignIn方案，<see cref="IAuthenticationService.SignInAsync(HttpContext, string, System.Security.Claims.ClaimsPrincipal, AuthenticationProperties)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetDefaultSignInSchemeAsync()
            => _optionsProvider.Value.DefaultSignInScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultSignInScheme)
            : GetDefaultSchemeAsync();

        /// <summary>
        /// 返回该租户的SignOut方案， <see cref="IAuthenticationService.SignOutAsync(HttpContext, string, AuthenticationProperties)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync()
             => _optionsProvider.Value.DefaultSignOutScheme != null
            ? GetSchemeAsync(_optionsProvider.Value.DefaultSignOutScheme)
            : GetDefaultSignInSchemeAsync();

        /// <summary>
        /// 返回<see cref="AuthenticationScheme"/>匹配的名称，或null。
        /// </summary>
        /// <param name="name">验证方案的名称</param>
        /// <returns></returns>
        public virtual Task<AuthenticationScheme> GetSchemeAsync(string name)
            => Task.FromResult(_schemes.ContainsKey(name) ? _schemes[name] : null);


        /// <summary>
        /// 按请求处理的优先顺序返回此租户的方案。
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync()
            => Task.FromResult<IEnumerable<AuthenticationScheme>>(_requestHandlers);
        /// <summary>
        /// 注册一个方案 <see cref="IAuthenticationService"/>. 
        /// </summary>
        /// <param name="scheme">方案名称</param>
        public virtual void AddScheme(AuthenticationScheme scheme)
        {
            if (_schemes.ContainsKey(scheme.Name))
            {
                throw new InvalidOperationException("方案已存在: " + scheme.Name);
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(scheme.Name))
                {
                    throw new InvalidOperationException("方案已存在: " + scheme.Name);
                }
                if (typeof(IAuthenticationRequestHandler).IsAssignableFrom(scheme.HandlerType))
                {
                    _requestHandlers.Add(scheme);
                }
                _schemes[scheme.Name] = scheme;
            }
        }


        /// <summary>
        /// 移除一个方案 <see cref="IAuthenticationService"/>.
        /// </summary>
        /// <param name="name"></param>
        public virtual void RemoveScheme(string name)
        {
            if (!_schemes.ContainsKey(name))
            {
                return;
            }
            lock (_lock)
            {
                if (_schemes.ContainsKey(name))
                {
                    var scheme = _schemes[name];
                    _requestHandlers.Remove(scheme);
                    _schemes.Remove(name);
                }
            }
        }

        /// <summary>
        /// 得到所有方案
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync()
            => Task.FromResult<IEnumerable<AuthenticationScheme>>(_schemes.Values);

    }
}
