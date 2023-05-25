using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Member.AI
{
    public class AIBehaviour : AvatarBehaviour
    {
        private static List<Transform> _movePoints = new List<Transform>();
        private Transform _designatedPoint;

        private Vector3 _lastPosition;
        private Transform _transform;
        bool isWalkingTowardTarget = false;

        [SyncVar(hook = nameof(AiIndexChanged))]
        public int aiIndex;

        [SerializeField] private NavMeshMovement navMeshMovement;

        [SerializeField] private bool _isWaiting = false;
        [SerializeField] private NavMeshAgent _meshAgent;

        private bool isIdle = false; 

        private void Awake()
        {
            SetDesignatedPoint(false);

            _transform = transform;
            _lastPosition = _transform.position;
        }

        private void Update()
        {
            Vector3 velocity = _transform.position - _lastPosition;
            if (canMove)
            {
                if (!isWalkingTowardTarget)
                {
                    isIdle = false; 
                    navMeshMovement.GoToDestination(_designatedPoint.position);
                    isWalkingTowardTarget = true;
                }

                if (navMeshMovement.arrived && !isIdle)
                {
                    StartCoroutine(Idle());
                }
                if (velocity.x != 0 || velocity.z != 0)
                {
                    Flip(velocity.x);

                    animator.SetBool("isWaiting", false);
                    
                }
                else
                {
                    animator.SetBool("isWaiting", true);
                }
            }
            _lastPosition = _transform.position;
        }

        public override void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Dead:
                    Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
                    foreach (var _renderer in renderers)
                    {
                        _renderer.enabled = false;
                    }

                    //joueur désigné mort. (lien vers UI)
                    // if VoteYes, playerVote are suspected.
                    break;
            }
        }

        public void SetAiIndex(int newValue)
        {
            aiIndex = newValue;
            StartCoroutine(WaitForProfiller());
        }

        public void AiIndexChanged(int oldValue, int newValue)
        {
            StartCoroutine(WaitForProfiller());
        }

        protected override void SelectRandomProfile()
        {
            if (aiIndex == -1 || ProfileFillerComponent == null) return;
            profileIndex = ProfileFillerComponent.GetIndex(aiIndex);
            SendProfilToClient(profileIndex);
        }

        public static void AddMovePoint(Transform transform) => _movePoints.Add(transform);
        public static void RemoveMovePoint(Transform transform) => _movePoints.Remove(transform);

        private void SetDesignatedPoint(bool change)
        {
            int randIndex = Random.Range(0, _movePoints.Count);
            while (_designatedPoint == _movePoints[randIndex])
            {
                randIndex = Random.Range(0, _movePoints.Count);
            }

            _designatedPoint = _movePoints[randIndex];
            if (change)
            {
                isWalkingTowardTarget = false;
            }
        }

        private IEnumerator Idle()
        {
            isIdle = true; 
            //_isWaiting = true;
            //UpdateisWatingState(true);
            float randStop = Random.Range(4f, 10f);
            yield return new WaitForSeconds(randStop);
            SetDesignatedPoint(true);
            //_isWaiting = false;
            //isWalkingTowardTarget = false;
            //UpdateisWatingState(false);
        }

        [ClientRpc]
        private void UpdateisWatingState(bool isWaiting)
        {
            _isWaiting = isWaiting;
        }

        public override int GetAvatarIndex()
        {
            return aiIndex;
        }
    }
}