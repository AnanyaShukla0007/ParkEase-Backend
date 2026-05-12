namespace Auth.Domain.Enums;

public static class UserRoles
{
    public const string Driver = "DRIVER";
    public const string Manager = "MANAGER";
    public const string Admin = "ADMIN";

    public static readonly string[] All =
    {
        Driver,
        Manager,
        Admin
    };
}