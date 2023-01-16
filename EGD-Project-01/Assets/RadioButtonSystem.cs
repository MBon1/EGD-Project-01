using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RadioButtonSystem : MonoBehaviour
{
    ToggleGroup toggleGroup;

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
}
