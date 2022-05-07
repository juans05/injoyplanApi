using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace LAYHER.Backend.API.Helpers
{
    public class Utils
    {
        public static int GetLoggedUserId(IIdentity identity)
        {
            if (!identity.IsAuthenticated)
                throw new AuthenticationException();

            var claimsIdentity = identity as ClaimsIdentity;

            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return int.Parse(userId);
        }

        public static int GetLoggedClientId(IIdentity identity)
        {
            if (!identity.IsAuthenticated)
                throw new AuthenticationException();

            var claimsIdentity = identity as ClaimsIdentity;

            string clientId = claimsIdentity.FindFirst(ClaimTypes.PrimaryGroupSid).Value;

            return int.Parse(clientId);
        }
    }
}
