using System;
using BSL.v59.TitanEngine.Ciph;
using BSL.v59.TitanEngine.NaCl;
using BSL.v59.TitanEngine.Pepp.Crypto;

namespace BSL.v59.TitanEngine.Pepp;

public class PepperEncrypter(byte[] key, byte[] nonce) : StreamEncrypter
{
    public int Encrypt(byte[] input, byte[] output, int length)
    {
        if (length <= 0) return 0;

        NextNonce(nonce);
        try
        {
            var v1 = PepperCrypto.Secretbox(input, nonce, key);
            Buffer.BlockCopy(v1, 0, output, 0, v1.Length);
        }
        catch (TweetNaCl.InvalidCipherTextException)
        {
            return -1;
        }

        return 0;
    }

    public int Decrypt(byte[] input, byte[] output, int length)
    {
        if (length <= 0) return 0;
        NextNonce(nonce);

        try
        {
            var v1 = PepperCrypto.Secretbox_open(input, nonce, key, 16);
            Buffer.BlockCopy(v1, 0, output, 0, v1.Length);
        }
        catch (TweetNaCl.InvalidCipherTextException)
        {
            return -1;
        }

        return 0;
    }

    public void NextNonce(byte[] nonce)
    {
        var v8 = 2;
        for (var idx = 0; idx < 24; idx++)
        {
            var v10 = v8 + (nonce[idx] & 0xFF);
            nonce[idx] = (byte)(v10 & 0xFF);
            v8 = v10 / 0x100;
            if (v8 == 0) break;
        }
    }

    public override int GetEncryptionOverhead()
    {
        return 16;
    }
}