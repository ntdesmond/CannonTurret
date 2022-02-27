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
                    if (rb.GetComponent<RandomBezierMove>() == null)
                    {
                        rb.isKinematic = value;
                    }
                }

                if (_isAlive)
                {
                    return;
                }
                
                if (removeOnDeath)
                {
                    _removeTimer = removeDelay;
                }

                var randomMove = GetComponent<RandomBezierMove>();
                if (randomMove != null)
                {
                    randomMove.enabled = false;
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
