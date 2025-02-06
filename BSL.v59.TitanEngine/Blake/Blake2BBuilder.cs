namespace BSL.v59.TitanEngine.Blake;

public static class Blake2BBuilder
{
    private static readonly Blake2BTreeConfig SequentialTreeConfig = new()
    {
        IntermediateHashSize = 0,
        LeafSize = 0,
        FanOut = 1,
        MaxHeight = 1
    };

    public static ulong[] ConfigB(Blake2BConfig config, Blake2BTreeConfig treeConfig)
    {
        var isSequential = treeConfig == null!;
        if (isSequential) treeConfig = SequentialTreeConfig;

        var rawConfig = new ulong[8];
        rawConfig[0] |= (uint)Blake2BConfig.OutputSize;

        if (config.Key != null!) rawConfig[0] |= (uint)config.Key.Length << 8;

        rawConfig[0] |= (uint)treeConfig!.FanOut << 16;
        rawConfig[0] |= (uint)treeConfig.MaxHeight << 24;
        rawConfig[0] |= (ulong)(uint)treeConfig.LeafSize << 32;
        rawConfig[2] |= (uint)treeConfig.IntermediateHashSize << 8;

        if (config.Salt != null!)
        {
            rawConfig[4] = Blake2BCore.BytesToUInt64(config.Salt, 0);
            rawConfig[5] = Blake2BCore.BytesToUInt64(config.Salt, 8);
        }

        if (config.Personalization == null!) return rawConfig;
        rawConfig[6] = Blake2BCore.BytesToUInt64(config.Personalization, 0);
        rawConfig[7] = Blake2BCore.BytesToUInt64(config.Personalization, 8);

        return rawConfig;
    }
}