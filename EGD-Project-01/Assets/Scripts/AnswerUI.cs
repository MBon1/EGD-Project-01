using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AnswerUI : MonoBehaviour
{
    public QuestionUI question = null;
    public int answerID = -1;
    private bool inSetUp = true;

    Toggle toggle = null;

    private void Start()
    {
        // Set all toggle values to false
        toggle = this.gameObject.GetComponent<Toggle>();
        toggle.isOn = false;
        inSetUp = false;
    }

    public void Answer()
    {
        if (inSetUp)
        {
            return;
        }

        if (question == null)
        {
            Debug.LogError("ERROR: Question null");
            return;
        }

        /*Exam exam = question.exam;
        if (question.exam == null)
        {
            Debug.LogError("ERROR: Exam null");
            return;
        }*/

        int questionID = question.questionID;
        if (questionID < 0)
        {
            Debug.LogError("ERROR: Invalid Question ID");
            return;
        }

        if (answerID < 0)
        {
            Debug.LogError("ERROR: Invalid Answer ID");
            return;
        }

        Exam.AnswerQuestion(question.questionID, answerID);
        /*QType questionType = Exam.examQuestions[questionID].Item1.QuestionType;
        if (((questionType == QType.MultipleChoice || questionType == QType.TrueFalse) && toggle.isOn == true) ||
            questionType == QType.MultipleSelect)
        {
            //question.exam.AnswerQuestion(question.questionID, answerID);
            Exam.AnswerQuestion(question.questionID, answerID);
        }*/
    }
}
