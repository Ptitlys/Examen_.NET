namespace Examen.Exceptions;

public class ExpiredOrDeactivatedToken : Exception
{
    public ExpiredOrDeactivatedToken() : base("Token is expired or deactivated.")
    {
    }
}