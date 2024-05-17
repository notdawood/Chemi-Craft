using System;
using UnityEngine;

public static class Prefs
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void SubsystemRegistration()
    {
        OnDisplayNameChanged = null;
    }

    public const string StatScore = "Score";
    public const string StatHighScore = "HighScore";
    
    public static event Action OnDisplayNameChanged;
    public static string DisplayName {
        get => s_displayName;
        set {
            s_displayName = value;
            OnDisplayNameChanged?.Invoke();
        }
    }
    private static string s_displayName;

    public static string PlayFabId;

}
