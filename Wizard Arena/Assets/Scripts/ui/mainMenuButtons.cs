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
        Camera.main.gameObject.SetActive(true);
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
            case "Start":
                Camera.main.gameObject.SetActive(false);
                SceneManager.LoadScene("SampleScene");
                break;
            case "Info":
                if (!infoMenu) {break;}
                infoMenu.SetActive(true);
                break;
        }
    }

}
