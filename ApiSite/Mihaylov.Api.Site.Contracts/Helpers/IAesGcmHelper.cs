namespace Mihaylov.Api.Site.Data.Helpers
{
    public interface IAesGcmHelper
    {
        byte[] Decrypt(byte[] combined, byte[] associatedData = null);
        
        byte[] Encrypt(byte[] plainData, byte[] associatedData = null);

        byte[] GenerateKey();
        
        void Test();
    }
}