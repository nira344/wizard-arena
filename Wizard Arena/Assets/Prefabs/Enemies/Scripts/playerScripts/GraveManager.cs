using UnityEngine;

public class GraveManager : MonoBehaviour
{
    public static GraveManager Instance;

    public GameObject gravePrefab;
    private GameObject currentGrave;
    private int unclaimedSouls = 0;

    void Awake()
    {
        Instance = this;
    }

    public void CreateGrave(Vector3 position, int soulAmount)
    {
        if (currentGrave != null)
        {
            Destroy(currentGrave);
            soulAmount = Mathf.FloorToInt(soulAmount / 2f);
            Debug.Log("Old grave destroyed. Half of souls carried to new grave.");
        }

        currentGrave = Instantiate(gravePrefab, position, Quaternion.identity);
        currentGrave.GetComponent<Grave>().Initialize(soulAmount);
        unclaimedSouls = soulAmount;
    }

    public void ClearGrave()
    {
        currentGrave = null;
        unclaimedSouls = 0;
    }
}
