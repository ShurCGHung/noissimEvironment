﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IEntityDamageEvent
{
    #region Protected Fields
    [SerializeField]
    protected BulletStats bulletStat;
    protected new Rigidbody2D rigidbody2D;
    protected bool isCrit;
    protected GameObject selfExplosion = null;
    #endregion

    #region Private Fields

    #endregion

    #region Monobehaviour Methods
    protected virtual void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitEvent(collision);
    }
    #endregion

    #region Protected Methods
    // Event trigger when hit something may be wall or obstacles or entity,....
    protected virtual void OnHitEvent(Collider2D collision)
    {
        if (bulletStat.exceptionTag.Contains(collision.tag))
            return;
        var entity = collision.GetComponent<Entity>();
        entity?.OnTakeDamage(this);
        isCrit = false;
        if (bulletStat.pfExplosion)
        {
            if (!selfExplosion)
                selfExplosion = Instantiate(bulletStat.pfExplosion.gameObject);
            selfExplosion.transform.position = transform.position;
            selfExplosion.transform.rotation = transform.rotation;
            selfExplosion.SetActive(true);
            TimeManipulator.GetInstance().InvokeActionAfterSeconds(1f, () => selfExplosion.SetActive(false));
            ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Public Methods
    // Function apply projectile force for bullet
    public virtual void Setup(Vector3 shootDirection, bool isCrit = false)
    {
        if (!rigidbody2D)
            rigidbody2D = GetComponent<Rigidbody2D>();
        this.isCrit = isCrit;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;
        rigidbody2D.AddForce(shootDirection * bulletStat.projectileSpeed, ForceMode2D.Impulse);
    }
    // Function calculate damage function
    public float GetDamage(ref bool isCrit)
    {
        // Pseudo Crit & Damage System Demo
        isCrit = this.isCrit;
        return isCrit == true ? bulletStat.damage * 2f : bulletStat.damage;
    }
    public ObjectPoolCode GetBulletCode()
    {
        return bulletStat.bulletCode;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    #endregion
}
