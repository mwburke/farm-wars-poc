using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Player enemy;

    public List<GameObject> allyUnitTypes;
    public List<GameObject> enemyUnitTypes;

    public static explicit operator GameManager(GameObject v) {
        throw new NotImplementedException();
    }

    public GameObject allyUnitsContainer;
    public GameObject enemyUnitsContainer;

    public GameObject spawnButtonPrefab;
    public GameObject spawnButtonContainer;


    public void Start() {
        CreateSpawnButtons();
    }

    public void SpawnUnit(GameObject unitPrefab, bool isAlly=true) {

        GameObject unitContainer;
        if (isAlly) {
            unitContainer = allyUnitsContainer;
            player.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
        } else {
            unitContainer = enemyUnitsContainer;
            player.TrySpawnUnit(unitPrefab, unitContainer, isAlly);
        }
    }

    public void CreateSpawnButtons() {
        foreach (Transform buttonTransform in spawnButtonContainer.transform) {
            Destroy(buttonTransform.gameObject);
        }

        foreach (GameObject unit in allyUnitTypes) {
            CreateSpawnButton(unit);
        }
    }

    public void CreateSpawnButton(GameObject unitPrefab) {
        // Create button and add it to container
        GameObject newButton = Instantiate(spawnButtonPrefab, spawnButtonContainer.transform);
        Unit unit = unitPrefab.GetComponent<Unit>();

        // Set the sprite to be the unit's sprite
        Transform buttonSpriteObject = newButton.transform.Find("Unit Sprite");
        Image buttonSprite = buttonSpriteObject.GetComponent<Image>();
        SpriteRenderer unitSpriteRenderer = unit.GetComponentInChildren<SpriteRenderer>();
        buttonSprite.sprite = unitSpriteRenderer.sprite;

        // Set the button's unit name
        Transform buttonUnitName = newButton.transform.Find("Unit Name");
        TextMeshProUGUI buttonUnitText = buttonUnitName.GetComponent<TextMeshProUGUI>();
        buttonUnitText.SetText(unit.unitName);

        // Set the button's cost
        Transform buttonUnitCost = newButton.transform.Find("Cost Text");
        TextMeshProUGUI buttonUnitCostText = buttonUnitCost.GetComponent<TextMeshProUGUI>();
        int energyCost = unit.GetEnergyCost();
        buttonUnitCostText.SetText("Cost:\n" + energyCost.ToString());

        // Set unit value
        newButton.GetComponent<SpawnButton>().SetUnit(unitPrefab);
    }
}
