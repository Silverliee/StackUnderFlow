namespace StackUnderFlow.Application.Security;

public interface ICryptographer
{
    public string Encrypt(string password);

    public string Decrypt(string password);
}