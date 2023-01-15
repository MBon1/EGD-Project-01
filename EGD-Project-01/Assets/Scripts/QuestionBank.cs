using System.Linq;

using Rand = System.Random;

public class QuestionBank
{
    static Rand rnd = new Rand();

    private  Question[] questions = {
        new Question(QType.MultipleChoice, "The sea pangolin build an external layer of what over its shell?", new (string, bool)[] {
            ("Sodium Chloride", false),
            ("Calcium Sulfide", false),
            ("Iron Sulfide", true),
            ("Copper Phosphate", false),
        }),

        new Question("True or False: The scientific name for the \"comb jelly\" Phylum is \"Cnidaria\". ", false)
    };

    public Question[] Questions { get { return questions; } }

    /* Randomizes the order of questions and their answers.
     * 
     *    Takes: NONE
     * Modifies: questions
     *  Returns: NONE
     *  Expects: NONE
     */
    public void RandomizeQuestions()
    {
        questions = questions.OrderBy(x => rnd.Next()).ToArray();

        foreach(Question question in Questions)
        {
            question.RandomizeAnswers();
        }
    }
}
