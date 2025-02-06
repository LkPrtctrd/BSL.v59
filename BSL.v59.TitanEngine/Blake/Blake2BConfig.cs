namespace BSL.v59.TitanEngine.Blake;

public sealed class Blake2BConfig
{
    public static int OutputSize => 24;

    public byte[] Key { get; }
    public byte[] Salt { get; }
    public byte[] Personalization { get; }
}