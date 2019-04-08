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
using SDSFoundation.Security.OpenIdDict.Base.Windows;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Base.AspNetCore
{

    public class SecureStartup<TProgram> : SecureProgram<TProgram> where TProgram : class
    {

        public SecureStartup(IConfiguration configuration)
        {
            var args = new List<string>().ToArray();
            //Initialize Application to guarantee secure connection
            var commandLineOptions = Initialize(args: args, appSettingsFileName: "appsettings.json", maximumLicenseAge: 12);

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
                options.ClientId = ClientId;
                options.ClientSecret = ClientSecret;

                options.RequireHttpsMetadata = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                // Use the authorization code flow.
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                // Note: setting the Authority allows the OIDC client middleware to automatically
                // retrieve the identity provider's configuration and spare you from setting
                // the different endpoints URIs or the token validation parameters explicitly.
                options.Authority = AuthorizationServer;

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
