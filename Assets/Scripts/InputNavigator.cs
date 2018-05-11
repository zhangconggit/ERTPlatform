using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputNavigator : MonoBehaviour{

    EventSystem system;

    List<Selectable> enabledAllSelectable = new List<Selectable>();
    public int index = 0;

	// Use this for initialization
	void Start () {
        system = EventSystem.current;
        index = 0;
        foreach (Selectable selectableUI in Selectable.allSelectables)
        {
            if (selectableUI.gameObject.gameObject.activeSelf == true)
                enabledAllSelectable.Add(selectableUI);
        }
        if (enabledAllSelectable.Count > 0)
            system.firstSelectedGameObject = enabledAllSelectable[index++].gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Tab) )
        {
           if(index == enabledAllSelectable.Count)
           {
               index = 0;
           }
           system.SetSelectedGameObject(enabledAllSelectable[index++].gameObject);
        }
	}
}
