using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class and_dragObj : MonoBehaviour
{
    bool isSelected = false;
    Vector3 offset;
    [SerializeField]private float rotateSpeed;
    public float zMoveSpeed;

    List<BlockSpawner> instantiatingUI = new List<BlockSpawner>();
    Rigidbody rb;
    GameManager manager;
    TouchManager touchManager;
    private bool isDraggedExternal;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        manager = FindAnyObjectByType<GameManager>();
        touchManager = FindAnyObjectByType<TouchManager>();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            if (isDraggedExternal)
            {
                //position to assigned
                transform.position = MouseWorldPosition() + offset;
            }

            if (Input.touches[0].phase == TouchPhase.Ended && isDraggedExternal)
            {
                DragOff();
                isDraggedExternal = false;
            }
        }
    }
    public void Select(bool selection)
    {
        isSelected = selection;
        rb.isKinematic = selection;

        manager.SwitchPanel(this, selection);

    }
    public void RotateButtons(int rotateDir)
    {
        print("rotated " + rotateDir);

        touchManager.StartCoroutine(touchManager.SwitchallowedtoRaycastWithDelay(false));
        //Disa
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed, 0);
    }
    public void ForwardButtons(int moveDir)
    {
        touchManager.StartCoroutine(touchManager.SwitchallowedtoRaycastWithDelay(false));

        transform.position += new Vector3(0, 0, moveDir * zMoveSpeed);

    }
    private void OnMouseDown()
    {

        manager.TogglePanel(false);

        if (!isSelected)
            return;

        DragOn();
    }

    private void OnMouseDrag()
    {
        if (!isSelected)
            return;

        Dragging();
    }

    private void OnMouseUp()
    {
        manager.TogglePanel(true);


        if (!isSelected)
            return;

        DragOff();
    }
    public void DragOn()
    {
        //calc the error
        offset = transform.position - MouseWorldPosition();

        isSelected = true;

        //Defaulting --->

        transform.GetComponent<Collider>().enabled = false;


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

        manager.TogglePanel(true);

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
    public void DraggedExternally(BlockSpawner UItoReset)
    {
        isDraggedExternal = true;
        isSelected = true;
        instantiatingUI.Add(UItoReset);
    }
    public void InstantiationSetup()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        manager = FindAnyObjectByType<GameManager>();
        touchManager = FindAnyObjectByType<TouchManager>();
    }
    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        return worldPos;
    }
}
