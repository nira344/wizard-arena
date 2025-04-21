using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public List<string> dialogueLines;
    private bool playerInRange = false;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public float typingSpeed = 0.05f;  // Adjust typing speed

    private bool inUse;
    private int currentLine = 0;

    void Update()
    {
        // When the player presses 'F' while in range, start typing the dialogue
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
                dialoguePanel.SetActive(true);
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
                    StartCoroutine(TypeDialogue(dialogueLines[currentLine]));  // Start typing effect
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
        dialoguePanel.SetActive(false);
        StopAllCoroutines();  // Stop typing if the player leaves
        dialogueText.text = "";  // Clear the text
        inUse = false;
        currentLine = 0;
    }

    // Coroutine for typing effect
    IEnumerator TypeDialogue(string line)
    {
        inUse = true;
        dialogueText.text = "";  // Start with empty text
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;  // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before adding the next letter
        }
        inUse = false;
        currentLine++;
    }
}
