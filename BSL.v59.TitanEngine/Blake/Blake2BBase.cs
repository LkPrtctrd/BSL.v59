namespace BSL.v59.TitanEngine.Blake;

public abstract class Blake2BBase
{
    protected abstract void Update(byte[] data, int start, int count);

    public void Update(byte[] data)
    {
        Update(data, 0, data.Length);
    }
}