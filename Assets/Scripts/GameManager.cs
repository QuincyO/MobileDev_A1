using System;
using System.Collections;
using System.Collections.Generic;
using static Quincy.Structs.Scenes;
using Quincy.Structs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SoundManager))]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        OnSceneChange += SceneChange;
        SceneManager.activeSceneChanged += (replacedScene, newScene) => OnSceneChange?.Invoke((Scenes)newScene.buildIndex);
        trackables = new Dictionary<string, int>
        {
            {"Score", 0 },
            {"HighScore", 0 },
            {"Coins",0},
            {"Enemies",0}
        };

    }

    [Header("Bounds")]
    [SerializeField] public Boundary playerBoundary;
    [SerializeField] public Boundary playerBulletBoundary; 
    [SerializeField] public  Boundary enemyBoundary;
    [SerializeField] public Axis enemyRange;
    
    
    [Header("Trophies")]
    public Dictionary<string,int> trackables;
    
    private TextMeshProUGUI scoreText;
    
    //     Delegates    //
    public static event Action<Scenes> OnSceneChange;
    public static Scenes currentScene;

    
    
    // Start is called before the first frame update
    void Start()
    {
        OnSceneChange?.Invoke((Scenes)SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateScore(int newScore = 1)
    {
        if (scoreText == null) return;
        trackables["Score"] += newScore;
        if (trackables["Score"] > trackables["HighScore"])
        {
            trackables["HighScore"] = trackables["Score"];
        }
        UpdateScoreUI();
    }

    public static void UpdateScoreUI()
    {
        Instance.scoreText.text = "Score: " + Instance.trackables["Score"];
    }

    public void ChangeScene(Scenes nextScene)
    {
        SceneManager.LoadScene((int)nextScene);
        currentScene = nextScene;
    }

    private void SceneChange(Scenes nextScene)
    {
        scoreText = null;
        var highScore = trackables["Score"];
        var enemiesKilled = trackables["Enemies"];
        var coinsCollected = trackables["Coins"];
        
        switch (nextScene)
        {
            case Lose:
                trackables["HighScore"] = trackables["Score"];
                var lossWidgetElements = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var element in lossWidgetElements)
                {
                    if (element.name == "Coins")
                    {
                        element.text = $"Coins Collected: {coinsCollected}";
                        continue;
                    }

                    if (element.name == "HighScore")
                    {
                        element.text = $"High Score: {highScore}";
                        continue;
                    }

                    if (element.name == "Enemies")
                    {
                        element.text = $"Enemies Killed: {enemiesKilled}";
                    }
                }
                
                break;

            case Gameplay:
                var gameplayCanvasElements = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var obj in gameplayCanvasElements)
                {
                    if (obj.name != "ScoreText") continue;
                    scoreText = obj;
                    UpdateScore(0);
                    break;
                }
                break;
            
        }

        trackables["HighScore"] = 0;
        trackables["Score"] = 0;
        trackables["Coins"] = 0;
        trackables["Enemies"] = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
