using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftGround : MonoBehaviour, ItemsInterface
{
    [SerializeField] GameObject buriedItem;
    [SerializeField] Sprite duggedGround;
    [SerializeField] Sprite normalGround;
    [SerializeField] GameObject neededItem;
    [SerializeField] float raycastItemDistance;


    private RaycastHit2D objectAboveHitUP;
    private RaycastHit2D objectAboveHitDOWN;
    private RaycastHit2D objectAboveHitRIGHT;
    private RaycastHit2D objectAboveHitLEFT;
    private bool dugged;

    [SerializeField] LayerMask whatIsObject;

    public void Interact(ref Player player)
    {
        CheckRaycastHit();

        if (!objectAboveHitUP && !objectAboveHitDOWN && !objectAboveHitLEFT && !objectAboveHitRIGHT && !dugged)
        {
            if (player.IsItemInInventory(neededItem.tag))
            {
                Debug.Log("possui o item necessario");
                this.GetComponent<SpriteRenderer>().sprite = duggedGround;
                dugged = true;

                buriedItem.SetActive(true);
                buriedItem.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                Debug.Log("nao possui o item necessario");
            }
        }
        else
        {
            if(objectAboveHitUP)
            {
                Debug.Log("Objeto obstruindo o local: " + objectAboveHitUP.collider.tag); 
            }
            else if(objectAboveHitDOWN)
            {
                Debug.Log("Objeto obstruindo o local: " + objectAboveHitDOWN.collider.tag);
            }
            else if(objectAboveHitRIGHT)
            {
                Debug.Log("Objeto obstruindo o local: " + objectAboveHitRIGHT.collider.tag);
            }
            else if(objectAboveHitLEFT)
            {
                Debug.Log("Objeto obstruindo o local: " + objectAboveHitLEFT.collider.tag);
            }
        }
    }

    private void Start()
    {
        dugged = false;
    }

    private void CheckRaycastHit()
    {
        objectAboveHitUP = Physics2D.Raycast(transform.position, Vector2.up, raycastItemDistance, whatIsObject);
        
        objectAboveHitDOWN = Physics2D.Raycast(transform.position, Vector2.down, raycastItemDistance, whatIsObject);
        
        objectAboveHitRIGHT = Physics2D.Raycast(transform.position, Vector2.right, raycastItemDistance, whatIsObject);
        
        objectAboveHitLEFT = Physics2D.Raycast(transform.position, Vector2.left, raycastItemDistance, whatIsObject);
    }
}
