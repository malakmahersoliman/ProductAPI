using ProductAPI.Common.Secuirty;

namespace ProductAPI.services
{

    public interface ICurrentUserService
    {

        CurrentUser GetCurrentUser();

    }
}
