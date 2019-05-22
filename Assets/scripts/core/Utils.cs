using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Utils
{

    public static string StringFromBytes(byte[] b)
    {
        return Encoding.ASCII.GetString(b);
    }

    public static byte[] BytesFromString(string s)
    {
        return Encoding.ASCII.GetBytes(s);
    }


    public static byte[] SerializeToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static object Deserialize(this byte[] byteArray)
    {
        if (byteArray == null)
        {
            return null;
        }
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(byteArray, 0, byteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }


}
