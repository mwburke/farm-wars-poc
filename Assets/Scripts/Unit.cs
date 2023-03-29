using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType {
    SLASHING,
    PIERCING,
    FIRE,
    ARCHERY,
    MAGIC,
    NONE
}

public class Unit : MonoBehaviour
{
    public string unitName;
    [SerializeField]
    protected bool isAlly;
    [SerializeField]
    protected int maxShield;
    [SerializeField]
    protected int shield;
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int currentHealth;
    [SerializeField]
    protected int attackDamage;
    [SerializeField]
    protected float minAttackRange;
    [SerializeField]
    protected float maxAttackRange;
    [SerializeField]
    protected int energyCost;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected DamageType damageType;
    [SerializeField]
    protected DamageType resistanceType;
    [SerializeField]
    protected DamageType weaknessType;
    [SerializeField]
    protected Image healthBarImage;
    [SerializeField]
    protected Color buttonColor;
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected float projectileFlyTime;
    [SerializeField]
    protected bool isRanged;

    protected float attackTimer;
    protected GameManager gameManager;


    void Start()
    {
        currentHealth = maxHealth;
        shield = maxShield;

        if (!isAlly) {
            SpriteRenderer weapon = this.GetComponentInChildren<SpriteRenderer>();
            weapon.flipX = true;
        }

        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Unit[] opponentUnits;
        if (isAlly) {
            opponentUnits = gameManager.enemyUnitsContainer.GetComponentsInChildren<Unit>();
        } else {
            opponentUnits = gameManager.allyUnitsContainer.GetComponentsInChildren<Unit>();
        }

        Unit target = GetFirstOpponent(opponentUnits);
        if (target != null) {
            if (attackTimer > attackDelay) {
                // Attack target unit
                AttackUnit(target);
                // Reset timer
                attackTimer = 0f;
            }
        } else {
            Move();
        }

        attackTimer += Time.deltaTime;
    }

    private Unit GetFirstOpponent(Unit[] opponentUnits) {
        /* If there are any opponents, scan through them
         * and see if they are in attack range.
         * If they are, then keep track of the closest one.
         * Return null if none in range, otherwise return closes.
         */
        if (opponentUnits.Length == 0) {
            return null;
        }

        Unit firstOpponent = null;
        float minDist = 100f;

        foreach (Unit opponentUnit in opponentUnits) {
            float dist = Vector3.Distance(opponentUnit.transform.position, transform.position);
            if (IsDistanceInRange(dist)) {
                if (dist < minDist) {
                    minDist = dist;
                    firstOpponent = opponentUnit;
                }
            }
        }

        return firstOpponent;
    }

    private bool IsDistanceInRange(float dist) {
        return (dist >= minAttackRange) & (dist <= maxAttackRange);
    }

    private void Move() {
        // Move towards enemy
        Vector3 moveDirection = isAlly ? Vector3.right : Vector3.left;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    public int GetEnergyCost() {
        return energyCost;
    }

    public void SetIsAlly(bool isAlly) {
        this.isAlly = isAlly;
    }

    protected void UpdateHealthBar() {
        healthBarImage.fillAmount = NormalizedHealth();
    }

    protected float NormalizedHealth() {
        return (float)currentHealth / maxHealth;
    }

    protected Unit TryGetTarget(List<GameObject> enemyUnits) {
        float minDist = int.MaxValue;
        Unit targetUnit = null;

        foreach (GameObject enemyUnit in enemyUnits) {
            float distance = (this.transform.position - enemyUnit.transform.position).magnitude;
            if (distance < minDist & distance >= minAttackRange & distance <= maxAttackRange) {
                minDist = distance;
                targetUnit = enemyUnit.GetComponent<Unit>();
            }
        }

        return targetUnit;
    }

    protected void ProcessAttack(int damage, DamageType damageType) {
        // Adjust damage based on resistance or weakness
        int adjustedDamage = damage;
        if (damageType == resistanceType) {
            adjustedDamage = Mathf.RoundToInt(adjustedDamage / 2);
        } else if (damageType == weaknessType) {
            adjustedDamage *= 2;
        }

        // Take damage
        TakeDamage(adjustedDamage);
    }

    protected void TakeDamage(int damage) {
        // Apply damage
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Destroy(gameObject);
            // Add animation?
        } else {
            // Update health bar
            UpdateHealthBar();
        }
    }

    protected void AttackUnit(Unit targetUnit) {
        // Animation

        // Apply damage to target unit
        targetUnit.ProcessAttack(attackDamage, damageType);
    }
}
