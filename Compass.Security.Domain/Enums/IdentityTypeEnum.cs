namespace Compass.Security.Domain.Enums
{
    public enum IdentityTypeEnum
    {
        Succeeded,
        IsNotAllowed,
        IsLockedOut,
        RequiresTwoFactor,
        Failed
    }
}