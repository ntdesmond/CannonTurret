using Dummy;
using UnityEngine;

namespace CannonBall
{
    public class KillTarget : MonoBehaviour
    {
        public float removeAfterHitDistance;
        private Vector3 _hitPosition;
        private void OnCollisionEnter(Collision collision)
        {
            var target = collision.collider.GetComponentInParent<LiveTarget>();
            if (target == null || !target.IsAlive)
            {
                return;
            }

            target.IsAlive = false;
            _hitPosition = target.transform.position;
        }

        private void Update()
        {
            // Remove the projectile when it flew far away
            if ((transform.position - _hitPosition).magnitude >= removeAfterHitDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
