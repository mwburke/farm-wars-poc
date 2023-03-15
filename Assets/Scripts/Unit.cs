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
    protected int minAttackRange;
    [SerializeField]
    protected int maxAttackRange;
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

    protected float attackTimer;


    void Start()
    {
        currentHealth = maxHealth;
        shield = maxShield;

        if (!isAlly) {
            SpriteRenderer weapon = this.GetComponentInChildren<SpriteRenderer>();
            weapon.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Testing for now
        Unit target = TryGetTarget(new List<GameObject>());

        if (target != null) {
            // Battle with targeted unit
            if (attackTimer > attackDelay) {
                // Attack target unit
                AttackUnit(target);
                // Reset timer
                attackTimer = 0f;
            }
        } else {
            // Move towards enemy
            Vector3 moveDirection = isAlly ? Vector3.right : Vector3.left;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }

        attackTimer += Time.deltaTime;
    }

    public int GetEnergyCost() {
        return energyCost;
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
            Destroy(this);
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
