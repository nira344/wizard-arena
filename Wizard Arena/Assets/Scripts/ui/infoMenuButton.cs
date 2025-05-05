using UnityEngine;
using UnityEngine.UI;

public class infoMenuButton : MonoBehaviour
{
    public GameObject InfoMenu;
    private bool menuActivated;
    public string buttonType = "";

    private Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button component not found.");
        }
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case "Info":
                menuActivated = !menuActivated;
                InfoMenu.SetActive(menuActivated);
                break;
        }
    }
}
