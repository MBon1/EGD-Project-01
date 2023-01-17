using UnityEngine;
using UnityEngine.UI;


public class Exam : MonoBehaviour
{
    private static QuestionBank questionBank = new QuestionBank();
    public (Question, int[])[] examQuestions { get; private set; } = { };
    public static bool examGenerated = false;                                  // Change this value on start scene
    static bool wasCaughtCheating = false;

    public GameObject cheatSheetCanvas { get; private set; } = null;

    [SerializeField] uint questionCount = 5;
    [SerializeField] bool capQuestionCount = false;

    [SerializeField] GameObject multipleChoicePrefab = null;
    [SerializeField] GameObject multipleSelectPrefab = null;
    [SerializeField] GameObject trueFalsePrefab = null;


    public float grade { get; private set; } = 0.0f;

    private static (char, float)[] gradingSystem = new (char, float)[]     // Letter Grade and their minimum point value
    {
        ('A', 0.8f),
        ('B', 0.5f),
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
     *  Expects: Question Prefabs != null
     */
    private void GenerateExam()
    {
        if (examGenerated)
        {
            return;
        }

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

        // Destroy all pre-existing children
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
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
            QType questionType = lastExamQuestion.QuestionType;

            Debug.Log("Q: " + lastExamQuestion.question);

            GameObject qPrefab = null;

            if (questionType == QType.MultipleChoice)
            {
                qPrefab = Object.Instantiate(multipleChoicePrefab, this.gameObject.transform);
            }
            else if (questionType == QType.MultipleSelect)
            {
                qPrefab = Object.Instantiate(multipleSelectPrefab, this.gameObject.transform);
            }
            else
            {
                qPrefab = Object.Instantiate(trueFalsePrefab, this.gameObject.transform);
            }

            // Set Question Variables
            qPrefab.GetComponentInChildren<Text>().text = question.question;
            QuestionUI qUI = qPrefab.GetComponent<QuestionUI>();
            qUI.exam = this;
            qUI.questionID = displayedQuestionCount;

            Text[] qanswers = qPrefab.transform.Find("Answers").transform.GetComponentsInChildren<Text>();

            // Create Question prefab
            for (int i = 0; i < lastExamQuestion.answers.Length; i++)
            {
                (string, bool) answer = lastExamQuestion.answers[i];
                char option = (char)('A' + i);
                string aText = option + ". " + answer.Item1;
                Debug.Log("\t" + aText + "   (" + (answer.Item2 ? "O" : "X") + ")");

                // Update the answer for the question prefab
                qanswers[i].text = aText;
            }

            displayedQuestionCount++;
        }

        examGenerated = true;

        wasCaughtCheating = false;

        // Make Cheat Sheet
        GenerateCheatSheet();
    }

    /* Create Cheat Sheet Canvas by copying Exam Canvas.
     * Sets answers in Cheat Sheet.
     * 
     *    Takes: NONE
     * Modifies: cheatSheetCanvas
     *  Returns: NONE
     *  Expects: # of questions = # of questions in UI
     */
    public void GenerateCheatSheet()
    {
        // Create copy of Exam Canvas
        cheatSheetCanvas = Object.Instantiate(this.gameObject.transform.parent).gameObject;

        // Get Exam Object (cheat sheet)
        GameObject cheatSheet = cheatSheetCanvas.transform.Find("Exam").gameObject;

        QuestionUI[] questions = cheatSheet.transform.GetComponentsInChildren<QuestionUI>();
        for (int i = 0; i < examQuestions.Length; i++)
        {
            Destroy(questions[i].gameObject);
        }

        for (int i = 0; i < examQuestions.Length; i++)
        {
            Question q = examQuestions[i].Item1;

            Toggle[] qanswers = questions[i + examQuestions.Length].gameObject.transform.Find("Answers").transform.GetComponentsInChildren<Toggle>();

            for (int j = 0; j < qanswers.Length; j++)
            {
                // Remove AnswerUI from toggle to not trigger AnswerQuestion()
                Toggle qanswer = qanswers[j];
                Destroy(qanswer.gameObject.GetComponent<AnswerUI>());

                if (q.answers[j].Item2)
                {
                    qanswer.isOn = true;
                }
            }
        }

        // Set the position of the cheat sheet
        Vector3 pos = this.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        cheatSheet.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(pos.x - 15, pos.y - 25, pos.z);

        // Change interactability and layer of canvas
        cheatSheetCanvas.GetComponent<CanvasGroup>().interactable = false;
        cheatSheetCanvas.GetComponent<Canvas>().sortingLayerName = "Hidden";
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

        Debug.Log("Answered Question " + _questionNumber + " with " + _answer);
    }

    /* Player was caught cheating on exam.
     * Will result in automatic 0 on grading.
     * 
     *    Takes: NONE
     * Modifies: wasCaughtCheating
     *  Returns: NONE
     *  Expects: NONE
     */
    public void GetCaughtCheating()
    {
        wasCaughtCheating = true;
        // Change scene?
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

        if (wasCaughtCheating)
        {
            return grade;
        }

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
        return GetLetterGrade(grade);
    }

    /* Returns the given float as a letter grade.
     * 
     *    Takes: float
     * Modifies: NONE
     *  Returns: char
     *  Expects: NONE
     */
    public char GetLetterGrade(float _grade)
    {
        for (int i = 0; i < gradingSystem.Length; i++)
        {
            if (_grade >= gradingSystem[i].Item2)
            {
                return gradingSystem[i].Item1;
            }
        }
        return '0';
    }
}
