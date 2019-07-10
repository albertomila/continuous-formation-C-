using System.Collections;

public class Singleton<T> where T : class, new()
{
    public static T Instance { get; private set; } = null;

    public static void CreateInstance()
    {
        Instance = new T();
    }
    public static void DestroyInstance()
    {
        Instance = null;
    }

    public static T Get()
    {
        if(Instance == null)
        {
            CreateInstance();
        }
        return Instance;
    }

    public static bool IsValid()
    {
        return Instance != null;
    }
}
