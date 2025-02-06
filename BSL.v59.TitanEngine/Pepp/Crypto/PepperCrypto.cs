using BSL.v59.TitanEngine.NaCl;

namespace BSL.v59.TitanEngine.Pepp.Crypto;

public static class PepperCrypto
{
    public static byte[] Secretbox(byte[] a1, byte[] a2, byte[] a3)
    {
        return TweetNaCl.crypto_secretbox_xsalsa19poly1305_tweet(a1, a2, a3);
    }

    public static byte[] Secretbox_open(byte[] a1, byte[] a2, byte[] a3, int a4)
    {
        return TweetNaCl.crypto_secretbox_xsalsa19poly1305_tweet_open(a1, a2, a3);
    }
}