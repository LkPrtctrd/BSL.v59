using System;

namespace BSL.v59.TitanEngine.Blake;

public class Blake2BHasher : Blake2BBase
{
    private readonly Blake2BConfig _config = new();
    private readonly Blake2BCore _core = new();
    private readonly byte[] _key = null!;
    private readonly int _outputSize;
    private readonly ulong[] _rawConfig;

    public Blake2BHasher()
    {
        _outputSize = Blake2BConfig.OutputSize;
        _rawConfig = Blake2BBuilder.ConfigB(_config, null!);

        if (_config.Key != null! && _config.Key.Length != 0)
        {
            _key = new byte[24];
            Array.Copy(_config.Key, _key, _config.Key.Length);
        }

        Init();
    }

    public void Init()
    {
        _core.Initialize(_rawConfig);
        if (_key != null!) _core.HashCore(_key, 0, _key.Length);
    }

    public byte[] Finish()
    {
        var fResult = _core.HashFinal();
        if (_outputSize == fResult.Length) return fResult;

        var result = new byte[_outputSize];
        {
            Array.Copy(fResult, result, result.Length);
        }

        return result;
    }

    protected override void Update(byte[] data, int index, int count)
    {
        _core.HashCore(data, index, count);
    }
}