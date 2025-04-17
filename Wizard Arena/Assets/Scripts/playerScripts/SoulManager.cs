using UnityEngine;
using TMPro;

public class SoulManager : MonoBehaviour
{
    public static SoulManager Instance;

    public int soulEssence = 0;
    public TextMeshProUGUI soulText;

    private const int MAX_SOULS = 999;

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

        // Clamp between 0 and max
        soulEssence = Mathf.Clamp(soulEssence, 0, MAX_SOULS);

        UpdateSoulText();
    }

    public void SpendSouls(int amount)
    {
        if (soulEssence >= amount)
        {
            soulEssence -= amount;
            UpdateSoulText();
        }
        else
        {
            Debug.Log("Not enough souls to spend.");
        }
    }

    public void UpdateSoulText()
    {
        if (soulText != null)
        {
            soulText.text = soulEssence.ToString();
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SoulEssence", soulEssence);
    }
}
