using System;
using System.Collections;
using System.Collections.Generic;
using Member.Player.ControlPlayer;
using UnityEngine;
using Random = UnityEngine.Random;
using Member.Player.DataPlayer;
using Mirror;

namespace Member.AI
{
    public class AIBehaviour : AvatarBehaviour
    {
        private static List<Transform> _movePoints = new List<Transform>();
        private Transform _designatedPoint;
        
        public Rigidbody2D rb;

        [SyncVar(hook = nameof(AiIndexChanged))]
        public int aiIndex;

        private bool _isWaiting = false;

        private void Awake()
        {
            SetDesignatedPoint();
        }

        private void Update()
        {
            Flip(rb.velocity.x);
            float characterVelocityX = Mathf.Abs(rb.velocity.x);
            float characterVelocityY = Mathf.Abs(rb.velocity.y);
            animator.SetBool("isWaiting", _isWaiting);
            
            if (canMove && !_isWaiting)
            {
                rb.transform.position = Vector3.MoveTowards(transform.position, _designatedPoint.position,
                    movementSpeed * Time.deltaTime);
                if (transform.position == _designatedPoint.position)
                {
                    StartCoroutine(Idle());
                }
            }
        }

        protected override void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            switch (newState)
            {
                case PlayerState.Dead:
                    Destroy(gameObject);
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

        public override void SelectRandomProfile()
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
        }

        private IEnumerator Idle()
        {
            _isWaiting = true;
            float randStop = Random.Range(4f, 10f);
            yield return new WaitForSeconds(randStop);
            SetDesignatedPoint();
            _isWaiting = false;
        }

        public override int GetAvatarIndex()
        {
            return aiIndex;
        }
    }
}