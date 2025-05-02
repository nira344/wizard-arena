using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenuButtons : MonoBehaviour
{
    public string buttonType = "";

    Button myButton; // Drag the Button from the hierarchy into this field in the Inspector

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
            case "Start": SceneManager.LoadScene("SampleScene"); return;
        }
    }

}
