namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public interface IQueryCipher
    {
        bool OnlyCriptedCalls { get; }

        string DecryptQuery(string value);
        string EncryptQuery(string value);
    }
}