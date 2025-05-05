using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenuButtons : MonoBehaviour
{
    public string buttonType = "";
    public GameObject infoMenu;
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
            case "Start": SceneManager.LoadScene("SampleScene"); break;
            case "Info":
                if (!infoMenu) {break;}
                infoMenu.SetActive(true);
                break;
        }
    }

}
