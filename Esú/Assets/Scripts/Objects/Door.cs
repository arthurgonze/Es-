using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,ItemsInterface
{
    [SerializeField] Sprite openedDoor;
    [SerializeField] GameObject neededItem;

    private bool opened;

    public void Interact(ref Player player)
    {

        Debug.Log("tentando abrir porta");
        if (player.IsItemInInventory(neededItem.tag) && !opened)
        {
            Debug.Log("possui o item necessario");
            this.GetComponent<SpriteRenderer>().sprite = openedDoor;
            opened = true;
            this.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Debug.Log("nao possui o item necessario");
        }
    }
}
