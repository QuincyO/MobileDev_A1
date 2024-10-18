using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barslider : MonoBehaviour
{
    public float maxValue;
    public float currentValue;
    [SerializeField] Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_slider == null) Debug.LogWarning($"{nameof(gameObject)} is null");
        var player = FindObjectOfType<PlayerController>();
        _slider.maxValue = player.MaxHealth;
        _slider.value = player.Health;
        player.HitEvent += o => _slider.value = player.Health;



        // Update is called once per frame
        void Update()
        {

        }
    }
}
