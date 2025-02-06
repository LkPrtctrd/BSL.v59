using BSL.v59.TitanEngine.NaCl;

namespace BSL.v59.TitanEngine.Pepp.Crypto;

public static class PepperKey
{
    static PepperKey()
    {
        ServerSecretKey = Convert.FromHexString("48d7d188d2c4b263233e1f0816bf231e8a3756e280a80083b47d8104e9d002da");

        ServerPublicKey = TweetNaCl.CryptoScalarmultBase(ServerSecretKey);
    }

    public static byte[] ServerSecretKey { get; }
    public static byte[] ServerPublicKey { get; }
}