using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] private Behaviour[] _disableOnDeath;
    private bool[] _wasEnabled;

    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    private bool _isDead = false;

    public bool IsDead { get { return _isDead; } protected set { _isDead = value; } }


    internal void Setup() {
        _wasEnabled = new bool[_disableOnDeath.Length];
        for (int i = 0; i < _wasEnabled.Length; i++) {
            _wasEnabled[i] = _disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    internal void SetDefaults() {
        _isDead = false;
        _currentHealth = _maxHealth;

        // Re-enable all behaviours
        for (int i = 0; i < _disableOnDeath.Length; i++) {
            _disableOnDeath[i].enabled = _wasEnabled[i];
        }

        // Enable collider
        Collider col = GetComponent<Collider>();
        if(col != null)
            col.enabled = true;
    }

    internal void RpcTakeDamage(float dmg) {
        if (_isDead)
            return;

        _currentHealth -= dmg;

        if (_currentHealth <= 0)
            Die();
        
    }

    private void Die() {
        _isDead = true;

        // Disable Components
        for (int i = 0; i < _disableOnDeath.Length; i++) {
            _disableOnDeath[i].enabled = false;
        }
        // Disable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Debug.Log(transform.name + " is DEAD!");

        // Call respawn method
        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn() {

        Debug.Log(transform.name + " respawned.");
        yield break;
    }
}
