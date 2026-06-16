using ProductAPI.Common.Secuirty;

namespace ProductAPI.services
{
    //create an interface to get the current user information from the HttpContext not
    ///implementing a database call to get the user information from the database
    ///in the services
    public interface ICurrentUserService
    {

        CurrentUser GetCurrentUser();

    }
}
