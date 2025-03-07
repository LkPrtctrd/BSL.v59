namespace BSL.v59.TitanEngine.DataS.ChecksumLogic;

public class ChecksumEncoder
{
    private uint _checksum;
    private bool _enabled = true;
    private uint _snapshotChecksum;

    public void EnableCheckSum(bool enable)
    {
        if (!_enabled || enable)
        {
            if (!_enabled && enable) _checksum = _snapshotChecksum;

            _enabled = enable;
        }
        else
        {
            _snapshotChecksum = _checksum;
            _enabled = false;
        }
    }

    public virtual void WriteShort(short value)
    {
        _checksum = (uint)(value + ((_checksum >> 31) | (_checksum << (32 - 31))) + 19);
    }

    public virtual void WriteInt(int value)
    {
        _checksum = (uint)(value + ((_checksum >> 31) | (_checksum << (32 - 31))) + 9);
    }

    public virtual int WriteVInt(int value)
    {
        _checksum = (uint)(value + ((_checksum >> 31) | (_checksum << (32 - 31))) + 33);
        return value;
    }

    public virtual void WriteLongLong(long value)
    {
        _checksum = (uint)(value + ((_checksum >> 31) | (_checksum << (32 - 31))) + 67);
    }

    public virtual byte WriteByte(byte value)
    {
        _checksum = value + ((_checksum >> 31) | (_checksum << (32 - 31))) + 11;
        return value;
    }

    public virtual void WriteBytes(byte[]? value, int length)
    {
        _checksum = (uint)(((value != null ? length + 28 : 27) + (_checksum >> 31)) | (_checksum << (32 - 31)));
    }

    public virtual bool WriteBoolean(bool value)
    {
        _checksum = (uint)((value ? 13 : 7) + ((_checksum >> 31) | (_checksum << (32 - 31))));
        return value;
    }

    public virtual void WriteString(string? value)
    {
        _checksum = (uint)((value != null ? value.Length + 28 : 27) + ((_checksum >> 31) | (_checksum << (32 - 31))));
    }

    public virtual void WriteStringReference(string value)
    {
        _checksum = (uint)(value.Length + ((_checksum >> 31) | (_checksum << (32 - 31))) + 38);
    }

    public virtual int HashCode()
    {
        return 42;
    }

    public bool IsCheckSumEnabled()
    {
        return _enabled;
    }

    public virtual bool IsCheckSumOnlyMode()
    {
        return true;
    }

    public virtual bool IsByteStream()
    {
        return false;
    }

    public bool Equals(ChecksumEncoder? checksumEncoder)
    {
        if (checksumEncoder == null) return false;
        var checksum = checksumEncoder._checksum;
        var checksum2 = _checksum;

        if (!checksumEncoder._enabled) checksum = checksumEncoder._snapshotChecksum;
        if (!_enabled) checksum2 = _snapshotChecksum;
        return checksum == checksum2;
    }

    public int GetCheckSum()
    {
        return (int)_checksum;
    }

    public void ResetCheckSum()
    {
        _checksum = 0;
    }
}