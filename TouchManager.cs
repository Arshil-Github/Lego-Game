using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    // Update is called once per frame

    Ray testRay;
    public bool allowedToRaycast = true;

    public and_dragObj currentSelected;

    bool allowedToTouch = true;
    void LateUpdate()
    {

        TouchSelectionHandlar();
    }
    public void TouchSelectionHandlar()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && allowedToTouch && allowedToRaycast)
        {
            StartCoroutine(CastRayForTouch());
        }
    }

    IEnumerator CastRayForTouch()
    {
        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;

        allowedToTouch = false;

        yield return new WaitForSeconds(0.3f);

        if (Physics.Raycast(raycast, out raycastHit) && allowedToRaycast)
        {
            if (raycastHit.collider.CompareTag("Block"))
            {
                if(currentSelected == null || currentSelected == raycastHit.collider.GetComponent<and_dragObj>())
                {
                    currentSelected = raycastHit.collider.GetComponent<and_dragObj>();

                    raycastHit.collider.GetComponent<and_dragObj>().Select(true);
                    Debug.Log("ro");
                }
            }
            else
            {
                Debug.Log("yo " + raycastHit.collider.name);

                foreach (and_dragObj item in FindObjectsOfType<and_dragObj>())
                {
                    item.Select(false);
                    FindAnyObjectByType<GameManager>().TogglePanel(false);
                }
                currentSelected = null;
            }
        }
        allowedToTouch = true;
    }
    public IEnumerator SwitchallowedtoRaycastWithDelay(bool switchTo)
    {
        allowedToRaycast = switchTo;

        yield return new WaitForSeconds(0.6f);

        allowedToRaycast = !switchTo;
    }
    private void OnDrawGizmos()
    {
    }
}
