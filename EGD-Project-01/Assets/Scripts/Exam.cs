using UnityEngine;


public class Exam : MonoBehaviour
{
    private static QuestionBank questionBank = new QuestionBank();
    private (Question, int[])[] examQuestions;

    [SerializeField] uint questionCount = 5;
    [SerializeField] bool capQuestionCount = false;

    public float grade { get; private set; } = 0.0f;

    private (char, float)[] gradingSystem = new (char, float)[]     // Letter Grade and their minimum point value
    {
        ('A', 0.9f),
        ('B', 0.8f),
        ('F', 0.0f)        
    };


    // Start is called before the first frame update
    void Start()
    {
        GenerateExam();
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
                examQuestions = new (Question, int[])[questionCount];
            }
            else
            {
                examQuestions = new (Question, int[])[questionBankSize];
            }
        }
        else
        {
            examQuestions = new (Question, int[])[questionCount];
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
            examQuestions[displayedQuestionCount] = (new Question(question), 
                question.QuestionType == QType.MultipleSelect ? new int[question.answers.Length] : new int[] { -1 });

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

    /* Given a question number, sets answer to the given answer number.
     * If answer is alread selected for a Multi Select question, the
     * answer is unselected.
     * 
     *    Takes: int, ints
     * Modifies: examQuestions
     *  Returns: NONE
     *  Expects: Question and Answer should be valid. Error returned if not.
     */
    public void AnswerQuestion(int _questionNumber, int _answer)
    {
        if (examQuestions.Length <= _questionNumber)
        {
            Debug.LogError("ERROR: Question requested is invalid");
            return;
        }

        (Question, int[]) q = examQuestions[_questionNumber];

        if (q.Item1.QuestionType == QType.MultipleSelect &&
            _answer < q.Item2.Length)
        {
            q.Item2[_answer] = (q.Item2[_answer] == 0) ? 1 : 0; 
        } 
        else if ((q.Item1.QuestionType == QType.MultipleChoice || q.Item1.QuestionType == QType.TrueFalse) && 
            _answer < q.Item1.answers.Length)
        {
            q.Item2[0] = _answer;
        }
        else
        {
            Debug.LogError("ERROR: Answer given is invalid");
        }
    }

    /* Grades the exam.
     * 
     *    Takes: NONE
     * Modifies: grade
     *  Returns: NONE
     *  Expects: NONE
     */
    public float Grade()
    {
        grade = 0.0f;

        for(int i = 0; i < examQuestions.Length; i++)
        {
            (Question, int[]) q = examQuestions[i];

            if (q.Item1.QuestionType == QType.MultipleSelect)
            {
                bool correctlyAnswered = true;
                for(int j = 0; j < q.Item2.Length; j++)
                {
                    if ((q.Item1.answers[j].Item2 ? 1 : 0) != q.Item2[j])
                    {
                        correctlyAnswered = false;
                    }
                }

                if (correctlyAnswered)
                {
                    grade += 1.0f;
                }
            }
            else
            {
                int answerIndex = q.Item2[0];
                if (answerIndex >= 0 && q.Item1.answers[answerIndex].Item2 == true)
                {
                    grade += 1.0f;
                }
            }
        }

        grade /= (float)examQuestions.Length;
        return grade;
    }

    /* Returns the current recorded grade as a letter grade.
     * 
     *    Takes: NONE
     * Modifies: NONE
     *  Returns: char
     *  Expects: NONE
     */
    public char GetLetterGrade()
    {
        for(int i = 0; i < gradingSystem.Length; i++)
        {
            if (grade >= gradingSystem[i].Item2)
            {
                return gradingSystem[i].Item1;
            }
        }
        return '0';
    }
}
