using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private float delta;
    private Text textField;
    private Player player;
    private float healthValue;
    private float currentHealth;

    private void Start()
    {
        player = GameManager.Instance.player;
        textField = transform.Find("Text").GetComponent<Text>();
        healthValue = player.Health.CurrentHealth / (float)player.Health.MaxHealth;
    }

    void Update()
    {
        currentHealth = player.Health.CurrentHealth / (float)player.Health.MaxHealth;
        if (currentHealth > healthValue)
            healthValue += delta;
        if (currentHealth < healthValue)
            healthValue -= delta;
        if (Mathf.Abs(currentHealth - healthValue) < delta)
            healthValue = currentHealth;
        health.fillAmount = healthValue;
        textField.text = player.Health.CurrentHealth + "/" + player.Health.MaxHealth;
    }
}
