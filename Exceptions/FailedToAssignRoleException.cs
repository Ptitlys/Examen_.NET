namespace Examen.Exceptions;

public class FailedToAssignRoleException : Exception
{
    public FailedToAssignRoleException() : base("Failed to assign role")
    {
    }
}