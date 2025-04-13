using UnityEngine;
using TMPro;

public class SoulManager : MonoBehaviour
{
    public static SoulManager Instance;

    public int soulEssence = 0;
    public TextMeshProUGUI soulText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it between scenes
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void UpdateSoulText()
    {
        soulText.text = soulEssence.ToString();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SoulEssence", soulEssence);
    }
}
