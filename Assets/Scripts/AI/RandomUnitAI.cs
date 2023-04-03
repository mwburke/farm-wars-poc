using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUnitAI : EnemyAI {

    private List<GameObject> possibleUnits;
    private GameObject nextUnitPrefab;
    private int nextUnitEnergyCost;

    public new void Start() {
        base.Start();

        possibleUnits = gameManager.enemyUnitTypes;
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

        int index = Random.Range(0, possibleUnits.Count);
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
