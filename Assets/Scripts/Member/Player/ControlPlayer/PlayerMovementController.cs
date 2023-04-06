using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;

namespace Member.Player.ControlPlayer
{
    public class PlayerMovementController :  NetworkBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerBehaviour playerBehaviour;
        [SerializeField] public float movementSpeed = 5f;
        //[SerializeField] private Animator _animator;
        
        private Vector2 _previousInput; 

        private Controls _controls;
        private Controls Controls
        {
            get
            {
                if (_controls != null)
                {
                    return _controls; 
                }
                return _controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            Controls.Player.Move.canceled += ctx => ResetMovement();
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();

        //better on PlayerBehaviour ? 
        [ClientCallback]
        private void Update()
        {
            if (playerBehaviour.canMove)
            {
                Move(); 
            }
            
            //_animator.SetBool("IsMoving", Mathf.Approximately(_movementInput.magnitude,0));
        }
        
        
        [Client]
        private void SetMovement(Vector2 movement) => _previousInput = movement;

        [Client]
        private void ResetMovement() => _previousInput = Vector2.zero;

        
        [Client]
        private void Move()
        {
            Vector3 movement = transform.right.normalized * _previousInput.x + transform.up.normalized * _previousInput.y;

            controller.Move(movement * (movementSpeed * Time.deltaTime));
        }
    }
}
