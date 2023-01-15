using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam : MonoBehaviour
{
    private static QuestionBank questionBank = new QuestionBank();

    [SerializeField] int questionCount = 5;
    [SerializeField] bool capQuestionCount = false;


    // Start is called before the first frame update
    void Start()
    {
        // Display Questions and Answers
        GenerateExam();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* Randomizes the order of exam questions. 
     * Displays "questionCount" number of questions and their answers 
     * from the question bank. 
     * If the number of question in the exam is greater than the number 
     * of questions in the question bank and capQuestionCount is false, 
     * re-randomize the questions in the question bank again, output 
     * that many questions, and repeat until questionCount is reached.
     * 
     * 
     *    Takes: NONE
     * Modifies: questions
     *  Returns: NONE
     *  Expects: NONE
     */
    private void GenerateExam()
    {
        int displayedQuestionCount = 0;

        int numQuestionsInBank = questionBank.Questions.Length;

        while ((capQuestionCount && displayedQuestionCount != numQuestionsInBank) || 
            (!capQuestionCount && displayedQuestionCount < questionCount))
        {
            if (displayedQuestionCount % numQuestionsInBank == 0)
            {
                questionBank.RandomizeQuestions();
            }

            Question question = questionBank.Questions[displayedQuestionCount % numQuestionsInBank];
            Debug.Log("Q: " + question.question);
            for (int i = 0; i < question.answers.Length; i++)
            {
                (string, bool) answer = question.answers[i];
                char option = (char)('a' + i);
                Debug.Log("\t" + option + ". " + answer.Item1 + "   (" + (answer.Item2 ? "O" : "X") + ")");

            }

            displayedQuestionCount++;
        }
    }

}

