using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeLineScritableObject", menuName = "asoiaf/TimeLineScritableObject", order = 0)]
public class TimeLineScritableObject : ScriptableObject
{
    public bool fileLoaded;
    public int currentSeason;
    public int currentEpisode;
    public int currentScene;
    public int totalNumOfScenes;
    public List<int> scenesInEachEpisode;
    public List<int> season1 = new List<int>();
    public List<int> season2 = new List<int>();
    public List<int> season3 = new List<int>();
    public List<int> season4 = new List<int>();
    public List<int> season5 = new List<int>();
    public List<int> season6 = new List<int>();
    public List<int> season7 = new List<int>();
    public List<int> season8 = new List<int>();
}
