using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic health component implementing the IHealthComponent interface.
/// </summary>
public class HealthComponent : MonoBehaviour, IHealthComponent
{
    #region IHealthComponent Reference
    /// <summary>
    /// The reference to the Health Component cached.
    /// </summary>
    IHealthComponent thisHealthComponent;
    #endregion

    // Variables exposed to Unity.
    #region Unity Variables
    /// <summary>
    /// The maximum health value exposed to Unity.
    /// </summary>
    [SerializeField]
    private int MaximumHealth_Unity;
    #endregion

    // Backing variables for our properties.
    #region Private Backing Fields
    /// <summary>
    /// The current health value exposed to Unity.
    /// </summary>
    private int CurrentHealth_Backing;
    #endregion

    //Implementation of IHealthComponent.
    #region IHealthComponent Implementation
    /// <summary>
    /// The current health of the entity.
    /// </summary>
    int IHealthComponent.CurrentHealth 
    {
        get
        {
            return CurrentHealth_Backing;
        }
        set
        {
            CurrentHealth_Backing = value;
        }
    }

    /// <summary>
    /// The maximum health of the entity.
    /// Current Health is set to this in Awake unless 
    /// there is a special case not to.
    /// </summary>
    int IHealthComponent.MaximumHealth 
    {
        get
        {
            return MaximumHealth_Unity;
        }
        set
        {
            MaximumHealth_Unity = value;
        }
    }

    /// <summary>
    /// The percentile Health of the entity.
    /// Percentile health is updated when current Health is updated.
    /// </summary>
    float IHealthComponent.PercentileHealth
    {
        get
        {
            return (float)thisHealthComponent.CurrentHealth / thisHealthComponent.MaximumHealth;
        }
    }

    /// <summary>
    /// This function handles when the entity takes damage.
    /// This subtracts the passed in damageAmount from the
    /// CurrentHealth property.
    /// </summary>
    /// <param name="damageAmount">The amount of damage taken.</param>
    public void OnDamage(int damageAmount)
    {
        if (damageAmount < 0)
        {
            throw new ArgumentException("Damage amount cannot be negative.");
        }
        thisHealthComponent.CurrentHealth -= damageAmount;
        if (IsDead())
        {
            OnDeath();
        }
    }

    /// <summary>
    /// This function handles when the entity is healed.
    /// This adds the passed in healAmount to the
    /// CurrentHealth property.
    /// </summary>
    /// <param name="healAmount">The amount healed.</param>
    public void Heal(int healAmount)
    {
        if(healAmount < 0)
        {
            throw new ArgumentException("Heal amount cannot be negative.");
        }
        thisHealthComponent.CurrentHealth += healAmount;
    }

    /// <summary>
    /// Checks if Current Health is zero or less.
    /// </summary>
    /// <returns>Whether or not the current health is zero.</returns>
    public bool IsDead()
    {
        if(thisHealthComponent.CurrentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This function handles when the entity dies.
    /// This is called usually when CurrentHealth 
    /// is less than 0.
    /// </summary>
    public void OnDeath()
    {
        Debug.Log("Entity " + gameObject.name + " died.");
    }
    #endregion

    //ToString for debugging purposes.
    #region ToString
    /// <summary>
    /// Overrides the to string implementation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        //TODO: Implement this using String Builder.
        string toReturn = "\n" + gameObject.name + "'s Health: " + "\n";
        toReturn += "Health: " + CurrentHealth_Backing / MaximumHealth_Unity + "\n";
        toReturn += "Percentage: " + thisHealthComponent.PercentileHealth + "\n";
        toReturn += "\n";
        return toReturn;
    }
    #endregion

    //Functions from Unity such as Awake, Update, etc.
    #region Unity Functions
    
    /// <inheritdoc/>
    public void Awake()
    {
        thisHealthComponent = this;
        thisHealthComponent.CurrentHealth = thisHealthComponent.MaximumHealth;
    }

    #endregion
}
