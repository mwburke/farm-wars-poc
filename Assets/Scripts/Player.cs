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
    protected Image energyBarImage;

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
        if (energyBarImage != null) {
            UpdateEnergyBar();
        }
    }

    public Unit TrySpawnUnit(GameObject unitPrefab, GameObject parentObject, bool isAlly=true) {
        Unit unit = unitPrefab.GetComponent<Unit>();
        unit.SetIsAlly(isAlly);

        float energyCost = (float)unit.GetEnergyCost();

        if (energyCost <= currentEnergy) {
            // Update energy bar
            currentEnergy -= energyCost;

            if (energyBarImage != null) {
                UpdateEnergyBar();
            }

            // Spawn unit
            Instantiate(unitPrefab, parentObject.transform);

            return unit;
        }

        return null;
    }

    protected void UpdateEnergyBar() {
        energyBarImage.fillAmount = NormalizedEnergy();        
    }

    protected float NormalizedEnergy() {
        return (float)currentEnergy / maxEnergy;
    }

    public float GetCurrentEnergy() {
        return currentEnergy;
    }
}
