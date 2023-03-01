using System;
using System.Collections;
using System.Collections.Generic;
using Member.Player.ControlPlayer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Member.AI
{
    public class AIBehaviour : AvatarBehaviour
    {
        private float _actualAIMoveSpeed;
        private static List<Transform> _movePoints = new List<Transform>();
        private Transform _designatedPoint;
        public float epsilon;

        private void Awake()
        {
            _actualAIMoveSpeed = movementSpeed;
            SetDesignatedPoint();
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

        public static void AddMovePoint(Transform transform) => _movePoints.Add(transform);
        public static void RemoveMovePoint(Transform transform) => _movePoints.Remove(transform);

        public void Update()
        {
            if (canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, _designatedPoint.position,
                    _actualAIMoveSpeed * Time.deltaTime);

                if (Mathf.Approximately(transform.position.x, _designatedPoint.position.x / epsilon) &&
                    Mathf.Approximately(transform.position.y, _designatedPoint.position.y / epsilon) &&
                    Mathf.Approximately(transform.position.z, _designatedPoint.position.z / epsilon))
                {
                    _actualAIMoveSpeed = 0; 
                    SetDesignatedPoint();
                    Idle();
                    _actualAIMoveSpeed = movementSpeed; 
                }
            }
        }

        private void SetDesignatedPoint()
        {
            int randIndex = Random.Range(0, _movePoints.Count);
            while (_designatedPoint == _movePoints[randIndex])
            {
                randIndex = Random.Range(0, _movePoints.Count);
            }

            _designatedPoint = _movePoints[randIndex];
        }

        private void Idle()
        {
            float timer = 0;
            int randStop = Random.Range(4, 10);
            
            while (timer < randStop)
            {
                timer += Time.deltaTime;
            }
        }
    }
}