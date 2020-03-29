using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private int energy;
    public int CurrentEnergy
    {
        get { return energy; }
    }
    [SerializeField] private int maxEnergy;
    public int MaxEnergy
    {
        get { return maxEnergy; }
    }

    private void Start()
    {
        GameManager.Instance.energyContainer.Add(gameObject,this);
    }
    
    public void AddEnergy(int bonusEnergy)
    {
        energy += bonusEnergy;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }       
    
    public bool CheckEnergySufficiency(int cost)
    {
        Debug.Log("energy");
        if (energy >= cost)
        {
            energy -= cost;
            return true;
        }

        return false;
    }    
    public void AddMaxEnergy(int bonusEnergy)
    {
        maxEnergy += bonusEnergy;
        energy += bonusEnergy;
    }    
    
    public void SetEnergy(int bonusEnergy)
    {
        maxEnergy = bonusEnergy;
        energy = bonusEnergy;
    }
}
