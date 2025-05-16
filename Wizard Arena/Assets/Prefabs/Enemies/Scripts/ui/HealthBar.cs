using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public float maxHealth;
	public float currentHealth;

	private RectTransform rt;

    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
	}

    public void SetMaxHealth(float health)
	{
		Debug.Log("max hp set");
		maxHealth = health;
		SetHealth(health);
	}

    public void SetHealth(float health)
	{
		currentHealth = health;
		if (rt) rt.localScale = new Vector3((5.0f * currentHealth / maxHealth), rt.localScale.y, rt.localScale.z);
	}
}
