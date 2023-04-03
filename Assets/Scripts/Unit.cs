using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    protected Unit target;

    public event OnDeathDelegate OnUnitDeath;
    public delegate void OnDeathDelegate(Unit unit);

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

    public bool HasTarget() {
        return target != null;
    }

    public void AssignTarget(Unit target) {
        this.target = target;
    }

    public void RemoveTarget() {
        target = null;
    }

    public Unit GetTarget() {
        return target;
    }

    public bool IsDistanceInRange(float dist) {
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

    public bool GetIsAlly() {
        return isAlly;
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
            OnUnitDeath?.Invoke(this);
            Destroy(gameObject);
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
