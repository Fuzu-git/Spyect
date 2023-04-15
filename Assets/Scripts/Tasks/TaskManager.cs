using System;
using System.Collections;
using System.Collections.Generic;
using Member;
using Member.Player;
using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField]
        private int totalTask;
        [SerializeField]
        private int actualTask = 0; 
        
        [SerializeField]
        private  Slider taskBar;

        public List<GameObject> pingList = new List<GameObject>();
        public GameObject pingObject;

        [SerializeField]
        private float pingInterval = 15;
        [SerializeField]
        private float pingLife = 2; 
        
        

        public IEnumerator Start()
        {
            totalTask = GameObject.FindGameObjectsWithTag("Task").Length;
            taskBar.maxValue = totalTask;

            yield return new WaitForSeconds(5f);

            GameObject playerCounter = GameObject.FindGameObjectWithTag("PlayerCounter");
            int playerNumber = playerCounter.GetComponent<PlayerNumberCounter>().playerNumber;
            for (int i = 0; i < playerNumber; i++)
            {
                pingList.Add(Instantiate(pingObject));
            }
        }

        public void AddTask()
        {
            actualTask++;
            if (actualTask == totalTask)
            {
                StartCoroutine(TaskEnd());
            }
            taskBar.value = actualTask;
        }

        private IEnumerator TaskEnd()
        {
            Debug.Log("ASSEMBLE");
            while (true)
            {
                for (int i = 0; i < GameManager.instance.playerList.Count; i++)
                {
                    PlayerBehaviour currentPlayer =  GameManager.instance.playerList[i].GetComponent<PlayerBehaviour>();
                    if(currentPlayer == PlayerBehaviour.local)
                    {
                        continue; 
                    }
                    if (currentPlayer.State == PlayerState.Alive && PlayerBehaviour.local.State == PlayerState.Alive)
                    {
                        pingList[i].transform.position = currentPlayer.transform.position;
                        pingList[i].SetActive(true); 
                        Debug.Log("WEEEEEEEEEEEEEEEEEEHEEEEEEE");
                    }
                    else
                    {
                        pingList[i].SetActive(false); 
                    }
                }

                StartCoroutine(DesactivatePing());
                yield return new WaitForSeconds(pingInterval);
            }
        }

        private IEnumerator DesactivatePing()
        {
            yield return new WaitForSeconds(pingLife);
            foreach (var ping in pingList)
            {
                ping.SetActive(false); 
            }
        }
    }
}

