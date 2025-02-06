namespace BSL.v59.TitanEngine.Blake;

public sealed class Blake2BTreeConfig
{
    public int IntermediateHashSize { get; init; } = 64;

    public int FanOut { get; init; }
    public long LeafSize { get; init; }
    public int MaxHeight { get; init; }
}