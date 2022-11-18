using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SkinManager : NetworkBehaviour
{
    public List<Sprite> skinList = new List<Sprite>();
    //private List<GameObject> botList = new List<GameObject>();
    private readonly List<GameObject> _memberList = new List<GameObject>();

    private Sprite _spriteComponent;
    private void Awake()
    {
        _memberList.AddRange(GameManager.playerList);
        //memberList.AddRange(botList);
    }

    private void Start()
    {
        {
            for (int i = 0; i < _memberList.Count; i++)
            {
                Sprite randomSkin = skinList[Random.Range(0, skinList.Count)];
                _spriteComponent = randomSkin;
                _memberList[i].gameObject.GetComponent<SpriteRenderer>().sprite = _spriteComponent;
                skinList.Remove(randomSkin);
            }
        }
    }
}