using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public List<string> dialogueLines;
    private bool playerInRange = false;

    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private bool isTyping = false;
    private int currentLine = 0;
    private GameObject continueText;
    private Coroutine typingCoroutine;

    void Start()
    {
        continueText = dialogueText.transform.GetChild(0).gameObject;
        continueText.SetActive(false);
        dialogueText.text = "";
    }

    void Update()
    {
        if (!playerInRange || dialogueLines.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isTyping)
            {
                // Skip typing and show full line
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
                continueText.SetActive(true);
            }
            else
            {
                currentLine++;

                if (currentLine >= dialogueLines.Count)
                {
                    CloseDialogue();
                }
                else
                {
                    continueText.SetActive(false);
                    typingCoroutine = StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (dialogueLines.Count > 0 && dialogueText.text == "")
            {
                typingCoroutine = StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
            }
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
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueText.text = "";
        continueText.SetActive(false);
        isTyping = false;
        currentLine = 0;
    }

    IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueText.SetActive(true);
    }
}
