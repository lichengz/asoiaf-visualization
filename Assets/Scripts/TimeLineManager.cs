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
    private float curSliderValue;
    private bool sliding = false;
    private Coroutine co;
    private int targetSliderValue = 0;
    public TimeLineScritableObject timeLineAsset;
    public MapScriptableObject mapAsset;
    public List<int>[] seasons = new List<int>[8];
    public EpisodesCollection episodesCollection;
    HashSet<string> set = new HashSet<string>();
    private CharactersManager charactersManager;

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
        charactersManager = gameObject.GetComponent<CharactersManager>();
        string file = File.ReadAllText(Application.dataPath + "/Data/episodes.json");
        episodesCollection = JsonUtility.FromJson<EpisodesCollection>(file);
        UpdateTimeLineAsset();
        slider.maxValue = timeLineAsset.totalNumOfScenes - 1;
        seasons[0] = timeLineAsset.season1;
        seasons[1] = timeLineAsset.season2;
        seasons[2] = timeLineAsset.season3;
        seasons[3] = timeLineAsset.season4;
        seasons[4] = timeLineAsset.season5;
        seasons[5] = timeLineAsset.season6;
        seasons[6] = timeLineAsset.season7;
        seasons[7] = timeLineAsset.season8;
        timeLineAsset.currentScene = (int)slider.value;
        UpdateUI();
    }

    public void UpdateUI()
    {
        slider.value = Mathf.Max(slider.value, curSliderValue);
        curSliderValue = slider.value;
        if (!sliding)
        {
            co = StartCoroutine(IncreaseSceneIndex((int)slider.value));
        }
        // while (timeLineAsset.currentScene < (int)slider.value) timeLineAsset.currentScene++;
        // EpisodeData epiData = CalculateEpisodeData(timeLineAsset.currentScene);
        // timeLineAsset.currentSeason = epiData.seasonIndex;
        // timeLineAsset.currentEpisode = epiData.episodeIndex;
        // expisodeCounter.text = String.Format("Season {0}, Episode {1}, Scene {2}/{3}", epiData.seasonIndex + 1, epiData.episodeIndex + 1, CalculateSceneProgrerss(timeLineAsset.currentScene) + 1, timeLineAsset.scenesInEachEpisode[CalculateEpiIndex(timeLineAsset.currentScene)]);
    }
    private IEnumerator IncreaseSceneIndex(int value)
    {
        sliding = true;
        targetSliderValue = value;
        while (timeLineAsset.currentScene != value)
        {
            yield return new WaitForSeconds(0.01f);
            timeLineAsset.currentScene++;
            EpisodeData epiData = CalculateEpisodeData(timeLineAsset.currentScene);
            timeLineAsset.currentSeason = epiData.seasonIndex;
            timeLineAsset.currentEpisode = epiData.episodeIndex;
            expisodeCounter.text = String.Format("Season {0}, Episode {1}, Scene {2}/{3}", epiData.seasonIndex + 1, epiData.episodeIndex + 1, CalculateSceneProgrerss(timeLineAsset.currentScene) + 1, timeLineAsset.scenesInEachEpisode[CalculateEpiIndex(timeLineAsset.currentScene)]);
        }
        sliding = false;
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

    private void UpdateTimeLineAsset()
    {
        if (timeLineAsset.fileLoaded) return;
        mapAsset.mapNames.Clear();
        foreach (Episode epi in episodesCollection.episodes)
        {
            foreach (Scene scene in epi.scenes)
            {
                string location = (scene.subLocation != "" && scene.subLocation != null) ? scene.location + "_" + scene.subLocation : scene.location;
                if (!set.Contains(location))
                {
                    set.Add(location);
                    mapAsset.mapNames.Add(location);
                }
            }
        }
        GetScenes();
        foreach (Episode epi in episodesCollection.episodes)
        {
            foreach (Scene scene in epi.scenes)
            {
                string location = (scene.subLocation != "" && scene.subLocation != null) ? scene.location + "_" + scene.subLocation : scene.location;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAll();
        }
    }

    void GetScenes()
    {
        List<Scene> sceneList = new List<Scene>();
        foreach (Episode episode in episodesCollection.episodes)
        {
            sceneList.AddRange(episode.scenes);
        }
        timeLineAsset.sceneArray = sceneList.ToArray();
    }

    void ResetAll()
    {
        timeLineAsset.currentScene = 0;
        slider.value = 0;
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject go in characters) Destroy(go);
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (GameObject go in lines) Destroy(go);
        StopCoroutine(co);
        sliding = false;
        curSliderValue = 0;
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
    public string GetLocation()
    {
        return (subLocation != "" && subLocation != null) ? location + "_" + subLocation : location;
    }
}

[System.Serializable]
public class Character
{
    public string name;
}
