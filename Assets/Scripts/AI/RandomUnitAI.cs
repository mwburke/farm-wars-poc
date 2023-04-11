using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUnitAI : EnemyAI {

    private List<GameObject> possibleUnits;
    private GameObject nextUnitPrefab;
    private int nextUnitEnergyCost;
    private int unitIndex;

    public new void Start() {
        base.Start();

        possibleUnits = gameManager.enemyUnitTypes;
        unitIndex = Random.Range(0, possibleUnits.Count);
        
        SetNextUnit();
        unitSpawnTimer = 0f;
    }

    public void Update() {
        if (nextUnitPrefab == null) {
            SetNextUnit();
        }

        if (enemyPlayer.GetCurrentEnergy() > nextUnitEnergyCost) {
            if (unitSpawnTimer >= unitSpawnDelayTime) {
                SpawnUnit();
            } else {
                unitSpawnTimer += Time.deltaTime;
            }
        } else {
            unitSpawnTimer += Time.deltaTime;
        }
    }

    protected override void SetNextUnit() {

        int index = 0;
        if (AIParams.enemyAiType == "random") {
            index = Random.Range(0, possibleUnits.Count);
        } else if (AIParams.enemyAiType == "ordered") {
            index = unitIndex % possibleUnits.Count;
            unitIndex += 1;
        }
        
        nextUnitPrefab = possibleUnits[index];

        Unit unit = nextUnitPrefab.GetComponent<Unit>();
        nextUnitEnergyCost = unit.GetEnergyCost();
    }

    protected void SpawnUnit() {
        gameManager.SpawnUnit(nextUnitPrefab, false);

        nextUnitPrefab = null;
        nextUnitEnergyCost = 1000;
        unitSpawnTimer = 0f;
    }
}
