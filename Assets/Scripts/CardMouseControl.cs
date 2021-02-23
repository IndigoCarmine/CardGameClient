using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Mouseでカードを移動させるためのClass
/// </summary>
public class CardMouseControl : MonoBehaviour, IDragHandler, IEndDragHandler
{
    GameObject HandField;
    GameObject SelectedField;
    Vector3 TargetPos;
    void Start()
    {
        HandField = GameObject.FindGameObjectWithTag("HandField");
        SelectedField = GameObject.FindGameObjectWithTag("SelectedField");
    }

    

    public void OnDrag(PointerEventData data)
	{

        TargetPos = data.position;
		TargetPos.z = 0;
		transform.position = TargetPos;
	}

    public void OnEndDrag(PointerEventData data)
    {
        if (IsOverlapping(HandField)) this.transform.SetParent(HandField.transform);
        if (IsOverlapping(SelectedField)) this.transform.SetParent(SelectedField.transform);
        
    }

    private bool IsOverlapping(GameObject gameObject)
    {
        return gameObject.transform.position.x - gameObject.GetComponent<RectTransform>().sizeDelta.x / 2 <= TargetPos.x && gameObject.transform.position.x + gameObject.GetComponent<RectTransform>().sizeDelta.x / 2 >= TargetPos.x
            && gameObject.transform.position.y - gameObject.GetComponent<RectTransform>().sizeDelta.y / 2 <= TargetPos.y && gameObject.transform.position.y + gameObject.GetComponent<RectTransform>().sizeDelta.y / 2 >= TargetPos.y;
    }



  
}