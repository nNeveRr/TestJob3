using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEnemyIndication : MonoBehaviour
{

    [SerializeField]
    RectTransform VisualPoint;

    [SerializeField]
    Transform myPosObject;

    [SerializeField]
    Color VisualColor = Color.white;

    void Awake()
    {
        VisualPoint.GetComponent<Image>().color = VisualColor;
        VisualPoint.transform.SetParent(GameVisualInfo.Instance.VisualsTargetingBorder,false);
    }

    void Update()
    {
        if(isVisible())
        {
            VisualPoint.gameObject.SetActive(false);
            return;
        }
        else
        {
            VisualPoint.gameObject.SetActive(true);

            RectTransform BorderObj = GameVisualInfo.Instance.VisualsTargetingBorder;
            Vector3 PlayerPos = MapController.GetWoldPlayerPosition();
            Vector3 Norm3Pos = (PlayerPos - myPosObject.position);
            Vector2 Norm2Pos = new Vector2(Norm3Pos.x, Norm3Pos.z).normalized;

            RaycastHit2D hit;

            hit = Physics2D.Raycast(BorderObj.transform.position, -Norm2Pos, Mathf.Infinity, 1<<8);
            if(hit.collider!=null)
            {
                VisualPoint.position =  hit.point;
            }

        }
    }
    void OnDestroy()
    {
        if (VisualPoint != null)
        {
            Destroy(VisualPoint.gameObject);
        }
    }


    bool isVisible()
    {
        return Vector3.Angle(myPosObject.position - Camera.main.transform.position, Camera.main.transform.forward) < 40f;
    }

}
