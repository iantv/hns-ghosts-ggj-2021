using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunController : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private TimeSpan currentTime;
    [SerializeField] private Transform sunTransform;
    [SerializeField] private Text timerText;
    [SerializeField] public int speed;

    private bool _isSunrise = false;
    private float _timer = 0f;
    private void Update()
    {
        ChangeTime();
    }

    private void ChangeTime()
    {
        if(_isSunrise)
            return;
        
        time += Time.deltaTime * speed;
        if (time > 864000)
        {
            time = 0;
        }
        currentTime = TimeSpan.FromSeconds(time);
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
        sunTransform.rotation = Quaternion.Euler(new Vector3((time - 21600)/86400*360,0f,0f));
    }
}
