using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonArray
{
    private JsonArray() { }

    [Serializable]
    struct Wrapper<T>
    {
        public T[] array;
    }

    public static T[] Retrieve<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{ \"array\": " + json + "}");
        return wrapper.array;
    }
}
