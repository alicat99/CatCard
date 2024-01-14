using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity
{
    public EntityType entityType { get; private set; }
    public UnityEvent onValueUpdate = new UnityEvent();
    private int _maxHealth;
    public int maxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            onValueUpdate.Invoke();
        }
    }
    private int _health;
    public int health
    {
        get => _health;
        set
        {
            _health = value;
            _health = Mathf.Clamp(_health, 0, maxHealth);
            onValueUpdate.Invoke();
        }
    }

    public Entity(int maxHealth = 20, EntityType entityType = EntityType.P)
    {
        this.entityType = entityType;
        this.maxHealth = maxHealth;
        health = maxHealth;
    }
}
