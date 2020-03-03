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
    Dictionary<string, CharacterInfo> characterDictionary = new Dictionary<string, CharacterInfo>();

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
        string file = File.ReadAllText(Application.dataPath + "/Data/test.json");
        characterInfoCollection = JsonUtility.FromJson<CharacterInfoCollection>(file);
        foreach (CharacterInfo info in characterInfoCollection.characters)
        {
            if (!characterDictionary.ContainsKey(info.characterName)) characterDictionary.Add(info.characterName, info);
        }
    }

    void SpawnCharacter(Character character)
    {
        if (!characterDictionary.ContainsKey(character.name)) return;
        GameObject go = Instantiate(characterPrefab);
        go.GetComponent<CharacterBase>().info = characterDictionary[character.name];
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
    public string characterName;
    public string houseName;
    public string characterImageThumb;
    public string characterImageFull;
    public string killedBy;
}