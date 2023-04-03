using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public bool isGameActive;

    public Player player;
    public Player enemy;

    public List<GameObject> allyUnitTypes;
    public List<GameObject> enemyUnitTypes;

    public static explicit operator GameManager(GameObject v) {
        throw new NotImplementedException();
    }

    public GameObject allyUnitsContainer;
    public GameObject enemyUnitsContainer;


    // Dictionary mapping units that are targets to the units that are targeting them
    // Used to remove targets when units die
    // TODO: this is unused right now, need to see if I should optimize it
    public Dictionary<Unit, Unit> targetUnitMap;

    public void Start() {
        // TODO: remove and set separately upon scene entry
        isGameActive = true;
    }

    public void Update() {
        AssignTargets();
    }

    public void AssignTargets() {
        // Check for unassigned targets and reset when applicable

        List<Unit> allyUnits = new(allyUnitsContainer.GetComponentsInChildren<Unit>());
        List<Unit> enemyUnits = new(enemyUnitsContainer.GetComponentsInChildren<Unit>());

        AssignTargetsForTeam(allyUnits, enemyUnits);
        AssignTargetsForTeam(enemyUnits, allyUnits);
    }

    public List<GameObject> GetAllyUnitTypes() {
        return allyUnitTypes;
    }

    public void AssignTargetsForTeam(List<Unit> teamUnits, List<Unit> opponentUnits) {
        // Yes this is very slow. I'm aware
        foreach (Unit teamUnit in teamUnits) {
            // If has target, we can skip checking
            if (!teamUnit.HasTarget()) {
                Unit firstOpponent = null;
                float minDist = 100f;
                foreach (Unit opponentUnit in opponentUnits) {
                    float dist = Vector3.Distance(opponentUnit.GetComponent<Transform>().position,
                                                  teamUnit.GetComponent<Transform>().position);

                    if (teamUnit.IsDistanceInRange(dist)) {
                        if (dist < minDist) {
                            firstOpponent = opponentUnit;
                            minDist = dist;
                        }
                    }
                }
                if (firstOpponent != null) {
                    teamUnit.AssignTarget(firstOpponent);
                }
            }
        }
    }

    public void SpawnUnit(GameObject unitPrefab, bool isAlly=true) {

        Unit newUnit;
        GameObject unitContainer;

        if (isAlly) {
            unitContainer = allyUnitsContainer;
            newUnit = player.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
            if (newUnit != null) {
                newUnit.OnUnitDeath += GameManager_OnUnitDeath;
            }

        } else {
            unitContainer = enemyUnitsContainer;
            newUnit = enemy.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
            if (newUnit != null) {
                newUnit.OnUnitDeath += GameManager_OnUnitDeath;
            }
        }
    }

    private void GameManager_OnUnitDeath(Unit unit) {

        bool isAlly = unit.GetIsAlly();
        if (isAlly) {
            List<Unit> enemyUnits = new(enemyUnitsContainer.GetComponentsInChildren<Unit>());
            // Unset it as target
            foreach (Unit targetingUnit in enemyUnits) {
                if (targetingUnit.GetTarget() == unit) {
                    targetingUnit.RemoveTarget();
                }
            }


        } else {
            List<Unit> allyUnits = new(allyUnitsContainer.GetComponentsInChildren<Unit>());
            foreach (Unit targetingUnit in allyUnits) {
                if (targetingUnit.GetTarget() == unit) {
                    targetingUnit.RemoveTarget();
                }
            }
        }

        // remove listener
        unit.OnUnitDeath -= GameManager_OnUnitDeath;
    }
}