using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string LevelName;
    public bool ResetExam;
    
    public void LoadLevel()
    {
        if (ResetExam)
            Exam.examGenerated = false;

        SceneManager.LoadScene(LevelName);
    }

}
