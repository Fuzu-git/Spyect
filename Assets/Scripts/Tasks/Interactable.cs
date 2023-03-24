using System;
using System.Collections;
using UnityEngine;

namespace Tasks
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private GameObject miniGame;
        private GameObject _highlight;

        private bool _taskIsDone = false;
        private bool _taskIsInBounds = false;

        private Plane[] _planes;
        private Collider2D _objCollider;

        private void Awake()
        {
            _planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            _objCollider = gameObject.GetComponent<Collider2D>();
        }


        private IEnumerator Start()
        {
            while (!_taskIsDone /*&& _taskIsInBounds*/)
            {
                _highlight.SetActive(true);
                yield return new WaitForSeconds(1f);
                _highlight.SetActive(false);
                yield return new WaitForSeconds(1f);
            } 
            
            if (_taskIsDone || !_taskIsInBounds)
            {
                _highlight.SetActive(false); 
            }
        }

        /*private void Update()
        {
            if (GeometryUtility.TestPlanesAABB(_planes, _objCollider.bounds))
            {
                _taskIsInBounds = true;
            }
            else
            {
                _taskIsInBounds = false;
            }
        }*/

        private void OnEnable()
        {
            _highlight = transform.GetChild(0).gameObject;
        }

        public void PlayMiniGame()
        {
            miniGame.SetActive(true);
        }
    }
}