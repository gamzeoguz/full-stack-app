namespace BackendProjem.Serialization.Core;

/// <summary>
/// Byte array serilization için kullanılacak intarface.
/// </summary>
public interface IByteSerializer
{
    /// <summary>
    /// Parametre geçilen objeyi byte array olarak serilize edip geri döner.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    byte[] Serialize(object value);


    /// <summary>
    /// Serilize edilmiş byte array değerini TValue tipinde geri döner.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    TObject Deserialize<TObject>(byte[] value);
}
