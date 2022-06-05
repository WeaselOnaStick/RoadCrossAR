using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level List")]
public class LevelsListScriptableObject : ScriptableObject
{
    public List<LevelScriptableObject> levels_list;
}