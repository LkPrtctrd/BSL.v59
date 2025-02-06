using System.Collections.Frozen;
using BSL.v59.Logic.Protocol;

namespace BSL.v59.Logic.OknoLayer;

public static class LogicLaserMessageFactory
{
    private static FrozenDictionary<int, Type>? _messages;

    static LogicLaserMessageFactory()
    {
        try
        {
            _messages = typeof(PiranhaMessage).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(PiranhaMessage)))
                .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                .ToDictionary(messageTypeof =>
                    (Activator.CreateInstance(messageTypeof) as PiranhaMessage)!.GetMessageType())
                .ToFrozenDictionary();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static PiranhaMessage? CreateMessageByType(int type)
    {
        if (_messages != null && _messages.TryGetValue(type, out var message))
            return Activator.CreateInstance(message) as PiranhaMessage;
        return null;
    }

    public static void Destruct()
    {
        _messages = null;
    }
}