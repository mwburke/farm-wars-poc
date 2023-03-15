using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{

    [SerializeField]
    protected float maxEnergy;
    protected float currentEnergy;
    [SerializeField]
    protected float energyGainRate;

    [SerializeField]
    protected Image EnergyBarImage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnergy < maxEnergy) {
            currentEnergy += energyGainRate * Time.deltaTime;
            
        }

        // Check if this is resource intensive
        UpdateEnergyBar();
    }

    private void TrySpawnUnit(Unit unit) {
        float energyCost = (float)unit.GetEnergyCost();

        if (energyCost <= currentEnergy) {
            currentEnergy -= energyCost;
        }

        // Update energy bar
        // TODO: figure out if this redundant since we update every frame
        UpdateEnergyBar();
    }

    protected void UpdateEnergyBar() {
        healthBarImage.fillAmount = NormalizedEnergy();
    }

    protected float NormalizedEnergy() {
        return (float)currentEnergy / maxEnergy;
    }
}
