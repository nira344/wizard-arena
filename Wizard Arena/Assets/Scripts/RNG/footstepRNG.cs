using UnityEngine;

public class footstepRNG : MonoBehaviour
{

    // The result of the current roll
    public int currentNumber;

    public int RollNumber()
    {
        // Random.Range with ints is exclusive on the upper bound
        currentNumber = Random.Range(1, 6);  // Rolls between 1 and 5
        Debug.Log("Rolled: " + currentNumber);
        return currentNumber;
    }
}
