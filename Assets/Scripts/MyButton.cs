using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using static Quincy.Structs.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{

     [SerializeField] private Scenes _scene = None;

    
    // Start is called before the first frame update
    void Start()
    {
        if (_scene == None)
        {
            Debug.Log($"Failed to load scene, Defaulted to main menu");
            SceneManager.LoadScene((int)MainMenu);
        }

        GetComponent<Button>().onClick
            .AddListener(delegate { SoundManager.Instance.sounds["UI_Button_Select"].Play(); GameManager.Instance.ChangeScene(_scene); });
        
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
