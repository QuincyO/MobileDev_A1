using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudSpawner : MonoBehaviour
{
    public static CloudSpawner Instance{get; private set;}

    private LinkedList<GameObject> _inactiveClouds;
    private LinkedList<GameObject> _slowClouds;
    private LinkedList<GameObject> _mediumClouds;
    private LinkedList<GameObject> _fastClouds;
    
    [Header("Cloud Speeds")]
    [SerializeField] float SlowCloudSpeed = .25f;
    [SerializeField] float MediumCloudSpeed = 1f;
    [SerializeField] float FastCloudSpeed = 1.5f;
    [Space]

    [SerializeField] int MAX_ACTIVE_CLOUDS = 5;
    [SerializeField] private int _totalActiveClouds;
    private const int MAX_CLOUDS = 50;
    
    public Boundary cloudBoundaries;



    void Awake()
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

        GameManager.OnSceneChange += InitClouds;
        
        _inactiveClouds = new LinkedList<GameObject>();
        _slowClouds = new LinkedList<GameObject>();
        _mediumClouds = new LinkedList<GameObject>();
        _fastClouds = new LinkedList<GameObject>();
        
    }

    void InitClouds(Scenes loadedScene)
    {
        #region Destorying Clouds And Clearing List
        foreach (var obj in _inactiveClouds) Destroy(obj);
        foreach (var obj in _slowClouds) Destroy(obj);
        foreach (var obj in _mediumClouds) Destroy(obj);
        foreach (var obj in _fastClouds) Destroy(obj);
        _inactiveClouds.Clear();
        _slowClouds.Clear();
        _mediumClouds.Clear();
        _fastClouds.Clear();
        #endregion

        _totalActiveClouds = 0;
        
        for (int i = 0; i < MAX_CLOUDS; i++)
        {
            var cloud = CloudFactory.MakeCloud();
            cloud.Item1.name = "Cloud " + i;
            cloud.Item1.transform.position = new Vector3(0, 0, 0);
            cloud.Item2.Bounds = cloudBoundaries;
            cloud.Item2.Speed = (Cloud.SpeedEnum)Random.Range(1,4);
            _inactiveClouds.AddLast(cloud.Item1);
        }
        
    }
  
    void Start()
    {
    }

    void SendCloud()
    {
        if(_totalActiveClouds >= MAX_ACTIVE_CLOUDS || _inactiveClouds.Last == null) return;

        int randomSpeedSelector = Random.Range(1, 4);
        var cloud = _inactiveClouds.Last.Value;
        var cloudScript = cloud.GetComponent<Cloud>();

        switch (randomSpeedSelector)
        {
            case 1: //Slow Speed
                _slowClouds.AddLast(cloud);
                cloudScript.CloudSpeed = SlowCloudSpeed;
                cloudScript.Speed = Cloud.SpeedEnum.Slow;
                break;
            case 2: //Medium Speed
                _mediumClouds.AddLast(cloud);
                cloudScript.CloudSpeed = MediumCloudSpeed;
                cloudScript.Speed = Cloud.SpeedEnum.Medium;
                break;
            case 3://Fast Speed
                _fastClouds.AddLast(cloud);
                cloudScript.CloudSpeed = FastCloudSpeed;
                cloudScript.Speed = Cloud.SpeedEnum.Fast;

                break;
            default:
                _slowClouds.AddLast(cloud);
                cloudScript.CloudSpeed = SlowCloudSpeed;
                cloudScript.Speed = Cloud.SpeedEnum.Slow;
                break;
                
        }

        _inactiveClouds.Remove(cloud);
        cloud.SetActive(true);
        _totalActiveClouds++;
    }

    public void ReturnToPool((Cloud,GameObject) tuple)
    {
        var script = tuple.Item1;
        var cloudObject = tuple.Item2;
        
       //if (_slowClouds.Contains(cloudObject)) _slowClouds.Remove(cloudObject);
       //if (_mediumClouds.Contains(cloudObject)) _mediumClouds.Remove(cloudObject);
       //if (_fastClouds.Contains(cloudObject)) _fastClouds.Remove(cloudObject);
        #region Verbose and dumb
        switch (script.Speed)
        {
            case Cloud.SpeedEnum.None:
                return;
            case Cloud.SpeedEnum.Slow:
                _slowClouds.Remove(cloudObject);
                break;
            case Cloud.SpeedEnum.Medium:
                _mediumClouds.Remove(cloudObject);
                break;
            case Cloud.SpeedEnum.Fast:
                _fastClouds.Remove(cloudObject);
                break;

            default:
                if (_slowClouds.Contains(cloudObject)) _slowClouds.Remove(cloudObject);
                if (_mediumClouds.Contains(cloudObject)) _mediumClouds.Remove(cloudObject);
                if (_fastClouds.Contains(cloudObject)) _fastClouds.Remove(cloudObject);
                break;
                
        }
        #endregion
        
        script.Speed = Cloud.SpeedEnum.None;
        _inactiveClouds.AddLast(cloudObject);
        _totalActiveClouds--;
    }

    float timeSinceLastCloud = 0f;

    [SerializeField] private float minTimeBetweenClouds = 0.2f;
    [SerializeField] private float maxTimeBetweenClouds = 0.8f;
    private float newCloudTime = 0.5f;
    // Update is called once per frame
    void Update()
    {
        timeSinceLastCloud += Time.deltaTime;
        if (timeSinceLastCloud > newCloudTime)
        {
            SendCloud();
            timeSinceLastCloud -= newCloudTime;
            newCloudTime = Random.Range(minTimeBetweenClouds, maxTimeBetweenClouds);
        }
    }
}
