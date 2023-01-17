using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspicion : MonoBehaviour
{
    [SerializeField] Exam exam;
    bool viewingCheatSheet = false;
    float suspicion = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            exam.TabCheatSheet();
            viewingCheatSheet = false;
        }

    }
}
