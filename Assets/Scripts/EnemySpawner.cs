using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    
    private LinkedList<GameObject> _activeEnemies;
    private LinkedList<GameObject> _inActiveEnemies;
    public GameObject[] enemyPrefab;
   [SerializeField] Axis SpawnerRange;

    public float enemiesToSpawn = 10;


    [SerializeField] private Boundary Bounds;

    private bool canSpawn = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        _activeEnemies = new LinkedList<GameObject>();
        _inActiveEnemies = new LinkedList<GameObject>();
        GameManager.OnSceneChange += SceneChange;


    }

    private void SceneChange(Scenes scene)
    {
        
        #region Hard to understand Switch Expression
        //scene switch: This is a switch expression that checks the value of scene and performs different actions based on the result.
        canSpawn = scene switch
        {
            //Scenes.Gameplay => true,: If scene is equal to Scenes.Gameplay, then the expression evaluates to true.
            Scenes.Gameplay => true,
            //_ => false: The underscore (_) is a discard pattern that matches anything not covered by the previous case. So if scene is not Scenes.Gameplay, the expression evaluates to false.
            _ => false
        };
        #endregion

        #region Doing the same as above
        #if false 
               canSpawn = false;
       switch (scene)
       {
           case Scenes.Gameplay:
               canSpawn = true;
               break;
       }
        #endif
        #endregion

    }

    // Start is called before the first frame update
    void Start()
    {
        Bounds = GameManager.Instance.enemyBoundary;
        SpawnerRange = GameManager.Instance.enemyRange;
        if (enemyPrefab.Length > 0)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                var prefab = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length - 1)]);
                var enemyController = prefab.GetComponent<EnemyBehaviour>();
                enemyController.Bounds = this.Bounds;
                
                _inActiveEnemies.AddLast(prefab);
                prefab.SetActive(false);
            }
        }
        
        InvokeRepeating(nameof(DropEnemy), 1f,Random.Range(0.8f, 1.2f));
    }

    void DropEnemy()
    {
        if (_inActiveEnemies.Last != null)
        {
            var enemyToDrop = _inActiveEnemies.Last;
            _inActiveEnemies.Remove(enemyToDrop);
        
            enemyToDrop.Value.SetActive(true);
            enemyToDrop.Value.GetComponent<EnemyBehaviour>().isActive = true;
        
        
            float xValue = Random.Range(SpawnerRange.min, SpawnerRange.max);
            enemyToDrop.Value.transform.position = new Vector3(xValue, Bounds.y.max, 0);
        }
        else
        {
            var enemyToDrop = Instantiate(enemyPrefab[Random.Range(0,enemyPrefab.Length - 1)]);
            _activeEnemies.AddLast(gameObject);
            
            float xValue = Random.Range(Bounds.x.min, Bounds.x.max);
            enemyToDrop.transform.position = new Vector3(xValue, Bounds.y.max, 0);
        }

    }


    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        _inActiveEnemies.AddLast(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
