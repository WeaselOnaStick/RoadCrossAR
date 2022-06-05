using UnityEngine;


public enum obstacle_type{
    CROSS_SIMPLE,
    CROSS_WITH_TRAFFIC_LIGHT,
}

[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public string scene_name;
    [TextArea(6,10)]
    public string intro_hint;
    public obstacle_type obstacle;
}
