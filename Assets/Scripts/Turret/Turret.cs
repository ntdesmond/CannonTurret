using Dummy;
using UnityEngine;

namespace Turret
{
    public class Turret : MonoBehaviour
    {
        public Transform ammoPrefab;

        [Min(0)]
        public float shotImpulse;
    
        [Min(0)]
        public float attackCooldown;
    
        [Range(1, 360)]
        public float fieldOfView;
        public LineRenderer fovLine;

        public Transform cannonObject;
        private float _oldFOV = -1;
        private float _cooldown;

        private void Awake()
        {
            SetFOVLines();
        }

        private void Update()
        {  
            SetFOVLines();
        
            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                return;
            }

            if (!Attack())
            {
                return;
            }
            _cooldown = attackCooldown;
        }

        private bool Attack()
        {
            foreach (var target in transform.parent.GetComponentsInChildren<LiveTarget>())
            {
                if (!target.IsAlive)
                {
                    continue;
                }
                
                // Find a collider to aim to the center of the target
                var targetCollider = target.GetComponentInChildren<Collider>();
                if (targetCollider == null)
                {
                    Debug.LogWarning("An object with LiveTarget component must also have a Collider in the transform tree, but none found.");
                    continue;
                }
                
                var cannonNormal = -cannonObject.right;
                var distance = targetCollider.bounds.center - cannonObject.position;
                var angle = Vector3.Angle(
                    Vector3.ProjectOnPlane(distance, Vector3.up),
                    Vector3.ProjectOnPlane(cannonNormal, Vector3.up)
                );
                if (angle > fieldOfView / 2)
                {
                    continue;
                }
            
                // Rotate the turret
                transform.right = Vector3.ProjectOnPlane(-distance, transform.up).normalized;
                cannonObject.localEulerAngles = -Vector3.forward * Vector3.Angle(distance, -transform.right);
            
                // Shoot
                var projectile = Instantiate(ammoPrefab, cannonObject.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().AddForce(distance.normalized * shotImpulse, ForceMode.Impulse);
                return true;
            }

            return false;
        }

        private void SetFOVLines()
        {
            // Update FOV line in the game if FOV has changed
            if (Mathf.Approximately(fieldOfView, _oldFOV))
            {
                return;
            }
            _oldFOV = fieldOfView;
            var fovRotation = new Vector3(0, fieldOfView / 2, 0);
            var fovNormal = Vector3.ProjectOnPlane(-cannonObject.right, Vector3.up);
            var newFovLinePositions = new[]
            {
                Quaternion.Euler(fovRotation) * fovNormal,
                Vector3.zero,
                Quaternion.Euler(-fovRotation) * fovNormal
            };
            fovLine.SetPositions(newFovLinePositions);
        }
    }
}
