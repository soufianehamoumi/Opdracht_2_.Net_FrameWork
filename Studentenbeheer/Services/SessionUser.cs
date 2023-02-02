using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;

namespace Studentenbeheer.Controllers
{
    public class SessionUser 
    {
        class UserStats
        {
            public DateTime LastEntered { get; set; }
            public int Count { get; set; }
            public StudentenbeheerUser User { get; set; }
        }


        readonly RequestDelegate _next;
        static Dictionary<string, UserStats> UserDictionary = new Dictionary<string, UserStats>();

        public SessionUser(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IdentityContext dbContext)
        {
            string name = httpContext.User.Identity.Name == null ? "-" : httpContext.User.Identity.Name;
            try
            {
                UserStats us = UserDictionary[name];
                us.Count++;
                us.LastEntered = DateTime.Now;
            }
            catch
            {
                UserDictionary[name] = new UserStats
                {
                    User = dbContext.Users.FirstOrDefault(u => u.UserName == name),
                    Count = 1,
                    LastEntered = DateTime.Now
                };
            }

            await _next(httpContext);
        }

        public static StudentenbeheerUser GetUser(HttpContext httpContext)
        {
            return UserDictionary[httpContext.User.Identity.Name == null ? "-" : httpContext.User.Identity.Name].User;
        }
    }
}

    

