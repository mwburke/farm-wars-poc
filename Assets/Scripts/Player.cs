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
    }

    private void TrySpawnUnit(Unit unit) {
        float energyCost = (float)unit.GetEnergyCost();

        if (energyCost <= currentEnergy) {
            currentEnergy -= energyCost;
        }

        // Update energy bar
    }
}
