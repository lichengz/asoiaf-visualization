using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CharactersManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject[] characterCubes;
    CharacterInfoCollection characterInfoCollection;
    CharacterIncludeCollection characterIncludeCollection;
    HashSet<string> characterSet = new HashSet<string>();
    Dictionary<string, bool> includeDictionary = new Dictionary<string, bool>();
    public CharacterDataBaseScriptableObject characterDataBase;


    // Timeline Manager
    private TimeLineManager timeLineManager;
    //public Scene[] sceneArray;
    public TimeLineScritableObject timeLineAsset;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Character character in timeLineAsset.sceneArray[timeLineAsset.currentScene].characters)
        {
            if (!GameObject.Find(character.name))
            {
                SpawnCharacter(character);
            }
        }
    }

    void Init()
    {
        timeLineManager = gameObject.GetComponent<TimeLineManager>();
        string characterfile = File.ReadAllText(Application.dataPath + "/Data/characters.json");
        characterInfoCollection = JsonUtility.FromJson<CharacterInfoCollection>(characterfile);
        string includefile = File.ReadAllText(Application.dataPath + "/Data/characters-include.json");
        characterIncludeCollection = JsonUtility.FromJson<CharacterIncludeCollection>(includefile);
        foreach (CharacterInclude include in characterIncludeCollection.include)
        {
            includeDictionary.Add(include.name, include.include);
        }
        // foreach (CharacterInfo info in characterInfoCollection.characters)
        // {
        //     if (includeDictionary.ContainsKey(info.characterName) && includeDictionary[info.characterName])
        //     {
                // if (!characterSet.Contains(info.characterName))
                // {
                //     characterSet.Add(info.characterName);
                    //meta data
                    // characterDataBase.characterNameList.Add(info.characterName);
                    //characterDataBase.characterInfoList.Add(info);
                    // if (!characterDataBase.houseList.Contains(info.houseName) && info.houseName != "" && info.houseName != null)
                    // {
                    //     characterDataBase.houseList.Add(info.houseName);
                    // }
                // }
        //     }
        // }
    }

    void SpawnCharacter(Character character)
    {
        // if (!characterDictionary.ContainsKey(character.name)) return;
        if(!characterDataBase.characterNameList.Contains(character.name)) return;
        GameObject go = Instantiate(characterPrefab);
        int characterIndex = characterDataBase.characterNameList.IndexOf(character.name);
        go.GetComponent<CharacterBase>().info = characterDataBase.characterInfoList[characterIndex];
    }
}

[Serializable]
public class CharacterInfoCollection
{
    public CharacterInfo[] characters;
}

[Serializable]
public class CharacterInfo
{
    public bool show;
    public string characterName;
    public string houseName;
    public string characterImageThumb;
    public string characterImageFull;
    public string killedBy;
}

[Serializable]
public class CharacterIncludeCollection
{
    public CharacterInclude[] include;
}

[Serializable]
public class CharacterInclude
{
    public string name;
    public bool include;
}