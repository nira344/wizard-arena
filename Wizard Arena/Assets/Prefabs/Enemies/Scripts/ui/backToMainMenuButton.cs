using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class backToMainMenuButton : MonoBehaviour
{
    public string buttonType = "";

    Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button reference not set in the inspector.");
        }
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case "BackToMenu": SceneManager.LoadScene("Main Menu"); return;
        }
    }
}
