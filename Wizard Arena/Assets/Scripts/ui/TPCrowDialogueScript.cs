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
    private GameObject continueText;

    void Start()
    {
        continueText = dialogueText.transform.GetChild(0).gameObject;
        continueText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (inUse)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                continueText.SetActive(true);
                inUse = false;
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
                    continueText.SetActive(false);
                    StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
                }
            }

            currentLine++;
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.C))
        {
            if (inUse)
            {
                CloseDialogue();
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
        continueText.SetActive(false);
        inUse = false;
        currentLine = 0;
    }

    IEnumerator TypeDialogue(string line)
    {
        inUse = true;
        dialogueText.text = "";
        continueText.SetActive(false);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        inUse = false;
        continueText.SetActive(true);
    }
}
