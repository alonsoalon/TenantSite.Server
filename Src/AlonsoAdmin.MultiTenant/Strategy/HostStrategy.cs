using AlonsoAdmin.MultiTenant.Error;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class HostStrategy : IMultiTenantStrategy
    {
        private readonly string regex;

        public HostStrategy(string template)
        {
            // New in 2.1, match whole domain if just "__tenant__".
            if (template == "__tenant__")
            {
                template = template.Replace("__tenant__", @"(?<identifier>.+)");
            }
            else
            {
                // Check for valid template.
                // Template cannot be null or whitespace.
                if (string.IsNullOrWhiteSpace(template))
                {
                    throw new MultiTenantException("Template cannot be null or whitespace.");
                }
                // Wildcard "*" must be only occur once in template.
                if (Regex.Match(template, @"\*(?=.*\*)").Success)
                {
                    throw new MultiTenantException("Wildcard \"*\" must be only occur once in template.");
                }
                // Wildcard "*" must be only token in template segment.
                if (Regex.Match(template, @"\*[^\.]|[^\.]\*").Success)
                {
                    throw new MultiTenantException("\"*\" wildcard must be only token in template segment.");
                }
                // Wildcard "?" must be only token in template segment.
                if (Regex.Match(template, @"\?[^\.]|[^\.]\?").Success)
                {
                    throw new MultiTenantException("\"?\" wildcard must be only token in template segment.");
                }

                template = template.Trim().Replace(".", @"\.");
                string wildcardSegmentsPattern = @"(\.[^\.]+)*";
                string singleSegmentPattern = @"[^\.]+";
                if (template.Substring(template.Length - 3, 3) == @"\.*")
                {
                    template = template.Substring(0, template.Length - 3) + wildcardSegmentsPattern;
                }

                wildcardSegmentsPattern = @"([^\.]+\.)*";
                template = template.Replace(@"*\.", wildcardSegmentsPattern);
                template = template.Replace("?", singleSegmentPattern);
                template = template.Replace("__tenant__", @"(?<identifier>[^\.]+)");
            }

            this.regex = $"^{template}$";
        }

        public async Task<string> GetIdentifierAsync(object context)
        {
            if (!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException($"\"{nameof(context)}\" type must be of type HttpContext", nameof(context)));

            var host = (context as HttpContext).Request.Host;

            if (host.HasValue == false)
                return null;

            string identifier = null;

            var match = Regex.Match(host.Host, regex,
                RegexOptions.ExplicitCapture,
                TimeSpan.FromMilliseconds(100));

            if (match.Success)
            {
                identifier = match.Groups["identifier"].Value;
            }

            return await Task.FromResult(identifier);
        }
    }
}
