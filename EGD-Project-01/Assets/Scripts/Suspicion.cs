using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;

public class Suspicion : MonoBehaviour
{
    [SerializeField] float suspicionTime = 1.5f;
    [SerializeField] Exam exam;
    AudioSource audioSource;
    [SerializeField] AudioClip audioNormal, audioSuspenseful;
    [SerializeField] Text timerText;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip videoNormal, videoSuspenseful;
    bool viewingCheatSheet = false;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = true;
            // Increase Time Scale
            Time.timeScale = suspicionTime;
            // Change audio
            ChangeAudioClip(audioSuspenseful);
            // Change Timer Color
            ChangeTimeTextColor();
            // Change video
            ChangeVideo(videoSuspenseful, new Color(1, 160.0f / 255.0f, 160.0f / 255.0f, 1));
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = false;
            Time.timeScale = 1;
            ChangeAudioClip(audioNormal);
            ChangeTimeTextColor();
            ChangeVideo(videoNormal, Color.white);
        }

    }

    private void ChangeAudioClip(AudioClip clip)
    {
        float audioTime = audioSource.time;
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.time = audioTime;
    }

    private void ChangeTimeTextColor()
    {
        if (viewingCheatSheet)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.black;
        }
    }

    private void ChangeVideo(VideoClip clip, Color color)
    {
        double videoTime = videoPlayer.time;
        videoPlayer.clip = clip;
        videoPlayer.Play();
        videoPlayer.time = videoTime;

        videoPlayer.gameObject.transform.GetComponentInChildren<RawImage>().color = color;
    }
}
