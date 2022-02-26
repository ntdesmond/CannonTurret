using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class SpawnTargets : MonoBehaviour
    {
        public List<Transform> targetPrefabs;
        public Transform parentObject;

        private UnityEngine.Camera _camera;
        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
        
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit);
        
            var targetPrefab = targetPrefabs[Random.Range(0, targetPrefabs.Count)];
            var target = Instantiate(targetPrefab, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0));
            target.parent = parentObject;
        }
    }
}
