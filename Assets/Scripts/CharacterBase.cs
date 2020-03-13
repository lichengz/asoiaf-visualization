using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterBase : MonoBehaviour
{
    // Character info
    public CharacterInfo info;
    public TextMeshPro nameText;
    private MapManager mapManager;
    public MapScriptableObject mapAsset;
    public TimeLineScritableObject timeLineAsset;
    public CharacterDataBaseScriptableObject characterDataBase;
    // Points and Lines
    public GameObject linePrefab;
    private LineRenderer line;
    private int startSceneIndex;
    private UIManager uIManager;

    [SerializeField]
    private List<scenePositionPair> dotList = new List<scenePositionPair>();
    private int currentPositionIndex = 0;
    private Vector3 nextPosition;
    Dictionary<int, string> sceneToLocationDictionary = new Dictionary<int, string>();

    struct scenePositionPair
    {
        public int sceneIndex;
        public Vector3 position;
        public scenePositionPair(int index, Vector3 p)
        {
            sceneIndex = index;
            position = p;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        foreach (MeshRenderer mr in gameObject.transform.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = info.show;
        }
        line.enabled = info.show;
        // create line mesh collider
        Mesh mesh = new Mesh();
        line.BakeMesh(mesh, true);
        line.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        // Dead
        CheckDeath();
        // Kill
        CheckKill();
    }

    void Init()
    {
        mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
        uIManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
        gameObject.transform.position = mapManager.mapGameObjects[mapAsset.mapNames.IndexOf(timeLineAsset.sceneArray[timeLineAsset.currentScene].GetLocation())].position;
        gameObject.name = info.characterName;
        nameText.text = info.characterName;
        // dot and line
        dotList.Add(new scenePositionPair(timeLineAsset.currentScene, gameObject.transform.position));
        // Scene to Location Dictionary
        for (int i = 0; i < timeLineAsset.sceneArray.Length; i++)
        {
            Scene currentScene = timeLineAsset.sceneArray[i];
            Character[] characters = currentScene.characters;
            foreach (Character c in characters)
            {
                if (c.name == gameObject.name)
                {
                    sceneToLocationDictionary.Add(i, currentScene.GetLocation());
                    Vector3 curPosition = mapManager.mapGameObjects[mapAsset.mapNames.IndexOf(currentScene.GetLocation())].position;
                    if (dotList[dotList.Count - 1].position != curPosition)
                    {
                        dotList.Add(new scenePositionPair(i, curPosition));
                    }
                }
            }
        }
        // line
        GameObject go = Instantiate(linePrefab);
        //go.transform.parent = gameObject.transform;
        go.name = info.characterName + " line";
        line = go.GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.SetPosition(currentPositionIndex, dotList[currentPositionIndex].position);
        SetLineColor();
    }

    public void SetLineColor()
    {
        GameObject go = line.gameObject;
        if (!info.selected)
        {
            if (info.houseName != null && info.houseName != "")
            {
                // go.GetComponent<LineRenderer>().material.color = characterDataBase.houseColorList[characterDataBase.houseList.IndexOf(info.houseName)];
                go.GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0);
                Color c = characterDataBase.houseColorList[characterDataBase.houseList.IndexOf(info.houseName)];
                go.GetComponent<LineRenderer>().endColor = new Color(c.r, c.g, c.b, 1); ;
            }
            else
            {
                go.GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 0);
                go.GetComponent<LineRenderer>().endColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 1);
            }
        }
        else
        {
            go.GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 1);
            go.GetComponent<LineRenderer>().endColor = new Color(Color.red.r, Color.red.g, Color.red.b, 1);
        }

    }

    void UpdatePosition()
    {
        int sceneIndex = timeLineAsset.currentScene;
        if (sceneToLocationDictionary.ContainsKey(sceneIndex))
        {
            int start = dotList[currentPositionIndex].sceneIndex;
            Vector3 pos = mapManager.mapGameObjects[mapAsset.mapNames.IndexOf(sceneToLocationDictionary[sceneIndex])].position;
            if (sceneIndex == start)
                gameObject.transform.position = pos;
            if (dotList[currentPositionIndex].position != pos)
            {
                currentPositionIndex++;
                startSceneIndex = sceneIndex;
            }
            if (currentPositionIndex >= line.positionCount) line.positionCount++;
            line.SetPosition(currentPositionIndex, dotList[currentPositionIndex].position);
        }
        else if (currentPositionIndex < dotList.Count - 1)
        {
            int targetSceneIndex = dotList[currentPositionIndex + 1].sceneIndex;
            gameObject.transform.position = (float)(sceneIndex - startSceneIndex) / (targetSceneIndex - startSceneIndex) * (dotList[currentPositionIndex + 1].position - dotList[currentPositionIndex].position) + dotList[currentPositionIndex].position;
            line.positionCount = currentPositionIndex + 2;
            line.SetPosition(line.positionCount - 1, gameObject.transform.position);
        }
    }

    void CheckDeath()
    {
        int sceneIndex = timeLineAsset.currentScene;
        Scene currentScene = timeLineAsset.sceneArray[sceneIndex];
        // foreach (Character character in currentScene.characters)
        for (int i = 0; i < currentScene.characters.Length; i++)
        {
            if (info.show && info.characterName == currentScene.characters[i].name && currentScene.characters[i].alive == "false")
            {
                uIManager.alert.text = info.characterName + " killed by " + info.killedBy[0];// + " via " + currentScene.characters[i].mannerOfDeath;
            }
        }
    }

    void CheckKill()
    {
        if (info.selected)
        {
            int sceneIndex = timeLineAsset.currentScene;
            Scene currentScene = timeLineAsset.sceneArray[sceneIndex];
            foreach (Character character in currentScene.characters)
            {
                foreach (string killer in character.killedBy)
                {
                    if (killer == info.characterName)
                    {
                        Debug.Log(info.characterName + " killed " + character.name);
                    }
                }
            }
        }
    }
}
