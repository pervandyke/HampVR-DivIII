using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedEnemyHighlighter : MonoBehaviour
{
    private GameObject currentLeftTarget = null;
    private GameObject currentRightTarget = null;
    public GameObject leftHighlight;
    public GameObject rightHighlight;
    public LayerMask highlightMask; 


    private void Update()
    {
        if(Global.global.leftSelectedTarget != null && Global.global.leftSelectedTarget != currentLeftTarget)
        {
            currentLeftTarget = Global.global.leftSelectedTarget;
        }
        else if (Global.global.leftSelectedTarget == null)
        {
            currentLeftTarget = null;
        }
        if (Global.global.rightSelectedTarget != null && Global.global.rightSelectedTarget != currentRightTarget)
        {
            currentRightTarget = Global.global.rightSelectedTarget;
        }
        else if (Global.global.rightSelectedTarget == null)
        {
            currentRightTarget = null;
        }

        ProcessLeftHighlight();
        ProcessRightHighlight();
    }

    private void ProcessLeftHighlight()
    {
        if (currentLeftTarget != null)
        {
            if (!leftHighlight.activeInHierarchy)
            {
                leftHighlight.SetActive(true);
            }
            //move highlight to correct position
            //cast a ray from enemy to headset, if it hits the selection sphere add it to the list
            Vector3 direction = (gameObject.transform.position - currentLeftTarget.transform.position).normalized;
            RaycastHit hitData;
            Physics.Raycast(currentLeftTarget.transform.position, direction, out hitData, Vector3.Distance(currentLeftTarget.transform.position, 
                gameObject.transform.position), highlightMask, QueryTriggerInteraction.Collide);
            currentLeftTarget.transform.position = hitData.point;

            currentLeftTarget.transform.rotation.SetLookRotation(direction);
        }
        else
        {
            if (leftHighlight.activeInHierarchy)
            {
                leftHighlight.SetActive(false);
            }
        }
    }
    private void ProcessRightHighlight()
    {
        if (currentRightTarget != null)
        {
            if (!rightHighlight.activeInHierarchy)
            {
                rightHighlight.SetActive(true);
            }
            //move highlight to correct position
            //cast a ray from enemy to headset, if it hits the selection sphere add it to the list
            Vector3 direction = (gameObject.transform.position - currentRightTarget.transform.position).normalized;
            RaycastHit hitData;
            Physics.Raycast(currentRightTarget.transform.position, direction, out hitData, Vector3.Distance(currentRightTarget.transform.position,
                gameObject.transform.position), highlightMask, QueryTriggerInteraction.Collide);
            currentRightTarget.transform.position = hitData.point;

            currentRightTarget.transform.rotation.SetLookRotation(direction);
        }
        else
        {
            if (rightHighlight.activeInHierarchy)
            {
                rightHighlight.SetActive(false);
            }
        }
    }
}
