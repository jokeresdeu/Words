using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class GameSerializator 
{
    public static T SerializeData<T>(string path)
    {
        Object deserialized = Resources.Load("Path");
        return JsonConvert.DeserializeObject<T>(deserialized.ToString());
    }

    public static void SaveData<T>(string path, T data)
    {
        string serialized = JsonConvert.SerializeObject(data);
    }
}
