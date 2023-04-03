using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyAI : MonoBehaviour {

    protected GameManager gameManager;
    protected Player enemyPlayer;
    [SerializeField]
    protected float unitSpawnDelayTime;
    protected float unitSpawnTimer;


    public void Start() {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        enemyPlayer = GameObject.FindWithTag("EnemyPlayer").GetComponent<Player>();
    }
    protected abstract void SetNextUnit();
}
