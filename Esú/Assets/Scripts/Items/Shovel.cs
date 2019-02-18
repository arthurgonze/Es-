using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour, ItemsInterface
{
    public void Interact(ref Player player)
    {
        if(player.AddItemInInventory(this.gameObject))
        {
            Debug.Log("Item added to the inventory");
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
