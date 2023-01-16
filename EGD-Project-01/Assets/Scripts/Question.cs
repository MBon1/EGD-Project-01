using System;
using System.Linq;
using UnityEngine;

using Rand = System.Random;

/* Question Type */
public enum QType
{
    MultipleChoice,
    MultipleSelect,
    TrueFalse
}


public class Question
{
    QType questionType;
    public string question { get; private set; } = "";
    public (string, bool)[] answers { get; private set; } = { };

    static Rand rnd = new Rand();

    public QType QuestionType { get { return questionType; } }

    /* Create Question object. 
     * If questions have incorrect data, prints an error.
     * 
     *    Takes: QType, string, (string, bool)[]
     * Modifies: questionType, 
     *           questions
     *           answers
     *  Returns: new Question object
     *  Expects: Validates if Question is properly formatted (see ValidateQuestion)
     */
    public Question(QType _questionType, string _question, (string, bool)[] _answers)
    {
        questionType = _questionType;
        question = _question;
        answers = _answers;

        ValidateQuestion();
    }

    /* Create Question object for Multiple Choice Question
     * If questions have incorrect data, prints an error.
     * 
     *    Takes: string, bool
     * Modifies: questionType, 
     *           questions
     *           answers
     *  Returns: new Question object
     *  Expects: Validates if Question is properly formatted (see ValidateQuestion)
     */
    public Question(string _question, bool _answer)
    {
        questionType = QType.TrueFalse;
        question = _question;
        answers = new (string, bool)[] {
            ("True", _answer),
            ("False", !_answer)
        };

        ValidateQuestion();
    }

    /* Create a copy of a Question object
     * 
     *    Takes: string, bool
     * Modifies: questionType, 
     *           questions
     *           answers
     *  Returns: new Question object
     *  Expects: Source Question must be valid
     */
    public Question(Question _question)
    {
        questionType = _question.questionType;
        question = _question.question;
        answers = new (string, bool)[_question.answers.Length];
        Array.Copy(_question.answers, answers, _question.answers.Length);
    }

    /* Validates if Question is properly formatted.
     * 
     *    Takes: NONE
     * Modifies: NONE
     *  Returns: NONE
     *  Expects: Questions and Answer text cannot be null or blank
     *           Multiple Choice and Multiple Select must have at least 2 answers
     *           True-False must only have 2 answers
     *           Multiple Choice and True-False must have only 1 correct answer
     */
    void ValidateQuestion()
    {
        // Validate the question
        if (question == null || question == "")
        {
            Debug.LogError("ERROR: Invalid Question");
        }

        // Validate the number of answers
        if (answers.Length <= 1 ||
            (questionType == QType.MultipleChoice && answers.Length > 4) ||
            (questionType == QType.MultipleSelect && answers.Length > 4) ||
            (questionType == QType.TrueFalse && answers.Length != 2))
        {
            Debug.LogError("ERROR: Invalid Number of Answers");
        }

        // Validate the number of correct answers
        int numCorrectAnswers = 0;
        foreach ((string, bool) answer in answers)
        {
            if (answer.Item1 == null || answer.Item1 == "")
            {
                Debug.LogError("ERROR: Invalid Answer");
            }

            if (answer.Item2 == true)
            {
                numCorrectAnswers++;
            }

            if ((questionType == QType.MultipleChoice || questionType == QType.TrueFalse) &&
                numCorrectAnswers > 1)
            {
                Debug.LogError("ERROR: Invalid Number of Correct Answers");
            }
        }
    }

    /* Randomize the order of the question's answers.
     * 
     *    Takes: NONE
     * Modifies: answers
     *  Returns: NONE
     *  Expects: NONE
     */
    public void RandomizeAnswers()
    {
        if (questionType == QType.MultipleChoice ||
            questionType == QType.MultipleSelect)
        {
            answers = answers.OrderBy(x => rnd.Next()).ToArray();
        }
    }
}
