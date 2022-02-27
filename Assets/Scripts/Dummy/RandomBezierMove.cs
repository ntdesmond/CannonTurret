using System.Collections.Generic;
using UnityEngine;

namespace Dummy
{
    public class RandomBezierMove : MonoBehaviour
    {
        public float maxDistance;
        [Min(0)]
        public float segmentPassTime;

        private readonly List<QuadraticBezierCurve> _segments = new List<QuadraticBezierCurve>();
        private int _currentSegment;
        private float _currentTime;

        private Rigidbody _body;
        
        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            var initialPosition = transform.position;
            var withoutY = Vector3.one - Vector3.up;
            var midPosition = GetRandomPointNearby(initialPosition, withoutY);
            var endPosition = GetRandomPointNearby(initialPosition, withoutY);
            
            // Go Back and forth so that the path is looped
            _segments.Add(new QuadraticBezierCurve(
                initialPosition, midPosition, endPosition
            ));
            _segments.Add(new QuadraticBezierCurve(
                endPosition, midPosition, initialPosition
            ));
        }

        // Update is called once per frame
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= segmentPassTime)
            {
                _currentTime = 0;
                _currentSegment = (_currentSegment + 1) % _segments.Count;
            }
            var segment = _segments[_currentSegment];
            var velocity = segment.GetVelocity(_currentTime / segmentPassTime);
            _body.velocity = velocity;
            transform.forward = velocity.normalized;
        }

        private Vector3 GetRandomPointNearby(Vector3 origin, Vector3 mask)
        {
            return origin + Vector3.Scale(Random.insideUnitSphere, mask) * maxDistance;
        }
        
        private class QuadraticBezierCurve
        {
            private Vector3 P0 { get; }
            private Vector3 P1 { get; }
            private Vector3 P2 { get; }
            public QuadraticBezierCurve(Vector3 startPoint, Vector3 intermediatePoint, Vector3 endPoint)
            {
                P0 = startPoint;
                P1 = intermediatePoint;
                P2 = endPoint;
            }

            public Vector3 GetPosition(float t)
            {
                return P1 + Mathf.Pow(1 - t, 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);
            }

            public Vector3 GetVelocity(float t)
            {
                return 2 * (1 - t) * (P1 - P0) + 2 * t * (P2 - P1);
            }
        }
    }
}
