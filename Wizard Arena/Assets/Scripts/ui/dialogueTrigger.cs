using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogueLine = "Hello, adventurer! Welcome to the land of magic!";
    private bool playerInRange = false;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public float typingSpeed = 0.05f;  // Adjust typing speed

    void Update()
    {
        // When the player presses 'F' while in range, start typing the dialogue
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            dialoguePanel.SetActive(true);
            StartCoroutine(TypeDialogue(dialogueLine));  // Start typing effect
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
            dialoguePanel.SetActive(false);
            StopAllCoroutines();  // Stop typing if the player leaves
            dialogueText.text = "";  // Clear the text
        }
    }

    // Coroutine for typing effect
    IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";  // Start with empty text
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;  // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before adding the next letter
        }
    }
}
