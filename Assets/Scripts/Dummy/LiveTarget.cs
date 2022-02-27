using UnityEngine;

namespace Dummy
{
    public class LiveTarget : MonoBehaviour
    {
        public bool isInitiallyAlive;
        public bool removeOnDeath;
    
        [Min(0)]
        public float removeDelay;
        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                _isAlive = value;
                foreach (var rb in GetComponentsInChildren<Rigidbody>())
                {
                    rb.isKinematic = value;
                }
            
                if (!_isAlive && removeOnDeath)
                {
                    _removeTimer = removeDelay;
                }
            }
        }
        private bool _isAlive;
        private float _removeTimer;
    
        private void Awake()
        {
            IsAlive = isInitiallyAlive;
        }

        private void Update()
        {
            if (_isAlive || !removeOnDeath)
            {
                return;
            }
            if (_removeTimer > 0)
            {
                _removeTimer -= Time.deltaTime;
                return;
            }
        
            Destroy(gameObject);
        }
    }
}
