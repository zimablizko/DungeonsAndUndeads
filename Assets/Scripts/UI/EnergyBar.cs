using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image energy;
    [SerializeField] private Player player;
    [SerializeField] private float delta;
    private float energyValue;
    private float currentEnergy;

    private void Start()
    {
        energyValue = player.Energy.CurrentEnergy / (float)player.Energy.MaxEnergy;
    }

    void Update()
    {
        currentEnergy = player.Energy.CurrentEnergy / (float)player.Energy.MaxEnergy;
        if (currentEnergy > energyValue)
            energyValue += delta;
        if (currentEnergy < energyValue)
            energyValue -= delta;
        if (Mathf.Abs(currentEnergy - energyValue) < delta)
            energyValue = currentEnergy;
        energy.fillAmount = energyValue;
    }
}
