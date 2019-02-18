using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleepy : MonoBehaviour
{
    float counter;
    bool isSleep;
    float randomTime;
    // Use this for initialization
    void Start()
    {
        counter = 0;
        isSleep = false;
        randomTime = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        
        if (counter >= randomTime)
        {
            Sleep();
        }
        //Debug.Log("Timer: " + counter);

        if(GetComponent<FOV>())
        {
            GetComponent<FOV>().SetDirection(new Vector2(0, -1));
        }
    }

    private void Sleep()
    {
        if(isSleep)
        {
            GetComponent<Animator>().SetBool("Sleep", true);
            GetComponent<FOV>().enabled = false;
            GetComponentInChildren<FOVMesh>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Animator>().SetBool("Sleep", false);
            GetComponent<FOV>().enabled = true;
            GetComponentInChildren<FOVMesh>().enabled = true;
            GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        isSleep = !isSleep;
        counter = 0;
    }
}
