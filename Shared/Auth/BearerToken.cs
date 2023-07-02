using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.Auth
{
    public class BearerToken
    {
        public string Access_Token { get; set; }
        public string Scope { get; set; }
        public long Expires_In { get; set; }
        public string Token_Type { get; set; }
    }
}