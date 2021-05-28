using UnityEngine;

public static class PlayerPrefsManager
{
    public static BoolPref FirstEnter { get; private set; }
    public static BoolPref GameDataDownloaded { get; private set; }
    public static IntPref CoinsAmount { get; private set; }

    public static IntPref GameVersion { get; private set; }
    public static StringPref PlayedLvls { get; private set; }

    static PlayerPrefsManager()
    {
        FirstEnter = new BoolPref("FirstEnter", true);
        GameDataDownloaded = new BoolPref("GameDataDownloaded", false);
        PlayedLvls = new StringPref("PlayedLvls", string.Empty);
        CoinsAmount = new IntPref("CoinsAmount", 10);
        GameVersion = new IntPref("GameVersion", 1);
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    public static string GetStringPref(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
           return PlayerPrefs.GetString(key);
        }

        return string.Empty;
    }

    public static void SaveStringPref(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}

public abstract class Pref<T>
{
    protected string Key;
    protected T DefaultValue;

    public bool IsSaved => PlayerPrefs.HasKey(Key);
   
    public Pref(string key, T defaultValue)
    {
        Key = key;
        DefaultValue = defaultValue;
        if (!IsSaved)
        {
            Set(DefaultValue);
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(Key);
    }

    public abstract T Get();

    public abstract void Set(T value);

    public void DeleteKey()
    {
        PlayerPrefs.DeleteKey(Key);
    }
}

public class BoolPref : Pref<bool>
{
   public BoolPref(string key,  bool defaultValue) : base(key, defaultValue)
   {
     
   }
    
   public override bool Get()
   {
        if (!IsSaved)
        {
            Set(DefaultValue);
        }
           
        return PlayerPrefs.GetInt(Key) == 1;
   }

    public override void Set(bool value)
    {
        PlayerPrefs.SetInt(Key, value ? 1 : 0);
    }
}


public class IntPref : Pref<int>
{
    public IntPref(string key, int defaultValue): base(key, defaultValue)
    {

    }

    public void ChangeValue(int value)
    {
        int currentValue = Get();
        Set(currentValue + value);
    }

    public override int Get()
    {
        if (!IsSaved)
        {
            Set(DefaultValue);
        }
        return PlayerPrefs.GetInt(Key);
    }

    public override void Set(int value)
    {
        PlayerPrefs.SetInt(Key, value);
    }
}

public class StringPref : Pref<string>
{
    public StringPref(string key, string defaultValue) : base(key, defaultValue)
    {

    }

    public override string Get()
    {
        if (!IsSaved)
        {
            Set(DefaultValue);
        }
        return PlayerPrefs.GetString(Key);
    }

    public override void Set(string value)
    {
        PlayerPrefs.SetString(Key, value);
    }
}

public class FloatPref : Pref<float>
{
    public FloatPref(string key, float defaultValue) : base(key, defaultValue)
    {

    }

    public override float Get()
    {
        if (!IsSaved)
        {
            Set(DefaultValue);
        }
        return PlayerPrefs.GetFloat(Key);
    }

    public override void Set(float value)
    {
        PlayerPrefs.SetFloat(Key, value);
    }

    public void ChangeValue(float value)
    {
        float currentValue = Get();
        Set(currentValue + value);
    }
}
