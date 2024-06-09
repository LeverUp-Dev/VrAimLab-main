using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRFPSKit
{
    /// <summary>
    /// Represents a physical bullet that has been fired
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public AudioSource hitSound;
        public ParticleSystem hitParticle;
        public GameObject tracerTail;
        [Space]
        public BallisticProfile ballisticProfile;
        public BulletType bulletType;

        public BulletShooter shooter;
        
        //NOTE events will only be called on server
        public event Action<Bullet> HitEvent;
        public event Action<Bullet, Damageable> HitDamageableEvent;

        private Rigidbody _rigidbody;

        public bool isBulletContatct = false;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            //Bullet apply randomSpreadAngle to rotation
            if(ballisticProfile.randomSpreadAngle != 0)
                transform.rotation *= Quaternion.Euler(
                    Random.Range(-ballisticProfile.randomSpreadAngle, ballisticProfile.randomSpreadAngle) , 
                    Random.Range(-ballisticProfile.randomSpreadAngle, ballisticProfile.randomSpreadAngle), 0);//TODO this doesnt work
            
            _rigidbody.AddForce(transform.forward * ballisticProfile.startVelocity, ForceMode.VelocityChange);
            _rigidbody.useGravity = false; //Use custom gravity solution
            
            tracerTail.SetActive(bulletType == BulletType.Tracer);
        }

        private void FixedUpdate()
        {
            //Custom gravity implementation
            _rigidbody.AddForce(Physics.gravity * ballisticProfile.gravityScale, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision other)
        {
            //Call Hit Event
            HitEvent?.Invoke(this);
            
            //Get damageable component in collider or in parents
            if (other.gameObject.GetComponentInParent<Damageable>() is Damageable damageable)
            {
                //Apply damage to damageable
                damageable.TakeDamage(ballisticProfile.baseDamage);
                
                //Call Hit Damageable Event
                HitDamageableEvent?.Invoke(this, damageable);
            }

            //Schedule destruction, let sound play out first
            transform.GetComponent<BoxCollider>().enabled = false;
            Invoke(nameof(DestroyBullet), 1);
            //Stop moving
            _rigidbody.isKinematic = true;

            //Play hit effects on clients
            RPC_PlayHitEffect();
        }

        private void RPC_PlayHitEffect()
        {
            hitSound.Play();
            hitParticle.Play();
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}