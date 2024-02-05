using System.Security.Claims;
using Notes.Application.Interfaces;

namespace Notes.WebApi.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var id = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id?.ToString()) ? Guid.Empty : Guid.Parse(id.ToString());
        }
    }
}