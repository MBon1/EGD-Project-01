using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayExamGrade : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text gradeText;

    // Start is called before the first frame update
    void Start()
    {
        float grade = Exam.Grade();
        char lettreGrade = Exam.GetLetterGrade();

        if (grade == float.NaN)
        {
            grade = 0f;
        }
        scoreText.text = "SCORE: " + grade.ToString("0.00");
        gradeText.text = "FINAL GRADE: " + lettreGrade;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
