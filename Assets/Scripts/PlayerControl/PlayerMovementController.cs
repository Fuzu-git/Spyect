using Mirror;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private CharacterController controller; 
        
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

        [ClientCallback]
        private void Update()
        {
            if (PlayerBehaviour.canMove)
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
            Transform controllerTransform = controller.transform;
            Vector3 right = controllerTransform.right;
            Vector3 up = controllerTransform.up;
            right.z = 0f;
            up.z = 0f;

            Vector3 movement = right.normalized * _previousInput.x + up.normalized * _previousInput.y;

            controller.Move(movement * (movementSpeed * Time.deltaTime));
        }
    }
}
