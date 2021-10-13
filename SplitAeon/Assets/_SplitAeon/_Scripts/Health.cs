using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class Health : MonoBehaviour
{
    [Header("Health Data")]
    public float maxHealth;

    //[HideInInspector]
    public float health;

    [Header("Healing")]
    public bool enableHealing;

    public float timeBeforeHeal;
    public float healAmount;
    public float healTick;

    private float lastHitTimer;
    private float healTickTimer;

    [Header("On Hit")]
    public UnityEvent onHitEvents;

    [Header("On Death")]
    public bool destroyOnDeath;

    public UnityEvent onDeathEvents;

    /// <summary>
    /// Called when the object is hit.
    /// </summary>
    public void Hit()
    {
        onHitEvents.Invoke();

        lastHitTimer = 0;

        if (health <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Called when the object is hit.
    /// </summary>
    /// <param name="damage">The amount to damage the object by.</param>
    public void Hit(float damage)
    {
        Damage(damage);
        Hit();
    }

    /// <summary>
    /// Called when the objects health value reaches zero or less.
    /// </summary>
    void Death()
    {
        if (destroyOnDeath)
        {
            Destroy(this.gameObject);
        }
        else
        {
            onDeathEvents.Invoke();
        }
    }

    /// <summary>
    /// Damages the health of the current object.
    /// </summary>
    /// <param name="damage">The amount to damage the object by.</param>
    public void Damage(float damage)
    {
        health -= damage;
    }

    /// <summary>
    /// Heals the current object by an amount.
    /// </summary>
    /// <param name="heal">The amount to heal the object by.</param>
    public void Heal(float heal)
    {
        health += heal;
    }

    /// <summary>
    /// Logs the current health of this object
    /// </summary>
    public void LogHealth()
    {
        Debug.Log(health);
    }

    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {

        lastHitTimer += Time.deltaTime;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (enableHealing)
        {
            if (lastHitTimer > timeBeforeHeal)
            {
                if (health < maxHealth)
                {
                    healTickTimer += Time.deltaTime;

                    if (healTickTimer >= healTick)
                    {
                        healTickTimer = 0;
                        Heal(healAmount);
                    }

                }
            }
        }


    }

}
