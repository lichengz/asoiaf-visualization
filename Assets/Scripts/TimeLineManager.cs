using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TimeLineManager : MonoBehaviour
{
    // Start is called before the first frame update
    JSONObject j;
    void Start()
    {
        // string encodedString = "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";
        // j = new JSONObject(encodedString);
        // accessData(j);
        string file = File.ReadAllText(Application.dataPath + "/Data/test.json");
        Debug.Log(file);
        ScenesCollection sceneCollection = JsonUtility.FromJson<ScenesCollection>(file);
        Debug.Log(sceneCollection.scenes.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

}

[System.Serializable]
public class ScenesCollection
{
    public Scene[] scenes;
}

[System.Serializable]
public class Scene
{
    public string sceneStart;
    public string sceneEnd;
    public string location;
    public string subLocation;
    public string[] characters;
}
