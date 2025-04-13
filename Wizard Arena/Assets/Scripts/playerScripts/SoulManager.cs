using UnityEngine;
using TMPro;

public class SoulManager : MonoBehaviour
{
    public static SoulManager Instance;

    public int soulEssence = 0;
    public TextMeshProUGUI soulText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Optional: Keeps it between scenes
    }

    void Start()
    {
        soulEssence = PlayerPrefs.GetInt("SoulEssence", 0);
        UpdateSoulText();
    }

    public void AddSouls(int amount)
    {
        soulEssence += amount;
        UpdateSoulText();
    }

    public void SpendSouls(int amount)
    {
        soulEssence = Mathf.Max(0, soulEssence - amount);
        UpdateSoulText();
    }

    private void UpdateSoulText()
    {
        soulText.text = soulEssence.ToString(); // Fixed the error here
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SoulEssence", soulEssence);
    }
}
