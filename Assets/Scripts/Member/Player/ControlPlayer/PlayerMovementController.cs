using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;

namespace Member.Player.ControlPlayer
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private PlayerBehaviour playerBehaviour;
        [SerializeField] private NavMeshMovement navMeshMovement;

        public Camera mainCamera;

        public override void OnStartAuthority()
        {
            enabled = true;
        }

        private void Update()
        {
            if (PlayerBehaviour.local == playerBehaviour)
            {
                mainCamera.gameObject.SetActive(true);

                if (playerBehaviour.canMove)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
                        {
                            navMeshMovement.GoToDestination(hitInfo.point);
                        }
                    }
                }
            }
        }
    }
}