using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public List<string> dialogueLines;
    private bool playerInRange = false;

    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private bool inUse;
    private int currentLine = 0;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (inUse)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                inUse = false;
                currentLine++;
            }
            else
            {
                if (currentLine >= dialogueLines.Count)
                {
                    if (currentLine == 0)
                    {
                        Debug.LogError(gameObject.name + " AIN'T GOT NO DIALOGUE IDIOT");
                    }
                    else
                    {
                        CloseDialogue();
                    }
                }
                else
                {
                    StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        inUse = false;
        currentLine = 0;
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
