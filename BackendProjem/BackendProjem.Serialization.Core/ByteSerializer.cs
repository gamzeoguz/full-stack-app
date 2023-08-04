using System.Runtime.Serialization.Formatters.Binary;

namespace BackendProjem.Serialization.Core;

public class ByteSerializer : IByteSerializer
{
    public TObject Deserialize<TObject>(byte[] value)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(value))
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            object obj = bf.Deserialize(ms);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            return (TObject)obj;
        }
    }

    public byte[] Serialize(object value)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            bf.Serialize(ms, value);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            return ms.ToArray();
        }
    }
}
