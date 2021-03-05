using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public static class Constants
    {
        public const string ISSUER = AUDIANCE;
        public const string AUDIANCE = @"https://localhost:44373/";
        public const string SECRET = "not_too_short_secret_otherwise_it_might_error";
    }
}
