using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private Behaviour[] _disableOnDeath;
    private bool[] _wasEnabled;

    [SerializeField] private float _maxHealth = 100f;
    private float m_Health;
    private bool _isDead = false;
    

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        SerializeState(stream, info);
    }

    private void SerializeState(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(m_Health);
        }
        else {
            float oldHealth = m_Health;
            m_Health = (float)stream.ReceiveNext();

            if (m_Health != oldHealth) {
                OnHealthChanged();
            }
        }
    }

    private void OnHealthChanged() {
        throw new NotImplementedException();
    }

    internal void Setup() {
        _wasEnabled = new bool[_disableOnDeath.Length];
        for (int i = 0; i < _wasEnabled.Length; i++) {
            _wasEnabled[i] = _disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    internal void SetDefaults() {
        _isDead = false;
        m_Health = _maxHealth;

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

        m_Health -= dmg;

        if (m_Health <= 0)
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

    public bool IsDead {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
}
