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

    }

    void Init()
    {
        mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
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
        go.name = info.characterName + " line";
        if (info.houseName != null && info.houseName != "")
        {
            // go.GetComponent<LineRenderer>().material.color = characterDataBase.houseColorList[characterDataBase.houseList.IndexOf(info.houseName)];
            go.GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0);
            Color c = characterDataBase.houseColorList[characterDataBase.houseList.IndexOf(info.houseName)];
            go.GetComponent<LineRenderer>().endColor = new Color(c.r, c.g, c.b, 1);;
        }
        else
        {
            go.GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 0);
            go.GetComponent<LineRenderer>().endColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 1);
        }
        line = go.GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.SetPosition(currentPositionIndex, dotList[currentPositionIndex].position);
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
}
