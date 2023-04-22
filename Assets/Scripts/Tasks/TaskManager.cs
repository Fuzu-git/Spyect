using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Member;
using Member.Player;
using Member.Player.DataPlayer;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField]
        private int totalTask;
        [SerializeField] 
        private int taskToDo = 5; 
        [SerializeField]
        private int actualTask = 0;

        [SerializeField] private List<GameObject> totalTaskList = new List<GameObject>();
        [SerializeField] private List<GameObject> gameTaskList = new List<GameObject>();

        [SerializeField]
        private  Slider taskBar;

        public List<GameObject> pingList = new List<GameObject>();
        public GameObject pingObject;

        [SerializeField]
        private float pingInterval = 15;
        [SerializeField]
        private float pingLife = 2;

        [Header("Interactable")]
        public GameObject interactableMiniGame;
        public Button interactableUseButton;
        public GenericTask interactableGenericTask; 
        
        public IEnumerator Start()
        {
            totalTaskList.AddRange(GameObject.FindGameObjectsWithTag("Task"));
            taskBar.maxValue = taskToDo;
            
            AssignRandomTask();
            AddTaskComponent();
            
            yield return new WaitForSeconds(5f);

            GameObject playerCounter = GameObject.FindGameObjectWithTag("PlayerCounter");
            int playerNumber = playerCounter.GetComponent<PlayerNumberCounter>().playerNumber;
            for (int i = 0; i < playerNumber; i++)
            {
                pingList.Add(Instantiate(pingObject));
            }
        }

        private void AssignRandomTask()
        {
            for (int i = 0; i < taskToDo; i++)
            {
                int rng = Random.Range(0, totalTaskList.Count);
                gameTaskList.Add(totalTaskList[rng]);
                totalTaskList.RemoveAt(rng);

                var temp = new List<GameObject>();
                
                foreach (GameObject task in totalTaskList)
                {
                    if (task != null)
                    {
                        temp.Add(task);
                    }
                }
                totalTaskList.Clear();
                totalTaskList = temp;
            }
        }

        private void AddTaskComponent()
        {
            foreach (GameObject task in gameTaskList)
            {
                task.AddComponent<Interactable>();
                task.AddComponent<TaskData>();

                task.GetComponent<Interactable>().miniGame = interactableMiniGame;
                task.GetComponent<Interactable>().useButton = interactableUseButton;
                task.GetComponent<Interactable>().genericTask = interactableGenericTask;
                task.GetComponent<Interactable>().task = task.GetComponent<TaskData>();
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

