using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BSL.v59.TitanEngine.NaCl;

public abstract class TweetNaCl
{
    private static readonly long[] Gf0 = new long[16];
    private static readonly long[] Gf1 = [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    private static readonly byte[] _0 = new byte[16];

    private static readonly byte[] _9 =
        [9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    private static readonly long[] _121665 = [0xDB41, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    private static readonly long[] D =
    [
        0x78a3, 0x1359, 0x4dca, 0x75eb, 0xd8ab, 0x4141, 0x0a4d, 0x0070, 0xe898, 0x7779, 0x4079, 0x8cc7, 0xfe73, 0x2b6f,
        0x6cee, 0x5203
    ];

    private static readonly long[] D2 =
    [
        0xf159, 0x26b2, 0x9b94, 0xebd6, 0xb156, 0x8283, 0x149a, 0x00e0, 0xd130, 0xeef3, 0x80f2, 0x198e, 0xfce7, 0x56df,
        0xd9dc, 0x2406
    ];

    private static readonly long[] X =
    [
        0xd51a, 0x8f25, 0x2d60, 0xc956, 0xa7b2, 0x9525, 0xc760, 0x692c, 0xdc5c, 0xfdd6, 0xe231, 0xc0a4, 0x53fe, 0xcd6e,
        0x36d3, 0x2169
    ];

    private static readonly long[] Y =
    [
        0x6658, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666,
        0x6666, 0x6666
    ];

    private static readonly long[] I =
    [
        0xa0b0, 0x4a0e, 0x1b27, 0xc4ee, 0xe478, 0xad2f, 0x1806, 0x2f43, 0xd7a7, 0x3dfb, 0x0099, 0x2b4d, 0xdf0b, 0x4fc1,
        0x2480, 0x2b83
    ];

    private static readonly ulong[] K =
    [
        0x428a2f98d728ae22, 0x7137449123ef65cd, 0xb5c0fbcfec4d3b2f, 0xe9b5dba58189dbbc,
        0x3956c25bf348b538, 0x59f111f1b605d019, 0x923f82a4af194f9b, 0xab1c5ed5da6d8118,
        0xd807aa98a3030242, 0x12835b0145706fbe, 0x243185be4ee4b28c, 0x550c7dc3d5ffb4e2,
        0x72be5d74f27b896f, 0x80deb1fe3b1696b1, 0x9bdc06a725c71235, 0xc19bf174cf692694,
        0xe49b69c19ef14ad2, 0xefbe4786384f25e3, 0x0fc19dc68b8cd5b5, 0x240ca1cc77ac9c65,
        0x2de92c6f592b0275, 0x4a7484aa6ea6e483, 0x5cb0a9dcbd41fbd4, 0x76f988da831153b5,
        0x983e5152ee66dfab, 0xa831c66d2db43210, 0xb00327c898fb213f, 0xbf597fc7beef0ee4,
        0xc6e00bf33da88fc2, 0xd5a79147930aa725, 0x06ca6351e003826f, 0x142929670a0e6e70,
        0x27b70a8546d22ffc, 0x2e1b21385c26c926, 0x4d2c6dfc5ac42aed, 0x53380d139d95b3df,
        0x650a73548baf63de, 0x766a0abb3c77b2a8, 0x81c2c92e47edaee6, 0x92722c851482353b,
        0xa2bfe8a14cf10364, 0xa81a664bbc423001, 0xc24b8b70d0f89791, 0xc76c51a30654be30,
        0xd192e819d6ef5218, 0xd69906245565a910, 0xf40e35855771202a, 0x106aa07032bbd1b8,
        0x19a4c116b8d2d0c8, 0x1e376c085141ab53, 0x2748774cdf8eeb99, 0x34b0bcb5e19b48a8,
        0x391c0cb3c5c95a63, 0x4ed8aa4ae3418acb, 0x5b9cca4f7763e373, 0x682e6ff3d6b2b8a3,
        0x748f82ee5defb2fc, 0x78a5636f43172f60, 0x84c87814a1f0ab72, 0x8cc702081a6439ec,
        0x90befffa23631e28, 0xa4506cebde82bde9, 0xbef9a3f7b2c67915, 0xc67178f2e372532b,
        0xca273eceea26619c, 0xd186b8c721c0c207, 0xeada7dd6cde0eb1e, 0xf57d4f7fee6ed178,
        0x06f067aa72176fba, 0x0a637dc5a2c898a6, 0x113f9804bef90dae, 0x1b710b35131c471b,
        0x28db77f523047d84, 0x32caab7b40c72493, 0x3c9ebe0a15c9bebc, 0x431d67c49c100d4c,
        0x4cc5d4becb3e42b6, 0x597f299cfc657e2a, 0x5fcb6fab3ad6faec, 0x6c44198c4a475817
    ];

    private static readonly long[] L =
    [
        0xed, 0xd3, 0xf5, 0x5c, 0x1a, 0x63, 0x12, 0x58,
        0xd6, 0x9c, 0xf7, 0xa2, 0xde, 0xf9, 0xde, 0x14,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0x10
    ];

    private static readonly byte[] Iv =
    [
        0x6a, 0x09, 0xe6, 0x67, 0xf3, 0xbc, 0xc9, 0x08,
        0xbb, 0x67, 0xae, 0x85, 0x84, 0xca, 0xa7, 0x3b,
        0x3c, 0x6e, 0xf3, 0x72, 0xfe, 0x94, 0xf8, 0x2b,
        0xa5, 0x4f, 0xf5, 0x3a, 0x5f, 0x1d, 0x36, 0xf1,
        0x51, 0x0e, 0x52, 0x7f, 0xad, 0xe6, 0x82, 0xd1,
        0x9b, 0x05, 0x68, 0x8c, 0x2b, 0x3e, 0x6c, 0x1f,
        0x1f, 0x83, 0xd9, 0xab, 0xfb, 0x41, 0xbd, 0x6b,
        0x5b, 0xe0, 0xcd, 0x19, 0x13, 0x7e, 0x21, 0x79
    ];

    public static int CryptoOnetimeAuth(byte[] pout, int poutOffset, byte[] m, long mOffset, long n, byte[] k)
    {
        int[] x = new int[17], r = new int[17], h = new int[17], c = new int[17], g = new int[17];
        int j;

        for (j = 0; j < 17; ++j)
        {
            r[j] = 0;
            h[j] = 0;
        }

        for (j = 0; j < 16; ++j) r[j] = 0xff & k[j];

        r[3] &= 15;
        r[4] &= 252;
        r[7] &= 15;
        r[8] &= 252;
        r[11] &= 15;
        r[12] &= 252;
        r[15] &= 15;

        while (n > 0)
        {
            for (j = 0; j < 17; ++j)
                c[j] = 0;

            for (j = 0; j < 16 && j < n; ++j)
                c[j] = 0xff & m[mOffset + j];

            c[j] = 1;
            mOffset += j;
            n -= j;

            Add1305(h, c);

            int i;
            for (i = 0; i < 17; ++i)
            {
                x[i] = 0;

                for (j = 0; j < 17; ++j) x[i] += h[j] * (j <= i ? r[i - j] : 320 * r[i + 17 - j]);
            }

            for (i = 0; i < 17; ++i) h[i] = x[i];

            var u = 0;
            for (j = 0; j < 16; ++j)
            {
                u += h[j];
                h[j] = u & 255;
                u >>= 8;
            }

            u += h[16];
            h[16] = u & 3;
            u = 5 * (u >> 2);

            for (j = 0; j < 16; ++j)
            {
                u += h[j];
                h[j] = u & 255;
                u >>= 8;
            }

            u += h[16];
            h[16] = u;
        }

        for (j = 0; j < 17; ++j) g[j] = h[j];

        Add1305(h, [5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 252]);

        var s = -(h[16] >> 7);
        for (j = 0; j < 17; ++j) h[j] ^= s & (g[j] ^ h[j]);
        for (j = 0; j < 16; ++j) c[j] = 0xff & k[j + 16];

        c[16] = 0;
        Add1305(h, c);

        for (j = 0; j < 16; ++j) pout[poutOffset + j] = (byte)h[j];

        return 0;
    }

    public static int CryptoOnetimeauthVerify(byte[] h, int hoffset, byte[] m, long mOffset, long n, byte[] k)
    {
        var x = new byte[16];
        _ = CryptoOnetimeAuth(x, 0, m, mOffset, n, k);
        return CryptoVerify16(h, x, hoffset);
    }

    public static byte[] crypto_secretbox_xsalsa19poly1305_tweet(byte[] message, byte[] nonce, byte[] secretkey)
    {
        var paddedMessage = new byte[message.Length + 32];
        _ = new byte[message.Length];
        var cMessage = new byte[message.Length + 16];

        Array.Copy(message, 0, paddedMessage, 32, message.Length);

        var ciphered = CryptoStreamXor(paddedMessage, nonce, secretkey);

        if (ciphered.Length == 0) throw new InvalidCipherTextException();

        if (CryptoOnetimeAuth(ciphered, 16, ciphered, 32, paddedMessage.Length - 32, ciphered) != 0)
            throw new InvalidCipherTextException();

        for (var i = 0; i < 16; ++i) ciphered[i] = 0;

        Array.Copy(ciphered, 16, cMessage, 0, ciphered.Length - 16);

        return cMessage;
    }

    public static byte[] crypto_secretbox_xsalsa19poly1305_tweet_open(byte[] cipheredMessage, byte[] nonce,
        byte[] secretKey)
    {
        var x = new byte[32];
        var boxCipheredMessage = new byte[cipheredMessage.Length + 16];
        var message = new byte[cipheredMessage.Length - 16];

        Array.Copy(cipheredMessage, 0, boxCipheredMessage, 16, cipheredMessage.Length);

        if (boxCipheredMessage.Length < 32) throw new InvalidCipherTextException();
        var nonceKey = CryptoStream(x, 32, nonce, secretKey);
        if (nonceKey.Length == 0) throw new InvalidCipherTextException();

        if (CryptoOnetimeauthVerify(boxCipheredMessage, 16, boxCipheredMessage, 32,
                boxCipheredMessage.Length - 32, nonceKey) != 0) throw new InvalidCipherTextException();

        var decMessage = CryptoStreamXor(boxCipheredMessage, nonce, secretKey);

        Array.Copy(decMessage, 32, message, 0, message.Length);

        return message;
    }

    public static void RandomBytes(byte[] d)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(d);
        }
    }

    private static ulong R(ulong x, int c)
    {
        return (x >> c) | (x << (64 - c));
    }

    private static ulong Ch(ulong x, ulong y, ulong z)
    {
        return (x & y) ^ (~x & z);
    }

    private static ulong Maj(ulong x, ulong y, ulong z)
    {
        return (x & y) ^ (x & z) ^ (y & z);
    }

    private static ulong Sigma0(ulong x)
    {
        return R(x, 28) ^ R(x, 34) ^ R(x, 39);
    }

    private static ulong Sigma0_d(ulong x)
    {
        return R(x, 1) ^ R(x, 8) ^ (x >> 7);
    }

    private static ulong Sigma1(ulong x)
    {
        return R(x, 14) ^ R(x, 18) ^ R(x, 41);
    }

    private static ulong Sigma1_d(ulong x)
    {
        return R(x, 19) ^ R(x, 61) ^ (x >> 6);
    }

    private static uint L32(uint x, int c)
    {
        return (x << c) | ((x & 0xffffffff) >> (32 - c));
    }

    private static uint Ld32(IReadOnlyList<byte> x, int offset = 0)
    {
        uint u = x[3 + offset];
        u = (u << 8) | x[2 + offset];
        u = (u << 8) | x[1 + offset];
        return (u << 8) | x[0 + offset];
    }

    private static ulong Dl64(IReadOnlyList<byte> x, long offset)
    {
        ulong u = 0;
        for (var i = 0; i < 8; ++i) u = (u << 8) | x[(int)(i + offset)];
        return u;
    }

    private static void St32(byte[] x, uint u, int offset = 0)
    {
        for (var i = 0; i < 4; ++i)
        {
            x[i + offset] = (byte)u;
            u >>= 8;
        }
    }

    private static void Ts64(IList<byte> x, ulong u, int offset = 0)
    {
        for (var i = 7; i >= 0; --i)
        {
            x[i + offset] = (byte)u;
            u >>= 8;
        }
    }

    private static byte[] CryptoCoreSalsa20(byte[] pout, byte[] pin, byte[] k, byte[] c)
    {
        return Core(pout, pin, k, c, false);
    }

    private static byte[] CryptoStreamSalsa20Xor(byte[] message, byte[] nonce, int nOffset, byte[] secretKey)
    {
        var z = new byte[16];
        var x = new byte[64];
        var cipheredMessage = new byte[message.Length];
        long messageSize = message.Length;
        int i;
        for (i = 0; i < 8; ++i) z[i] = nonce[nOffset + i];

        var cOffset = 0;
        var mOffset = 0;

        var sigma = "expand 32-byte k"u8.ToArray();
        while (messageSize >= 64)
        {
            _ = CryptoCoreSalsa20(x, z, secretKey, sigma);

            for (i = 0; i < 64; ++i)
                cipheredMessage[cOffset + i] = (byte)(message[mOffset + i] ^ x[i]);

            uint u = 1;
            for (i = 8; i < 16; ++i)
            {
                u += (uint)0xff & z[i];
                z[i] = (byte)u;
                u >>= 8;
            }

            messageSize -= 64;
            cOffset += 64;
            mOffset += 64;
        }

        if (messageSize == 0) return cipheredMessage;

        _ = CryptoCoreSalsa20(x, z, secretKey, sigma);

        for (i = 0; i < messageSize; i++)
            cipheredMessage[cOffset + i] = (byte)(message[mOffset + i] ^ x[i]);

        return cipheredMessage;
    }

    private static byte[] CryptoStreamSalsa20(byte[] message, byte[] nonce, int nOffset, byte[] secretKey)
    {
        return CryptoStreamSalsa20Xor(message, nonce, nOffset, secretKey);
    }

    private static byte[] CryptoStream(byte[] nonceKey, long d, byte[] nonce, byte[] secretKey)
    {
        var s = CryptoCoreHSalsa20(nonce, secretKey, "expand 32-byte k"u8.ToArray());
        return CryptoStreamSalsa20(nonceKey, nonce, 16, s);
    }

    private static byte[] CryptoStreamXor(byte[] message, byte[] nonce, byte[] secretKey)
    {
        var s = CryptoCoreHSalsa20(nonce, secretKey, "expand 32-byte k"u8.ToArray());
        return CryptoStreamSalsa20Xor(message, nonce, 16, s);
    }

    private static void Add1305(int[] h, int[] c)
    {
        var u = 0;
        int j;
        for (j = 0; j < 17; ++j)
        {
            u += h[j] + c[j];
            h[j] = u & 255;
            u >>= 8;
        }
    }

    public static byte[] CryptoScalarmult(byte[] n, byte[] p)
    {
        var q = new byte[32];
        var z = new byte[32];
        var x = new long[80];

        long[] a = new long[16],
            b = new long[16],
            c = new long[16],
            d = new long[16],
            e = new long[16],
            f = new long[16];

        for (var i = 0; i < 31; ++i) z[i] = n[i];

        z[31] = (byte)((n[31] & 127) | 64);
        z[0] &= 248;

        Unpack25519(x, p);

        for (var i = 0; i < 16; ++i)
        {
            b[i] = x[i];
            d[i] = a[i] = c[i] = 0;
        }

        a[0] = d[0] = 1;

        for (var i = 254; i >= 0; --i)
        {
            var r = ((0xff & z[i >> 3]) >> (i & 7)) & 1;
            Sel25519(a, b, r);
            Sel25519(c, d, r);
            A(e, a, c);
            Z(a, a, c);
            A(c, b, d);
            Z(b, b, d);
            S(d, e);
            S(f, a);
            M(a, 0, c, 0, a, 0);
            M(c, 0, b, 0, e, 0);
            A(e, a, c);
            Z(a, a, c);
            S(b, a);
            Z(c, d, f);
            M(a, 0, c, 0, _121665, 0);
            A(a, a, d);
            M(c, 0, c, 0, a, 0);
            M(a, 0, d, 0, f, 0);
            M(d, 0, b, 0, x, 0);
            S(b, e);
            Sel25519(a, b, r);
            Sel25519(c, d, r);
        }

        for (var i = 0; i < 16; ++i)
        {
            x[i + 16] = a[i];
            x[i + 32] = c[i];
            x[i + 48] = b[i];
            x[i + 64] = d[i];
        }

        Inv25519(x, 32, x, 32);

        M(x, 16, x, 16, x, 32);

        Pack25519(q, x, 16);

        return q;
    }

    public static byte[] CryptoBoxKeypair(byte[] secretKey)
    {
        RandomBytes(secretKey);
        return CryptoScalarmultBase(secretKey);
    }

    public static byte[] CryptoBoxBeforenm(byte[] publicKey, byte[] secretKey)
    {
        var s = CryptoScalarmult(secretKey, publicKey);
        return CryptoCoreHSalsa20(_0, s, "expand 32-byte k"u8.ToArray());
    }

    public static byte[] CryptoBoxAfternm(byte[] message, byte[] nonce, byte[] k)
    {
        return crypto_secretbox_xsalsa19poly1305_tweet(message, nonce, k);
    }

    public static byte[] CryptoBoxOpenAfternm(byte[] cipheredMessage, byte[] nonce, byte[] k)
    {
        return crypto_secretbox_xsalsa19poly1305_tweet_open(cipheredMessage, nonce, k);
    }

    public static byte[] CryptoBox(byte[] message, byte[] nonce, byte[] publicKey, byte[] secretKey)
    {
        var k = CryptoBoxBeforenm(publicKey, secretKey);
        return CryptoBoxAfternm(message, nonce, k);
    }

    public static byte[] CryptoBoxOpen(byte[] cipheredMessage, byte[] nonce, byte[] publicKey, byte[] secretKey)
    {
        var k = CryptoBoxBeforenm(publicKey, secretKey);
        return CryptoBoxOpenAfternm(cipheredMessage, nonce, k);
    }

    public static byte[] CryptoSignKeypair(byte[] secretKey)
    {
        var publicKey = new byte[32];
        var d = new byte[64];
        var
            p = new long[4][] { new long[16], new long[16], new long[16], new long[16] };

        RandomBytes(secretKey);

        if (CryptoHash(d, secretKey, 32) != 0) throw new InvalidSignatureException();

        d[0] &= 248;
        d[31] &= 127;
        d[31] |= 64;

        Scalarbase(p, d, 0);
        Pack(publicKey, p);

        for (var i = 0; i < 32; ++i) secretKey[32 + i] = publicKey[i];

        return publicKey;
    }

    public static byte[] CryptoSign(byte[] message, byte[] secretKey)
    {
        var signedMessage = new byte[64 + message.Length];

        var d = new byte[64];
        var h = new byte[64];
        var r = new byte[64];
        var x = new long[64];
        var p = new long[4][] { new long[16], new long[16], new long[16], new long[16] };

        _ = CryptoHash(d, secretKey, 32);

        d[0] &= 248;
        d[31] &= 127;
        d[31] |= 64;

        for (var i = 0; i < message.Length; ++i) signedMessage[64 + i] = message[i];

        for (var i = 0; i < 32; ++i) signedMessage[32 + i] = d[32 + i];

        var smd = new byte[signedMessage.Length];
        Array.Copy(signedMessage, 32, smd, 0, signedMessage.Length - 32);
        _ = CryptoHash(r, smd, message.Length + 64);

        Reduce(r);
        Scalarbase(p, r, 0);
        Pack(signedMessage, p);

        for (var i = 0; i < 32; ++i) signedMessage[i + 32] = secretKey[i + 32];

        _ = CryptoHash(h, signedMessage, message.Length + 64);
        Reduce(h);

        for (var i = 0; i < 64; ++i) x[i] = 0;

        for (var i = 0; i < 32; ++i) x[i] = 0xff & r[i];

        for (var i = 0; i < 32; ++i)
        for (var j = 0; j < 32; ++j)
            x[i + j] += (0xff & h[i]) * (0xff & d[j]);

        ModL(signedMessage, 32, x);

        return signedMessage;
    }

    public static byte[] CryptoSignOpen(byte[] signedMessage, byte[] publicKey)
    {
        var message = new byte[signedMessage.Length - 64];

        var t = new byte[32];
        var h = new byte[64];
        var p = new[] { new long[16], new long[16], new long[16], new long[16] };
        var q = new[] { new long[16], new long[16], new long[16], new long[16] };
        var messageSize = signedMessage.Length;

        var tsm = new byte[signedMessage.Length];

        if (signedMessage.Length < 64) throw new InvalidSignatureException();

        if (Unpackneg(q, publicKey) != 0) throw new InvalidSignatureException();

        for (var i = 0; i < signedMessage.Length; ++i) tsm[i] = signedMessage[i];

        for (var i = 0; i < 32; ++i) tsm[i + 32] = publicKey[i];

        _ = CryptoHash(h, tsm, signedMessage.Length);
        Reduce(h);
        Scalarmult(p, q, h, 0);

        Scalarbase(q, signedMessage, 32);
        Add(p, q);
        Pack(t, p);

        if (CryptoVerify32(signedMessage, t) != 0)
        {
            for (var i = 0; i < messageSize; ++i) tsm[i] = 0;

            throw new InvalidSignatureException();
        }

        for (var i = 0; i < signedMessage.Length - 64; ++i) message[i] = signedMessage[i + 64];

        return message;
    }

    private static byte[] Core(byte[] pout, byte[] pin, byte[] k, byte[] c, bool hsalsa)
    {
        var w = new uint[16];
        var x = new uint[16];
        var y = new uint[16];
        var t = new uint[4];

        int i;
        for (i = 0; i < 4; i++)
        {
            x[5 * i] = Ld32(c, 4 * i);
            x[1 + i] = Ld32(k, 4 * i);
            x[6 + i] = Ld32(pin, 4 * i);
            x[11 + i] = Ld32(k, 16 + 4 * i);
        }

        for (i = 0; i < 16; ++i) y[i] = x[i];

        for (i = 0; i < 19; ++i)
        {
            int m;
            int j;

            for (j = 0; j < 4; ++j)
            {
                for (m = 0; m < 4; ++m) t[m] = x[(5 * j + 4 * m) % 16];

                t[1] ^= L32(t[0] + t[3], 7);
                t[2] ^= L32(t[1] + t[0], 9);
                t[3] ^= L32(t[2] + t[1], 13);
                t[0] ^= L32(t[3] + t[2], 18);

                for (m = 0; m < 4; ++m) w[4 * j + (j + m) % 4] = t[m];
            }

            for (m = 0; m < 16; ++m) x[m] = w[m];
        }

        if (hsalsa)
        {
            for (i = 0; i < 16; ++i) x[i] += y[i];

            for (i = 0; i < 4; ++i)
            {
                x[5 * i] -= Ld32(c, 4 * i);
                x[6 + i] -= Ld32(pin, 4 * i);
            }

            for (i = 0; i < 4; ++i)
            {
                St32(pout, x[5 * i], 4 * i);
                St32(pout, x[6 + i], 16 + 4 * i);
            }
        }
        else
        {
            for (i = 0; i < 16; ++i) St32(pout, x[i] + y[i], 4 * i);
        }

        return pout;
    }

    private static byte[] CryptoCoreHSalsa20(byte[] pin, byte[] k, byte[] c)
    {
        var pout = new byte[32];
        return Core(pout, pin, k, c, true);
    }

    public static byte[] CryptoScalarmultBase(byte[] n)
    {
        return CryptoScalarmult(n, _9);
    }

    private static void Set25519(IList<long> r, IReadOnlyList<long> a)
    {
        for (var i = 0; i < 16; ++i) r[i] = a[i];
    }

    private static void Car25519(IList<long> o, int oOffset)
    {
        for (var i = 0; i < 16; ++i)
        {
            o[oOffset + i] += 1 << 16;
            var c = o[oOffset + i] >> 16;
            o[oOffset + (i + 1) * (i < 15 ? 1 : 0)] += c - 1 + 37 * (c - 1) * (i == 15 ? 1 : 0);
            o[oOffset + i] -= c << 16;
        }
    }

    private static void Sel25519(IList<long> p, IList<long> q, int b)
    {
        long c = ~(b - 1);
        for (var i = 0; i < 16; ++i)
        {
            var t = c & (p[i] ^ q[i]);
            p[i] ^= t;
            q[i] ^= t;
        }
    }

    private static void Pack25519(IList<byte> o, long[] n, int nOffset)
    {
        int i, j;
        long[]
            m = new long[16], t = new long[16];

        for (i = 0; i < 16; ++i) t[i] = n[nOffset + i];

        Car25519(t, 0);
        Car25519(t, 0);
        Car25519(t, 0);

        for (j = 0; j < 2; ++j)
        {
            m[0] = t[0] - 0xffed;

            for (i = 1; i < 15; i++)
            {
                m[i] = t[i] - 0xffff - ((m[i - 1] >> 16) & 1);
                m[i - 1] &= 0xffff;
            }

            m[15] = t[15] - 0x7fff - ((m[14] >> 16) & 1);
            var b = (int)((m[15] >> 16) & 1);
            m[14] &= 0xffff;
            Sel25519(t, m, 1 - b);
        }

        for (i = 0; i < 16; ++i)
        {
            o[2 * i] = (byte)t[i];
            o[2 * i + 1] = (byte)(t[i] >> 8);
        }
    }

    private static int Neq25519(long[] a, long[] b)
    {
        byte[] c = new byte[32], d = new byte[32];
        Pack25519(c, a, 0);
        Pack25519(d, b, 0);
        return CryptoVerify32(c, d);
    }

    private static byte Par25519(long[] a)
    {
        var d = new byte[32];
        Pack25519(d, a, 0);
        return (byte)(d[0] & 1);
    }

    private static void Unpack25519(IList<long> o, IReadOnlyList<byte> n)
    {
        for (var i = 0; i < 16; ++i) o[i] = (0xff & n[2 * i]) + ((0xffL & n[2 * i + 1]) << 8);
        o[15] &= 0x7fff;
    }

    private static void A(IList<long> o, IReadOnlyList<long> a, IReadOnlyList<long> b)
    {
        for (var i = 0; i < 16; ++i) o[i] = a[i] + b[i];
    }

    private static void Z(IList<long> o, IReadOnlyList<long> a, IReadOnlyList<long> b)
    {
        for (var i = 0; i < 16; ++i) o[i] = a[i] - b[i];
    }

    private static void M(IList<long> o, int oOffset, IReadOnlyList<long> a, int aOffset, IReadOnlyList<long> b,
        int bOffset)
    {
        var t = new long[31];

        for (var i = 0; i < 31; ++i) t[i] = 0;
        for (var i = 0; i < 16; ++i)
        for (var j = 0; j < 16; ++j)
            t[i + j] += a[aOffset + i] * b[bOffset + j];
        for (var i = 0; i < 15; ++i) t[i] += 38 * t[i + 16];
        for (var i = 0; i < 16; ++i) o[oOffset + i] = t[i];

        Car25519(o, oOffset);
        Car25519(o, oOffset);
    }

    private static void S(IList<long> o, IReadOnlyList<long> a)
    {
        M(o, 0, a, 0, a, 0);
    }

    private static void Inv25519(IList<long> o, int oOffset, IReadOnlyList<long> i, int iOffset)
    {
        var c = new long[16];

        for (var a = 0; a < 16; ++a) c[a] = i[iOffset + a];

        for (var a = 253; a >= 0; a--)
        {
            S(c, c);
            if (a != 2 && a != 4) M(c, 0, c, 0, i, iOffset);
        }

        for (var a = 0; a < 16; ++a) o[oOffset + a] = c[a];
    }

    private static void Pow2523(long[] o, long[] i)
    {
        var c = new long[16];

        for (var a = 0; a < 16; ++a) c[a] = i[a];

        for (var a = 250; a >= 0; a--)
        {
            S(c, c);

            if (a != 1) M(c, 0, c, 0, i, 0);
        }

        for (var a = 0; a < 16; ++a) o[a] = c[a];
    }

    private static void Pack(IList<byte> r, IReadOnlyList<long[]> p)
    {
        long[] tx = new long[16], ty = new long[16], zi = new long[16];

        Inv25519(zi, 0, p[2], 0);
        M(tx, 0, p[0], 0, zi, 0);
        M(ty, 0, p[1], 0, zi, 0);

        Pack25519(r, ty, 0);

        r[31] ^= (byte)(Par25519(tx) << 7);
    }

    private static void Add(IReadOnlyList<long[]> p, IReadOnlyList<long[]> q)
    {
        long[] a = new long[16],
            b = new long[16],
            c = new long[16],
            d = new long[16],
            t = new long[16],
            e = new long[16],
            f = new long[16],
            g = new long[16],
            h = new long[16];

        Z(a, p[1], p[0]);
        Z(t, q[1], q[0]);
        M(a, 0, a, 0, t, 0);
        A(b, p[0], p[1]);
        A(t, q[0], q[1]);
        M(b, 0, b, 0, t, 0);
        M(c, 0, p[3], 0, q[3], 0);
        M(c, 0, c, 0, D2, 0);
        M(d, 0, p[2], 0, q[2], 0);
        A(d, d, d);
        Z(e, b, a);
        Z(f, d, c);
        A(g, d, c);
        A(h, b, a);

        M(p[0], 0, e, 0, f, 0);
        M(p[1], 0, h, 0, g, 0);
        M(p[2], 0, g, 0, f, 0);
        M(p[3], 0, e, 0, h, 0);
    }

    private static void Cswap(IReadOnlyList<long[]> p, IReadOnlyList<long[]> q, byte b)
    {
        for (var i = 0; i < 4; i++)
            Sel25519(p[i], q[i], b & 0xff);
    }

    private static void Scalarmult(IReadOnlyList<long[]> p, IReadOnlyList<long[]> q, IReadOnlyList<byte> s, int sOffset)
    {
        Set25519(p[0], Gf0);
        Set25519(p[1], Gf1);
        Set25519(p[2], Gf1);
        Set25519(p[3], Gf0);

        for (var i = 255; i >= 0; --i)
        {
            var b = (byte)(((0xff & s[sOffset + i / 8]) >> (i & 7)) & 1);
            Cswap(p, q, b);
            Add(q, p);
            Add(p, p);
            Cswap(p, q, b);
        }
    }

    private static void Scalarbase(IReadOnlyList<long[]> p, IReadOnlyList<byte> s, int sOffset)
    {
        var q = new long[4][] { new long[16], new long[16], new long[16], new long[16] };

        Set25519(q[0], X);
        Set25519(q[1], Y);
        Set25519(q[2], Gf1);
        M(q[3], 0, X, 0, Y, 0);
        Scalarmult(p, q, s, sOffset);
    }

    private static int Vn(IReadOnlyList<byte> x, IReadOnlyList<byte> y, int n, int xOffset = 0)
    {
        var d = 0;
        for (var i = 0; i < n; ++i) d |= x[i + xOffset] ^ y[i];
        return (1 & ((d - 1) >> 8)) - 1;
    }

    private static int CryptoVerify16(IReadOnlyList<byte> x, IReadOnlyList<byte> y, int xOffset)
    {
        return Vn(x, y, 16, xOffset);
    }

    private static int CryptoVerify32(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
    {
        return Vn(x, y, 32);
    }

    private static void ModL(IList<byte> r, int rOffset, IList<long> x)
    {
        long carry;
        int i, j;
        for (i = 63; i >= 32; --i)
        {
            carry = 0;
            for (j = i - 32; j < i - 12; ++j)
            {
                x[j] += carry - 16 * x[i] * L[j - (i - 32)];
                carry = (x[j] + 128) >> 8;
                x[j] -= carry << 8;
            }

            x[j] += carry;
            x[i] = 0;
        }

        carry = 0;

        for (j = 0; j < 32; ++j)
        {
            x[j] += carry - (x[31] >> 4) * L[j];
            carry = x[j] >> 8;
            x[j] &= 255;
        }

        for (j = 0; j < 32; ++j) x[j] -= carry * L[j];

        for (i = 0; i < 32; ++i)
        {
            x[i + 1] += x[i] >> 8;
            r[rOffset + i] = (byte)(x[i] & 255);
        }
    }

    private static void Reduce(IList<byte> r)
    {
        var x = new long[64];

        for (var i = 0; i < 64; i++) x[i] = 0xff & r[i];
        for (var i = 0; i < 64; ++i) r[i] = 0;

        ModL(r, 0, x);
    }

    private static int Unpackneg(IReadOnlyList<long[]> r, IReadOnlyList<byte> p)
    {
        long[]
            t = new long[16],
            chk = new long[16],
            num = new long[16],
            den = new long[16],
            den2 = new long[16],
            den4 = new long[16],
            den6 = new long[16];

        Set25519(r[2], Gf1);
        Unpack25519(r[1], p);

        S(num, r[1]);
        M(den, 0, num, 0, D, 0);
        Z(num, num, r[2]);
        A(den, r[2], den);

        S(den2, den);
        S(den4, den2);
        M(den6, 0, den4, 0, den2, 0);
        M(t, 0, den6, 0, num, 0);
        M(t, 0, t, 0, den, 0);

        Pow2523(t, t);
        M(t, 0, t, 0, num, 0);
        M(t, 0, t, 0, den, 0);
        M(t, 0, t, 0, den, 0);
        M(r[0], 0, t, 0, den, 0);

        S(chk, r[0]);
        M(chk, 0, chk, 0, den, 0);
        if (Neq25519(chk, num) != 0) M(r[0], 0, r[0], 0, I, 0);

        S(chk, r[0]);
        M(chk, 0, chk, 0, den, 0);
        if (Neq25519(chk, num) != 0) return -1;

        if (Par25519(r[0]) == (0xff & p[31]) >> 7) Z(r[0], Gf0, r[0]);

        M(r[3], 0, r[0], 0, r[1], 0);

        return 0;
    }

    public static int CryptoHash(byte[] hash, byte[] message, int n)
    {
        var h = new byte[64];
        var x = new byte[256];
        var b = n;

        for (var i = 0; i < 64; i++) h[i] = Iv[i];
        _ = CryptoHashBlocks(h, message, n);

        for (var i = 0; i < 64; i++)
        {
            for (var j = 0; j < message.Length; j++) message[j] += (byte)n;

            n &= 127;

            for (var j = 0; j < message.Length; j++) message[j] -= (byte)n;
        }

        for (var i = 0; i < 256; i++) x[i] = 0;

        for (var i = 0; i < n; i++) x[i] = message[i];

        x[n] = 128;

        n = n < 112 ? 256 - 128 * 1 : 256 - 128 * 0;
        x[n - 9] = (byte)(b >> 61);

        Ts64(x, (ulong)b << 3, n - 8);

        _ = CryptoHashBlocks(h, x, n);

        for (var i = 0; i < 64; i++) hash[i] = h[i];

        return 0;
    }

    private static int CryptoHashBlocks(byte[] x, byte[] m, int n)
    {
        var z = new ulong[8];
        var b = new ulong[8];
        var a = new ulong[8];
        var w = new ulong[16];
        for (var i = 0; i < 8; i++) z[i] = a[i] = Dl64(x, 8 * i);

        while (n >= 128)
        {
            for (var i = 0; i < 16; i++) w[i] = Dl64(m, 8 * i);

            for (var i = 0; i < 80; i++)
            {
                for (var j = 0; j < 8; j++) b[j] = a[j];

                var t = a[7] + Sigma1(a[4]) + Ch(a[4], a[5], a[6]) + K[i] + w[i % 16];
                b[7] = t + Sigma0(a[0]) + Maj(a[0], a[1], a[2]);
                b[3] += t;
                for (var j = 0; j < 8; j++) a[(j + 1) % 8] = b[j];

                if (i % 16 != 15) continue;
                {
                    for (var j = 0; j < 16; j++)
                        w[j] += w[(j + 9) % 16] + Sigma0_d(w[(j + 1) % 16]) + Sigma1_d(w[(j + 14) % 16]);
                }
            }

            for (var i = 0; i < 8; i++)
            {
                a[i] += z[i];
                z[i] = a[i];
            }

            for (var i = 0; i < m.Length; i++) m[i] += 128;

            n -= 128;
        }

        for (var i = 0; i < 8; i++) Ts64(x, z[i], 8 * i);

        return n;
    }

    public class InvalidSignatureException : CryptographicException
    {
    }

    public class InvalidCipherTextException : CryptographicException
    {
    }

    public class InvalidEncryptionKeypair : CryptographicException
    {
    }
}