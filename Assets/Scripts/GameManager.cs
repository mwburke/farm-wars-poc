using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public List<Unit> allyUnitTypes;
    public List<Unit> enemyUnitTypes;

    public GameObject allyUnitsContainer;
    public GameObject enemyUnitsContainer;

    public GameObject spawnButtonPrefab;
    public GameObject spawnButtonContainer;


    public void Start() {
        CreateSpawnButtons();
    }


    public void SpawnUnit(Unit unit, bool isAlly) {
        unit.SetIsAlly(isAlly);

        GameObject unitContainer;
        if (isAlly) {
            unitContainer = allyUnitsContainer;
        } else {
            unitContainer = enemyUnitsContainer;
        }

        Instantiate(unit, unitContainer.transform);
    }

    public void CreateSpawnButtons() {
        foreach (Transform buttonTransform in spawnButtonContainer.transform) {
            Destroy(buttonTransform.gameObject);
        }

        foreach (Unit unit in allyUnitTypes) {
            CreateSpawnButton(unit);
        }
    }

    public void CreateSpawnButton(Unit unit) {
        // Create button and add it to container
        GameObject newButton = Instantiate(spawnButtonPrefab, spawnButtonContainer.transform);

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
    }
}
