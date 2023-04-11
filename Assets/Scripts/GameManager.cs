using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;

    public bool isGameActive = true;

    public Player player;
    public Player enemy;

    public List<GameObject> allyUnitTypes;
    public List<GameObject> enemyUnitTypes;

    public static explicit operator GameManager(GameObject v) {
        throw new NotImplementedException();
    }

    public GameObject allyUnitsContainer;
    public GameObject enemyUnitsContainer;

    public float minTimeSpawnSound;
    private float spawnSoundTimer;

    public AudioClip spawnSound;
    public float spawnSoundVolume;
    public AudioClip deathSound;
    public float deathSoundVolume;

    public GameObject gameOverScreen;
    public TMPro.TextMeshProUGUI gameOverText;

    public void Start() {
        // TODO: remove and set separately upon scene entry
        isGameActive = true;

        player.OnUnitDeath += GameManager_OnUnitDeath;
        enemy.OnUnitDeath += GameManager_OnUnitDeath;
    }

    public void Update() {
        AssignTargets();

        spawnSoundTimer += Time.deltaTime;
    }

    public bool IsGameActive() {
        return isGameActive;
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

        GameObject newUnit;
        GameObject unitContainer;

        if (isAlly) {
            unitContainer = allyUnitsContainer;
            newUnit = player.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
            if (newUnit != null) {
                newUnit.GetComponent<Unit>().OnUnitDeath += GameManager_OnUnitDeath;
            }

        } else {
            unitContainer = enemyUnitsContainer;
            newUnit = enemy.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
            if (newUnit != null) {
                newUnit.GetComponent<Unit>().OnUnitDeath += GameManager_OnUnitDeath;
            }
        }

        if (newUnit != null & spawnSoundTimer >= minTimeSpawnSound) {
            audioManager.PlayOneShotRandomPitch(spawnSound, spawnSoundVolume);
            spawnSoundTimer = 0f;
        }
    }

    private void GameManager_OnUnitDeath(Unit unit) {

        Debug.Log(unit);

        bool isAlly = unit.GetIsAlly();

        if (unit is Player) {
            Debug.Log("Game over event");
            HandleGameOver(!isAlly);
        } else {
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
        }

        audioManager.PlayOneShotRandomPitch(deathSound, deathSoundVolume);
    }

    private void HandleGameOver(bool playerWon) {
        if (playerWon) {
            gameOverText.SetText("You won!");
        } else {
            gameOverText.SetText("You Lost :(");
        }
        
        gameOverScreen.SetActive(true);
        isGameActive = false;
    }

    public void LoadBattleScene() {
        gameOverScreen.SetActive(false);
        isGameActive = true;
        SceneManager.LoadSceneAsync("Scenes/BattleScene");
    }

    public void LoadMainMenuScene() {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
}