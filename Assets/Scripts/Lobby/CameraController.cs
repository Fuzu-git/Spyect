using Cinemachine;
using Mirror;
using UnityEngine;

namespace Lobby
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private Controls _controls;
        
        public override void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);
            
            enabled = true;
        }
    }
}