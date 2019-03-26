using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using SDSFoundation.Model.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace SDSFoundation.Base.AspNetCore
{
    public abstract class AspNetCoreStartupBase<TProgram> where TProgram : class
    {
        public static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TProgram));
        public AspNetCoreStartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; protected set; }

        public AuthenticationBuilder ConfigureAuthenticationServices(IServiceCollection services, string loginPath = "/signin")
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = new PathString(loginPath);
            })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = "Global";
                options.ClientSecret = "f066481c-2cea-47c7-a049-e7ab14ac2038";

                options.RequireHttpsMetadata = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                // Use the authorization code flow.
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                // Note: setting the Authority allows the OIDC client middleware to automatically
                // retrieve the identity provider's configuration and spare you from setting
                // the different endpoints URIs or the token validation parameters explicitly.
                options.Authority = "https://globalservicessecuritydemo.azurewebsites.net/";

                //options.Authority = "http://localhost:54540/";


                var authorizedScopes = Enum.GetNames(typeof(ClaimType)).ToList();

                foreach (var scope in authorizedScopes)
                {
                    options.Scope.Add(scope);

                    options.ClaimActions.MapCustomJson(scope, jobj =>
                    {
                        var result = GetJobObjectValuesDelimited(scope, jobj);

                        return result;

                    });
                }


                options.SecurityTokenValidator = new JwtSecurityTokenHandler
                {
                    // Disable the built-in JWT claims mapping feature.
                    InboundClaimTypeMap = new Dictionary<string, string>()
                };

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";

            });

            return authenticationBuilder;
        }

        public IApplicationBuilder ConfigureApplicationSecurity(IApplicationBuilder app)
        {
            app = app.UseAuthentication();
            app.UseCookiePolicy();
            return app;
        }

        private static string GetJobObjectValuesDelimited(string key, JObject jobj)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (jobj != null && jobj[key] != null)
                {
                    IEnumerable<string> values = jobj[key].Values<string>();


                    if (values != null && values.Count() > 0)
                    {
                        foreach (string val in values)
                        {
                            sb.Append(val + ",");
                        }
                    }
                }


                return sb.ToString().TrimEnd(',');
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }

        }

    }
}
