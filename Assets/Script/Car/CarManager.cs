using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private List<CarController> cars;
    private int currentIndex = 0;   
    public CarController Car => cars[currentIndex];
    public void SwitchCar(int index)
    {
        cars[currentIndex].gameObject.SetActive(false);
        currentIndex = index;
        cars[currentIndex].gameObject.SetActive(true);
    }
    private void Start()
    {
        foreach (var car in cars)
        {
            car.gameObject.SetActive(false);
        }
        Car.gameObject.SetActive(true);
    }
}
