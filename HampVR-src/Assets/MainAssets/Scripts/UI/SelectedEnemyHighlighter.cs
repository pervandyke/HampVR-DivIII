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
    public AnimationCurve highlightScalingCurve;


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

        ProcessHighlight(leftHighlight, currentLeftTarget);
        ProcessHighlight(rightHighlight, currentRightTarget);
    }

    private void ProcessHighlight(GameObject highlight, GameObject target)
    {
        if (target != null)
        {
            if (!highlight.activeInHierarchy)
            {
                highlight.SetActive(true);
            }

            //move highlight to correct position
            Vector3 direction = (gameObject.transform.position - target.transform.position).normalized;
            RaycastHit hitData;
            Physics.Raycast(target.transform.position, direction, out hitData, Vector3.Distance(target.transform.position,
                gameObject.transform.position), highlightMask, QueryTriggerInteraction.Collide);
            highlight.transform.position = hitData.point;

            //rotate highlight to face player
            highlight.transform.rotation = Quaternion.LookRotation((highlight.transform.position - target.transform.position).normalized);

            //scale highlight
            //float scaledSize = hitData.distance * 0.0001f;
            //highlight.transform.localScale = new Vector3();
        }
        else
        {
            if (highlight.activeInHierarchy)
            {
                highlight.SetActive(false);
            }
        }
    }
}
