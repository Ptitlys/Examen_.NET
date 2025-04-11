namespace Examen.Exceptions;

public class RegistrationFailedException : Exception
{
    public RegistrationFailedException() : base("Registration failed")
    {
    }
}