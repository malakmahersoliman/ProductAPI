namespace ProductAPI.Feature.Users.Commands.UpdateUser
{
    public enum UpdateUserResult
    {
        NotFound,
        EmailConflict,
        InvalidRole,
        Updated
    }
}
