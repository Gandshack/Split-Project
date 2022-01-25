
/// <summary>
/// The interface for handling Health of entities.
/// </summary>
public interface IHealthComponent
{
    #region Properties
    /// <summary>
    /// The current health of the entity.
    /// </summary>
    int CurrentHealth
    {
        get;
        set;
    }

    /// <summary>
    /// The maximum health of the entity.
    /// Current Health is set to this in Awake unless 
    /// there is a special case not to.
    /// </summary>
    int MaximumHealth
    {
        get;
        set;
    }

    /// <summary>
    /// The percentile Health of the entity.
    /// Percentile health is updated when current Health is updated.
    /// </summary>
    float PercentileHealth
    {
        get;
    }
    #endregion

    #region Functions
    /// <summary>
    /// This function handles when the entity takes damage.
    /// This subtracts the passed in damageAmount from the
    /// CurrentHealth property.
    /// </summary>
    /// <param name="damageAmount">The amount of damage taken.</param>
    void OnDamage(int damageAmount);

    /// <summary>
    /// This function handles when the entity is healed.
    /// This adds the passed in healAmount to the
    /// CurrentHealth property.
    /// </summary>
    /// <param name="healAmount">The amount healed.</param>
    void Heal(int healAmount);

    /// <summary>
    /// Checks if Current Health is zero or less.
    /// </summary>
    /// <returns>Whether or not the current health is zero or less.</returns>
    bool IsDead();

    /// <summary>
    /// This function handles when the entity dies.
    /// This is called usually when CurrentHealth 
    /// is less than 0.
    /// </summary>
    void OnDeath();

    public float GetHealthFraction();
    #endregion
}
