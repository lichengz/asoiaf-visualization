using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[CreateAssetMenu(fileName = "MapScriptableObject", menuName = "asoiaf/MapScriptableObject", order = 0)]
public class MapScriptableObject : ScriptableObject {
    public List<string> mapNames;
}
