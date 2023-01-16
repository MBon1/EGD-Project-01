using UnityEngine;


public class Exam : MonoBehaviour
{
    private static QuestionBank questionBank = new QuestionBank();
    private (Question, int)[] examQuestions;

    [SerializeField] uint questionCount = 5;
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
     * If the number of questions in the exam is less than the number 
     * of questions in the question bank and capQuestionCount is true, 
     * only that many questions will be added to the exam.
     * If the number of questions in the exam is greater than the number 
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
        if (capQuestionCount)
        {
            int questionBankSize = questionBank.Questions.Length;
            if (questionCount < questionBankSize)
            {
                examQuestions = new (Question, int)[questionCount];
            }
            else
            {
                examQuestions = new (Question, int)[questionBankSize];
            }
        }
        else
        {
            examQuestions = new (Question, int)[questionCount];
        }
        

        int displayedQuestionCount = 0;

        int numQuestionsInBank = questionBank.Questions.Length;

        while (displayedQuestionCount < examQuestions.Length)
        {
            if (displayedQuestionCount % numQuestionsInBank == 0)
            {
                questionBank.RandomizeQuestions();
            }

            Question question = questionBank.Questions[displayedQuestionCount % numQuestionsInBank];
            examQuestions[displayedQuestionCount] = (new Question(question), -1);

            Question lastExamQuestion = examQuestions[displayedQuestionCount].Item1;
            Debug.Log("Q: " + lastExamQuestion.question);
            for (int i = 0; i < lastExamQuestion.answers.Length; i++)
            {
                (string, bool) answer = lastExamQuestion.answers[i];
                char option = (char)('a' + i);
                Debug.Log("\t" + option + ". " + answer.Item1 + "   (" + (answer.Item2 ? "O" : "X") + ")");

            }

            displayedQuestionCount++;
        }
    }

    public void AnswerQuestion(int _questionNumber, int _answer)
    {
        if (examQuestions.Length >= _questionNumber)
        {
            return;
        }

        (Question, int) q = examQuestions[_questionNumber];
        q.Item2 = 1;

        Debug.Log("Q: " + examQuestions[_questionNumber].Item2);
    }

    // Grade
}

