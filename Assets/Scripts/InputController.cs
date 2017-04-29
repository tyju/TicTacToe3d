using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

  //===== Event Function =====//
	void Start () {
		
	}
	void Update () {
    GameObject click_obj = GetClickObject();
    if (click_obj) {
      if(click_obj.tag == "Grid") {
        click_obj.GetComponent<GridSpace>().SetObject();
      }
    }
	}

  //===== Private Function =====//
  // クリックされたオブジェクトの取得
  // @retval == null : クリックされなかった / オブジェクトをクリックしなかった
  private GameObject GetClickObject() {
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
