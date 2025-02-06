using BSL.v59.TitanEngine.DataS.ChecksumLogic;

namespace BSL.v59.TitanEngine.DataS.HelpsLogic;

public static class ByteStreamHelper
{
    public static LogicLong DecodeLogicLong(ByteStream byteStream)
    {
        var high = byteStream.ReadVInt();
        var low = byteStream.ReadVInt();

        return new LogicLong(high, low);
    }

    public static LogicLong EncodeLogicLong(ChecksumEncoder checksumEncoder, LogicLong value)
    {
        checksumEncoder.WriteVInt(value.GetHigherInt());
        checksumEncoder.WriteVInt(value.GetLowerInt());

        return value;
    }

    public static LogicLong EncodeLogicLong(ByteStream byteStream, LogicLong value)
    {
        byteStream.WriteVInt(value.GetHigherInt());
        byteStream.WriteVInt(value.GetLowerInt());

        return value;
    }

    public static int[] ReadDataReference(ByteStream byteStream)
    {
        var classId = byteStream.ReadVInt();
        if (classId == 0) return [0];
        var instanceId = byteStream.ReadVInt();

        return [classId, instanceId];
        //return classId + instanceId < 1 ? 0 : GlobalId.CreateGlobalId(classId, instanceId);
    }
    
    public static int WriteDataReference(ByteStream byteStream, int classId)
    {
        byteStream.WriteVInt(0);

        return classId;
    }
    
    public static int WriteDataReference(ByteStream byteStream, int classId, int instanceId)
    {
        if (classId > 0)
        {
            byteStream.WriteVInt(classId);
            byteStream.WriteVInt(instanceId);
        }
        else
        {
            byteStream.WriteVInt(0);
        }

        return instanceId;
    }

    public static List<int> ReadIntList(ByteStream byteStream)
    {
        var count = byteStream.ReadVInt();
        var list = new List<int>();
        
        for (var i = 0; i < count; i++)
            list.Add(byteStream.ReadVInt());
        
        return list;
    }
    
    public static void WriteIntList(ByteStream byteStream, List<int> list)
    {
        byteStream.WriteVInt(list.Count);
        foreach (var t in list.ToArray())
            byteStream.WriteVInt(t);
    }

    public static void WriteIntList(ChecksumEncoder checksumEncoder, List<int> list)
    {
        checksumEncoder.WriteVInt(list.Count);
        foreach (var t in list.ToArray())
            checksumEncoder.WriteVInt(t);
    }
}