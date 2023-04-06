using System;
using System.Collections;
using System.Collections.Generic;
using Member.Player.ControlPlayer;
using UnityEngine;
using Random = UnityEngine.Random;
using Member.Player.DataPlayer;
using Mirror;
using UnityEngine.AI;

namespace Member.AI
{
    public class AIBehaviour : AvatarBehaviour
    {
        private static List<Transform> _movePoints = new List<Transform>();
        private Transform _designatedPoint;

        private Vector3 _lastPosition;
        private Transform _transform; 
        

        [SyncVar(hook = nameof(AiIndexChanged))]
        public int aiIndex;
        
        [SerializeField] private bool _isWaiting = false;
        [SerializeField] private NavMeshAgent _meshAgent;

        private void Awake()
        {
            SetDesignatedPoint();

            _transform = transform;
            _lastPosition = _transform.position;
        }

        private void Update()
        {
            animator.SetBool("isWaiting", _isWaiting || !canMove);

            if (canMove && !_isWaiting && isServer)
            {

                //_transform.position = Vector3.MoveTowards(_transform.position, _designatedPoint.position, movementSpeed * Time.deltaTime);
                Flip(_transform.position.x - _lastPosition.x);
                if (_transform.position == _designatedPoint.position)
                {
                    StartCoroutine(Idle());
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


        private void SetDesignatedPoint()
        {
            int randIndex = Random.Range(0, _movePoints.Count);
            while (_designatedPoint == _movePoints[randIndex])
            {
                randIndex = Random.Range(0, _movePoints.Count);
            }

            _designatedPoint = _movePoints[randIndex];
            _meshAgent.SetDestination(_designatedPoint.position);
        }

        private IEnumerator Idle()
        {
            _isWaiting = true;
            UpdateisWatingState(true);
            float randStop = Random.Range(4f, 10f);
            yield return new WaitForSeconds(randStop);
            SetDesignatedPoint();
            _isWaiting = false;
            UpdateisWatingState(false);
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