using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSpawner : MonoBehaviour, IDragHandler
{

    Vector3 offset;


    public GameObject _tobeSpawnedObject;
    bool isObjectInstantiated = false;
    Transform instantiatedBlock;
    public void InstantiateGameObject()
    {
        if (isObjectInstantiated) return;

        isObjectInstantiated = true;
        instantiatedBlock = Instantiate(_tobeSpawnedObject).transform;
        instantiatedBlock.position = MouseWorldPosition() + offset;

        instantiatedBlock.GetComponent<and_dragObj>().InstantiationSetup();
        instantiatedBlock.GetComponent<and_dragObj>().Select(true);

    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.y = transform.position.y;
        return worldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        InstantiateGameObject();
        //instantiatedBlock.GetComponent<DraggableObject>().DraggedExternally(this);
        instantiatedBlock.GetComponent<and_dragObj>().DraggedExternally(this);
    }
    public void Reset()
    {
        isObjectInstantiated = false; instantiatedBlock = null;

    }
}
