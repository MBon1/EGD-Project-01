using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float examTime = 10;
    public bool timerRunning = false;
    public Text timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        timerRunning = true;
        Debug.Log("Start the clock!");
        timerDisplay.GetComponent<Text>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRunning)
        {
            if(examTime > 0)
            {
                examTime -= Time.deltaTime;
                timerUpdate(examTime);
            }
            else
            {
                Debug.Log("Put down your pencils!");
                examTime = 0;
                timerRunning = false;
                timerUpdate(examTime);
            }
        }
    }

    void timerUpdate(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000;
        timerDisplay.text = string.Format("{0:00}:{1:000}", seconds, milliseconds);
    }
}