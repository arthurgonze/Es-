using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, ItemsInterface
{
    [SerializeField] GameObject itemStored;
    [SerializeField] Sprite chestOpened;
    bool isOpened;

    public void Interact(ref Player player)
    {
        if(!isOpened)
        {
            this.GetComponent<SpriteRenderer>().sprite = chestOpened;
            GameObject item = GameObject.Instantiate(itemStored, new Vector3(player.transform.position.x + 1f,
                player.transform.position.y,
                player.transform.position.z), Quaternion.identity);
            isOpened = true;
        }
        else
        {
            Debug.Log("Bau aberto");
        }
    }

    // Use this for initialization
    void Start()
    {
        isOpened = false;
    }
}
