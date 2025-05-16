using UnityEngine;
using UnityEngine.UI;

public class CloseInfoMenuButton : MonoBehaviour
{
    public GameObject InfoMenu;

    private Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();

        if (myButton != null && InfoMenu != null)
        {
            myButton.onClick.AddListener(CloseMenu);
        }
        else
        {
            Debug.LogError("Button or InfoMenu reference is missing.");
        }
    }

    void CloseMenu()
    {
        InfoMenu.SetActive(false);
    }
}
