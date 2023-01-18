using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class NewSuspicion : MonoBehaviour
{
    [SerializeField] float suspicion = 0.0f;
    [SerializeField] float tabSuspicionCost = 0.25f;
    [SerializeField] float maxSuspicionLevel = 2.0f;
    [SerializeField] float alertSuspicionLevel = 1.5f;
    [SerializeField] float cautousSuspicionLevel = 1.0f;
    [SerializeField] float cheatCoolDown = 0.4f;
    float startViewTime = 0.0f;
    float lastViewTime = 0.0f;
    float startCoolDownTime = 0.0f;
    bool coolingDown = false;

    bool wasCaught = false;

    [SerializeField] Exam exam;
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips = new AudioClip[3];
    [SerializeField] Timer timer;
    [SerializeField] Text timerText;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips = new VideoClip[4];
    bool viewingCheatSheet = false;

    [SerializeField] string gameOverScene = "Game Over 1";



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startViewTime = Time.time;
        lastViewTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (wasCaught)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = true;
            ChangeTimeTextColor();
            suspicion += tabSuspicionCost;
            startViewTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = false;
            ChangeTimeTextColor();
            lastViewTime = Time.time;
            coolingDown = false;
        }

        if (viewingCheatSheet)
        {
            suspicion += Time.time - startViewTime;
            startViewTime = Time.time;
        }
        else
        {
            if (!coolingDown && Time.time - lastViewTime >= cheatCoolDown)
            {
                startCoolDownTime = Time.time;
                coolingDown = true;
            }
            if (coolingDown)
            {
                suspicion -= Time.time - startCoolDownTime;
                startCoolDownTime = Time.time;
            }
        }
        suspicion = Mathf.Clamp(suspicion, 0.0f, 10.0f);

        if (suspicion >= maxSuspicionLevel)
        {
            timer.InteruptTimer();
            wasCaught = true;
            exam.GetCaughtCheating();
            audioSource.mute = true;
            ChangeVideo(videoClips[3]);
            videoPlayer.frame = 0;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.Play();

        }
        else if (suspicion >= alertSuspicionLevel)
        {
            ChangeAudioClip(audioClips[2]);
            ChangeVideo(videoClips[2]);
        }
        else if (suspicion >= cautousSuspicionLevel)
        {
            ChangeAudioClip(audioClips[1]);
            ChangeVideo(videoClips[1]);
        }
        else
        {
            ChangeAudioClip(audioClips[0]);
            ChangeVideo(videoClips[0]);
        }
    }

    private void ChangeAudioClip(AudioClip clip)
    {
        if (audioSource.clip == clip)
            return;
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

    private void ChangeVideo(VideoClip clip)
    {
        if (videoPlayer == clip)
            return;
        double videoTime = videoPlayer.time;
        videoPlayer.clip = clip;
        //videoPlayer.Play();
        videoPlayer.time = videoTime;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Game Over 1");
    }
}
