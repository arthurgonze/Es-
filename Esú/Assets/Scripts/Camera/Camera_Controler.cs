using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controler : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform rightBound;
    [SerializeField] Transform leftBound;
    [SerializeField] Transform upBound;
    [SerializeField] Transform downBound;

    private float camWidth, camHeight, levelMinX, levelMaxX, levelMinY, levelMaxY;

    private Vector3 smoothDampVelocity = Vector3.zero;
    private float smoothDampTime = 0;

    private Game_Controller game_Controller;

    // Use this for initialization
    void Start()
    {
        //transform.position =  new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        game_Controller = FindObjectOfType<Game_Controller>();

        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundWidth = leftBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        float rightBoundWidth = rightBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;

        float upBoundHeight = upBound.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
        float downBoundHeight = downBound.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;

        levelMinX = leftBound.position.x + leftBoundWidth + (camWidth / 2);
        levelMaxX = rightBound.position.x - rightBoundWidth - (camWidth / 2);

        levelMinY = downBound.position.y + downBoundHeight + (camHeight / 2);
        levelMaxY = upBound.position.y - upBoundHeight - (camHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));
            float targetY = Mathf.Max(levelMinY, Mathf.Min(levelMaxY, target.position.y));

            //Vector3 vector = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
            //transform.position = new Vector3(vector.x, vector.y, transform.position.z);
            
            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);
            float y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);

            transform.position = new Vector3(x,y,transform.position.z);
        }
    }
}
