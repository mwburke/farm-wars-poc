using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSpawnButtonManager : MonoBehaviour
{
    public GameObject spawnButtonPrefab;
    public GameObject spawnButtonContainer;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        CreateSpawnButtons();
    }

    public void CreateSpawnButtons() {
        foreach (Transform buttonTransform in spawnButtonContainer.transform) {
            Destroy(buttonTransform.gameObject);
        }

        foreach (GameObject unit in gameManager.GetAllyUnitTypes()) {
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
