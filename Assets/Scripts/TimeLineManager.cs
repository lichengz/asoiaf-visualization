using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class TimeLineManager : MonoBehaviour
{
    public Text expisodeCounter;
    public Slider slider;
    public TimeLineScritableObject timeLineAsset;
    public MapScriptableObject mapAsset;
    public List<int>[] seasons = new List<int>[8];
    EpisodesCollection episodesCollection;
    HashSet<string> set = new HashSet<string>();

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInputHandler();
    }

    void OnApplicationQuit()
    {
        //Resources.UnloadAsset(timeLineAsset);
    }

    private void Init()
    {
        string file = File.ReadAllText(Application.dataPath + "/Data/episodes.json");
        episodesCollection = JsonUtility.FromJson<EpisodesCollection>(file);
        UpdateTimeLineAsset(episodesCollection);
        slider.maxValue = timeLineAsset.totalNumOfScenes - 1;
        seasons[0] = timeLineAsset.season1;
        seasons[1] = timeLineAsset.season2;
        seasons[2] = timeLineAsset.season3;
        seasons[3] = timeLineAsset.season4;
        seasons[4] = timeLineAsset.season5;
        seasons[5] = timeLineAsset.season6;
        seasons[6] = timeLineAsset.season7;
        seasons[7] = timeLineAsset.season8;
        UpdateUI();
    }

    public void UpdateUI()
    {
        int sceneIndex = (int)slider.value;
        EpisodeData epiData = CalculateEpisodeData(sceneIndex);
        timeLineAsset.currentSeason = epiData.seasonIndex;
        timeLineAsset.currentEpisode = epiData.episodeIndex;
        timeLineAsset.currentScene = sceneIndex;
        expisodeCounter.text = String.Format("Season {0}, Episode {1}, Scene {2}/{3}", epiData.seasonIndex + 1, epiData.episodeIndex + 1, CalculateSceneProgrerss(sceneIndex) + 1, timeLineAsset.scenesInEachEpisode[CalculateEpiIndex(sceneIndex)]);
    }

    private int CalculateSceneProgrerss(int sceneIndex)
    {
        int scene = 1;
        int tmp = 0;
        for (int i = 0; i < timeLineAsset.scenesInEachEpisode.Count; i++)
        {
            tmp += timeLineAsset.scenesInEachEpisode[i];
            if (sceneIndex < tmp)
            {
                return timeLineAsset.scenesInEachEpisode[i] - (tmp - sceneIndex);
            }
        }
        return scene;
    }

    private int CalculateEpiIndex(int sceneIndex)
    {
        int epiIndex = 0;
        int tmp = 0;
        for (int i = 0; i < timeLineAsset.scenesInEachEpisode.Count; i++)
        {
            tmp += timeLineAsset.scenesInEachEpisode[i];
            if (sceneIndex < tmp)
            {
                return i;
            }
        }
        return epiIndex;
    }

    private EpisodeData CalculateEpisodeData(int sceneIndex)
    {
        EpisodeData epiData = new EpisodeData(0, 0);
        int epiIndex = CalculateEpiIndex(sceneIndex);
        int tmp = 0;
        for (int i = 0; i < seasons.Length; i++)
        {
            tmp += seasons[i].Count;
            if (epiIndex < tmp)
            {
                epiData.seasonIndex = i;
                epiData.episodeIndex = seasons[i].Count - (tmp - epiIndex);
                return epiData;
            }
        }
        return epiData;
    }

    private void UpdateTimeLineAsset(EpisodesCollection episodesCollection)
    {
        mapAsset.mapNames.Clear();
        foreach (Episode epi in episodesCollection.episodes){
            foreach (Scene scene in epi.scenes)
            {
                string location = (scene.subLocation != "" && scene.subLocation != null) ? scene.location + "_" +scene.subLocation : scene.location;
                if (!set.Contains(location))
                {
                    set.Add(location);
                    mapAsset.mapNames.Add(location);
                }
            }
        }
        if (timeLineAsset.fileLoaded) return;
        foreach (Episode epi in episodesCollection.episodes)
        {
            foreach (Scene scene in epi.scenes)
            {
                string location = (scene.subLocation != "" && scene.subLocation != null) ? scene.location + "_" +scene.subLocation : scene.location;
                if (!set.Contains(location))
                {
                    set.Add(location);
                    mapAsset.mapNames.Add(location);
                }
            }

            timeLineAsset.totalNumOfScenes += epi.scenes.Length;
            timeLineAsset.scenesInEachEpisode.Add(epi.scenes.Length);
            if (epi.seasonNum == 1)
            {
                timeLineAsset.season1.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 2)
            {
                timeLineAsset.season2.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 3)
            {
                timeLineAsset.season3.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 4)
            {
                timeLineAsset.season4.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 5)
            {
                timeLineAsset.season5.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 6)
            {
                timeLineAsset.season6.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 7)
            {
                timeLineAsset.season7.Add(epi.scenes.Length);
            }
            if (epi.seasonNum == 8)
            {
                timeLineAsset.season8.Add(epi.scenes.Length);
            }
        }
        timeLineAsset.fileLoaded = true;
    }

    private void KeyInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && timeLineAsset.currentScene > 0)
        {
            slider.value--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && timeLineAsset.currentScene < timeLineAsset.totalNumOfScenes - 1)
        {
            slider.value++;
        }
    }

}

public struct EpisodeData
{
    public int seasonIndex, episodeIndex;

    public EpisodeData(int s, int e)
    {
        seasonIndex = s;
        episodeIndex = e;
    }
}

[System.Serializable]
public class EpisodesCollection
{
    public Episode[] episodes;
}

[System.Serializable]
public class Episode
{
    public int seasonNum;
    public int episodeNum;
    public string episodeTitle;
    public string episodeDescription;
    public Scene[] scenes;
}

[System.Serializable]
public class Scene
{
    public string sceneStart;
    public string sceneEnd;
    public string location;
    public string subLocation;
    public Character[] characters;
}

[System.Serializable]
public class Character
{
    public string name;
}
