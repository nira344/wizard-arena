using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private int maxHealth;
	private int currentHealth;

	private RectTransform rt;

    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
	}

    public void SetMaxHealth(int health)
	{
		maxHealth = health;
		SetHealth(health);
	}

    public void SetHealth(int health)
	{
		currentHealth = health;
		rt.localScale = new Vector3 ((5.0f * currentHealth / maxHealth), rt.localScale.y, rt.localScale.z);
	}

}
