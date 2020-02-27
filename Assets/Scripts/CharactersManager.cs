using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CharactersManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject[] characterCubes;
    // Start is called before the first frame update
    void Start()
    {
        // CharacterInfo info = 
        // CharacterBase test = new CharacterBase("test", "testHouse", "miniFace", "bigFace");
        string file = File.ReadAllText(Application.dataPath + "/Data/test.json");
        CharacterInfoCollection characterInfoCollection = JsonUtility.FromJson<CharacterInfoCollection>(file);
        GameObject go = Instantiate(characterPrefab);
        go.GetComponent<CharacterBase>().info = characterInfoCollection.characters[0];
    }

    // Update is called once per frame
    void Update()
    {

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