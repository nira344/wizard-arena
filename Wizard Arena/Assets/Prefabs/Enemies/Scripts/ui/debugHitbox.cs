using UnityEngine;

public class debugHitbox : MonoBehaviour
{

    //public Color fillColor;
    BoxCollider2D[] boxes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BoxCollider2D[] boxes = GetComponents<BoxCollider2D>();
    }

    void OnDrawGizmos()
    {
        if (boxes != null)
        {
            //Debug.LogError("okay time for box");
            foreach (BoxCollider2D box in boxes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
            }
        }
    }
}
