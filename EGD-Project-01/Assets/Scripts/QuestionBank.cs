using System.Linq;

using Rand = System.Random;

public class QuestionBank
{
    static Rand rnd = new Rand();

    private  Question[] questions = {
        // Multiple Choice
        new Question(QType.MultipleChoice, "The sea pangolin build an external layer of what over its shell?", new (string, bool)[] {
            ("Sodium Chloride", false),
            ("Calcium Sulfide", false),
            ("Iron Sulfide", true),
            ("Copper Phosphate", false),
        }),

        new Question(QType.MultipleChoice, "Bobbit-worms detect their pray through: ", new (string, bool)[] {
            ("Sight", false),
            ("Echolocation", false),
            ("Touch", true),
            ("Electroreception", false),
        }),

        new Question(QType.MultipleChoice, "The deepest oceanic trench on Earth is called the: ", new (string, bool)[] {
            ("Mariana Trench", true),
            ("Mariano Trench", false),
            ("Marianno Trench", false),
            ("Marianna Trench", false),
        }),

        new Question(QType.MultipleChoice, "The structure of coral reefs are made up of what?", new (string, bool)[] {
            ("Coral Jellies", false),
            ("Coral Polyps", true),
            ("Coral Buds", false),
            ("Coral Barnacles", false),
        }),

        new Question(QType.MultipleChoice, "Predatory tunicates are: ", new (string, bool)[] {
            ("Filter Feeders", false),
            ("Carrions", false),
            ("Photosynthetic", false),
            ("Ambush Predators", true),
        }),

        new Question(QType.MultipleChoice, "The majority of marine life is found in the: ", new (string, bool)[] {
            ("Epipelagic Zone", true),
            ("Mesopelagic Zone", false),
            ("Bathypelagic Zone", false),
            ("Hadal Zone", false),
        }),

        new Question(QType.MultipleChoice, "Which is a member of the order Hexanchiform?", new (string, bool)[] {
            ("Frilled Shark", true),
            ("Tiger Shark", false),
            ("Bottlenose Dolphin", false),
            ("Blue-ringed Octopus", false),
        }),

        new Question(QType.MultipleChoice, "Define anguilliform: ", new (string, bool)[] {
            ("Eel-like", true),
            ("Snake-like", false),
            ("Fish-like", false),
            ("Angular", false),
        }),

        // True-False
        new Question("T/F: The scientific name for the \"comb jelly\" phylum is \"Cnidaria\".", false),

        new Question("T/F: Ammonites were alive during the Devonian Period (420-359 MYA).", true),

        new Question("T/F: Eurypterids are commonly thought to be the ancestors to horseshoe crabs.", false),

        new Question("T/F: 500 atm is a potential pressure reading in the abyssal zone.", true),

        new Question("T/F: Temperatures around hydrothermal vents reach as high as 450?C.", false),

        new Question("T/F: The average depth of the ocean is around 3700m.", true),

        // Multiple Select
        new Question(QType.MultipleSelect, "Which of these are not examples of lobe-finned fish?", new (string, bool)[] {
            ("Coelacanth", false),
            ("Sturgeon", true),
            ("Lungfish", false),
            ("Carp", true),
        }),

        new Question(QType.MultipleSelect, "Which of these are related to sharks?", new (string, bool)[] {
            ("Skates", true),
            ("Rays", true),
            ("Chimaeras", true),
            ("Sawfish", true),
        }),

        new Question(QType.MultipleSelect, "Examples of Decapodiformes include: ", new (string, bool)[] {
            ("Heterololigo", true),
            ("Pencil Squid", true),
            ("Spirulida", true),
            ("Nautilus", false),
        }),

        new Question(QType.MultipleSelect, "Select all that are venomous: ", new (string, bool)[] {
            ("Lionfish", true),
            ("Stingray", true),
            ("Box Jellyfish", true),
            ("Barracuda", false),
        }),

        new Question(QType.MultipleSelect, "The following are Mid-Ocean ridges: ", new (string, bool)[] {
            ("Traveller Ridge", false),
            ("Mid-Atlantic Ridge", true),
            ("Peru Rise", false),
            ("East Pacific Rise", true),
        }),

        new Question(QType.MultipleSelect, "Crabs are: ", new (string, bool)[] {
            ("Vertebrates", false),
            ("Decapods", true),
            ("Arachnids", false),
            ("Crustaceans", true),
        }),
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
