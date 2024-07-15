using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{

    Vector3 offset;
    private string destinationTag = "DropArea";
    private string negativeTag = "RedZone";
    private float rotateSpeed;


    float zMoveSpeed = 3f;

    bool isSelected = false;

    Rigidbody rb;

    [HideInInspector]public bool isDraggedExternal = false;

    // Declare and initialize a new List of GameObjects called currentCollisions.
    List<GameObject> currentCollisions = new List<GameObject>();
    List<BlockSpawner> instantiatingUI = new List<BlockSpawner>();
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null) { 
            destinationTag = gm.destinationTag;
            negativeTag = gm.negativeTag;
            rotateSpeed = gm.BlockRotateSpeed;
            zMoveSpeed = gm.BlockZSpeed;
        }
        else
        {
            Debug.LogError("Game Manager not found in this scene");
        }
    }

    private void Update()
    {
        float inputZ = 0;
        if (Input.GetKey(KeyCode.W) && isSelected) inputZ = 1f;
        else if (Input.GetKey(KeyCode.S) && isSelected) inputZ = -1f;

        transform.position += new Vector3(0, 0, inputZ * Time.deltaTime * zMoveSpeed);

        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q) && isSelected) rotateDir += 1f;
        if (Input.GetKey(KeyCode.E) && isSelected) rotateDir -= 1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);

        if (isDraggedExternal)
        {
            //position to assigned
            transform.position = MouseWorldPosition() + offset;
        }

        if(Input.GetMouseButtonUp(0) && isDraggedExternal)
        {
            DragOff();
            isDraggedExternal = false;
        }
    }
    void OnMouseDown()
    {
        if (!this.enabled)
            return;

        DragOn();
    }
 
    void OnMouseDrag()
    {
        if (!this.enabled)
            return;

        Dragging();
    }
 
    void OnMouseUp()
    {
        if (!this.enabled)
            return;

        DragOff();
    }

    public void DraggedExternally(BlockSpawner UItoReset)
    {
        isDraggedExternal = true;
        isSelected = true;
        instantiatingUI.Add(UItoReset);
    }
    public void DragOn()
    {
        //calc the error
        offset = transform.position - MouseWorldPosition();

        isSelected = true;

        //Defaulting --->

        transform.GetComponent<Collider>().enabled = false;


        currentCollisions = new List<GameObject>();
        rb.useGravity = false;
    }
    public void Dragging()
    {
        //position to assigned
        transform.position = MouseWorldPosition() + offset;
    }
    public void DragOff()
    {
        isSelected = false;

        #region ColliderApproach

        //enable collider

        transform.GetComponent<Collider>().enabled = true;

        //activate gravity
        rb.useGravity = true;

        //when collider enter - if not green - reset

        #endregion

        foreach (BlockSpawner item in instantiatingUI)
        {
            //this will reset all the instantiating UI
            item.Reset();
        }
        instantiatingUI = new List<BlockSpawner>();
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        return worldPos;
    }

    void OnCollisionEnter(Collision col)
    {

        // Add the GameObject collided with to the list.
        currentCollisions.Add(col.gameObject);
    }

    void OnCollisionExit(Collision col)
    {

        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(col.gameObject);

    }
}
