using System;
using Cinemachine;
using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;

namespace Member.Player.ControlPlayer
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private PlayerBehaviour playerBehaviour;
        [SerializeField] private NavMeshMovement navMeshMovement;

        private Quaternion constantRotation;

        public Camera mainCamera;

        private void Awake()
        {
            //mainCamera.transform.localEulerAngles = Vector3.zero;
            constantRotation = mainCamera.transform.rotation;
        }

        public override void OnStartAuthority()
        {
            enabled = true;
        }

        //[ClientCallback]
        private void Update()
        {
            if (PlayerBehaviour.local == playerBehaviour)
            {
                Debug.Log("COUCOU ");
                mainCamera.gameObject.SetActive(true);
                Debug.Log("OUI ");
                //mainCamera.transform.rotation = constantRotation;

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