using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TPCrowDialogueScript : MonoBehaviour
{
    public List<string> dialogueLines;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private bool inUse;
    private int currentLine = 0;
    private GameObject continueText;
    private bool dialogueStarted = false;

    void Start()
    {
        continueText = dialogueText.transform.GetChild(0).gameObject;
        continueText.SetActive(false);
        StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
        dialogueStarted = true;
    }

    void Update()
    {
        if (!dialogueStarted) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inUse)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                inUse = false;
                continueText.SetActive(true);
            }
            else if (currentLine < dialogueLines.Count - 1)
            {
                currentLine++;
                continueText.SetActive(false);
                StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
            }
            else
            {
                dialogueText.text = "";
                continueText.SetActive(false);
                dialogueStarted = false; // Stop further input
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            StopAllCoroutines();
            dialogueText.text = "";
            continueText.SetActive(false);
            inUse = false;
            currentLine = dialogueLines.Count;
        }
    }

    IEnumerator TypeDialogue(string line)
    {
        inUse = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        inUse = false;
        continueText.SetActive(true);
    }
}
