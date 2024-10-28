using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    Slider _speedSlider;
    private void Start()
    {
        _speedSlider.maxValue = 100;
    }
    void Update()
    {
        _speedSlider.value = CarController.Instance.CurrentSpeed;
    }

}
