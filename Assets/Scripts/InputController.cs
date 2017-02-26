using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    GameObject click_obj = GetClickObject();
    if (click_obj != null) {
      Debug.Log(click_obj.name);
    }
	}

  /// クリックされたオブジェクトの取得
  /// @retval == null : クリックされなかった / オブジェクトをクリックしなかった
  GameObject GetClickObject() {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit = new RaycastHit();
      if(Physics.Raycast(ray, out hit)) {
        return hit.collider.gameObject;
      }
    }
    return null;
  }
}
