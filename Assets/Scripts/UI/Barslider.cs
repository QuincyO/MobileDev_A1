using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        player.HealthChanged += (controller) => StartCoroutine(HandleHit(controller));

        player.HealthChanged += (HandleeHit);


        //player.HealthChanged += HandleHit;
    }


    void HandleeHit(PlayerController player)
    {
        StartCoroutine(HandleHit(player));
    }

    IEnumerator HandleHit(PlayerController player)
    {
        float elapsedTime = 0f;
        float duration = .3f;
        float startValue = _slider.value;
        float endValue = player.Health;

        while (elapsedTime < duration)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Lerp between the current slider value and the target value (player.Health)
            _slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            
            
            yield return null;
        }

        _slider.value = player.Health;
    }
}
