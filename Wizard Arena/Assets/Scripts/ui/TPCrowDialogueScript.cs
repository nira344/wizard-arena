using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TPCrowDialogueScript : MonoBehaviour
{
    public List<string> dialogueLines;
    public TextMeshProUGUI dialogueText;

    public float typingSpeed = 0.05f;

    private bool inUse;
    private int currentLine = 0;

    void Start()
    {
        StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inUse)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                inUse = false;
                currentLine++;
            }
            else if (currentLine < dialogueLines.Count)
            {
                StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
            }
            else
            {
                dialogueText.text = "";
            }
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
        currentLine++;
    }
}
