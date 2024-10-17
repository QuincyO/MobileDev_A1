using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string, AudioSource> sounds { get;private set; }

    [SerializeField] private List<string> keyNames;

    public static SoundManager Instance { get; private set; }



    public float volume
    {
        get { return _volume; }
        set
        {
            _volume = value/100f;
            sounds["MainMenuMusic"].volume = Mathf.Clamp(_volume, 0f, 1f);
        }
    }

    private float _volume;

    [Range(0,100)]public  float EditorMusicVolume;

    private void Awake()
    {
        #region Instance Creation
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        #endregion
        
        #region Initialization of Variables
        sounds = new Dictionary<string,AudioSource>();
        keyNames = new List<string>();
        #endregion
        
        #region Binding Function Calls
        GameManager.OnSceneChange += ChangeScene;
        
        #endregion
        
        #region Populating Sounds
        var clips =  Resources.LoadAll<AudioClip>("Sounds");
        foreach (var clip in clips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            
            sounds.Add(clip.name, source);
            keyNames.Add(clip.name);
        }
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    

    #region SceneChange
    
    
    
    void ChangeScene(Scenes scene)
    {
        

        
        switch (scene)
        {
            
            case Scenes.MainMenu:
                if (sounds["MainMenuMusic"].isPlaying) break;
                sounds["MainMenuMusic"].Play();
                sounds["MainMenuMusic"].loop = true;
                break;
            case Scenes.Instructions:
                
                break;
            
            case Scenes.Gameplay:

                break;
            
            case Scenes.Lose:

                break;
            default:
                break;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        volume = EditorMusicVolume;
    }
}
