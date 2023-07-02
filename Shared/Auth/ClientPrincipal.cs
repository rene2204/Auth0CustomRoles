using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Auth
{
    public class ClientPrincipal
    {
        public string IdentityProvider { get; set; }
        public string UserId { get; set; }
        public string UserDetails { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<Dictionary<string, string>> Claims { get; set; }
        public string AccessToken { get; set; }
    }
}