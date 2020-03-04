using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBaseScriptableObject", menuName = "asoiaf/CharacterDataBaseScriptableObject", order = 0)]
public class CharacterDataBaseScriptableObject : ScriptableObject
{
    public List<string> characterNameList = new List<string>();
    public List<CharacterInfo> characterInfoList = new List<CharacterInfo>();
    public List<string> houseList = new List<string>();
    public List<Color> houseColorList = new List<Color>();
}