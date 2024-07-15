using System.Text;

namespace StackUnderFlow.Application.Security;

public class Cryptographer : ICryptographer
{
    public string Encrypt(string password)
    {
        var data = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(data);
    }

    public string Decrypt(string password)
    {
        var data = Convert.FromBase64String(password);
        return Encoding.UTF8.GetString(data);
    }
}