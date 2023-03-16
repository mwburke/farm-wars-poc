using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{

    private GameObject unitPrefab;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void SetUnit(GameObject unitPrefab) {
        this.unitPrefab = unitPrefab;
    }

    public void SpawnUnit() {
        gameManager.SpawnUnit(unitPrefab);
    }

}
