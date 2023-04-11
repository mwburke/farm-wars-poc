using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISelector : MonoBehaviour
{

    public GameObject randomButton;
    public GameObject orderedButton;

    public void SetAiTypeRandom() {
        AIParams.enemyAiType = "random";

        SetOutlineActivationStatus(randomButton, true);
        SetOutlineActivationStatus(orderedButton, false);

    }

    public void SetAiTypeOrdered() {
        AIParams.enemyAiType = "ordered";

        SetOutlineActivationStatus(randomButton, false);
        SetOutlineActivationStatus(orderedButton, true);
    }

    private void SetOutlineActivationStatus(GameObject obj, bool status) {
        Outline[] outlines = obj.GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines) {
            outline.enabled = status;
        }
    }
}
