using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunController : MonoBehaviour
{
    [SerializeField] private TimeSpan currentTime;
    [SerializeField] private Transform sunTransform;
    [SerializeField] private Text timerText;
    [SerializeField] private MainGameSettings settings;

    private int _speed;
    private float _time;
    private bool _isSunrise = false;
    private float _timer = 0f;

    public void Start()
    {
        _time = settings.currentTimeOfDay;
        _speed = settings.dayNightCycleSpeed;
    }

    private void Update()
    {
        ChangeTime();
    }

    private void ChangeTime()
    {
        if(_isSunrise)
            return;
        
        _time += Time.deltaTime * _speed;
        if (_time > 864000)
        {
            _time = 0;
        }
        currentTime = TimeSpan.FromSeconds(_time);
        if (currentTime.Hours == 6)
        {
            _timer = 60 - currentTime.Minutes;
            if (_timer == 1f)
            {
                timerText.text = "Рассвет!";
                _isSunrise = true;
                return;
            }
            timerText.text = "До рассвета : " + _timer + " минут!";
        }
        sunTransform.rotation = Quaternion.Euler(new Vector3((_time - 21600)/86400*360,0f,0f));
    }
}
