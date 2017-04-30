using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  public  GameObject  obj;
  public  int   FLICK_FLAME_MIN    = 50;
  public  int   FLICK_DISTANCE_MIN = 10;
  public  float FLICK_SPEED        = 10;
  private float     flick_frame      ;
  private Vector3   flick_pos_start  ;
  private Quaternion rotate_pos       ;

  //===== Event Function =====//
	void Start () {
    flick_frame = 0;
    rotate_pos  = obj.transform.rotation;
  }
	void Update () {
    GameObject click_obj = GetClickObject();
    if (click_obj) {
      if(click_obj.tag == "Grid") {
        click_obj.GetComponent<GridSpace>().SetObject();
      }
    }

    // フリック用カウント
    {
      if(Input.GetMouseButton(0)) {
        if(flick_frame != 0) { flick_frame++; }
      }
      if(Input.GetMouseButtonDown(0)) {
        flick_pos_start = Input.mousePosition;
        flick_frame = 1;
      }
      if(Input.GetMouseButtonUp(0)) {
        flick_frame = 0;
      }
    }
    GetFlick();
	}

  //===== Private Function =====//
  // クリックされたオブジェクトの取得
  // @retval == null : クリックされなかった / オブジェクトをクリックしなかった
  private GameObject GetClickObject() {
    if(Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit = new RaycastHit();
      if(Physics.Raycast(ray, out hit)) {
        return hit.collider.gameObject;
      }
    }
    return null;
  }

  // フリック
  private void GetFlick() {
    obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, rotate_pos, FLICK_SPEED * Time.deltaTime);
    if(flick_frame > FLICK_FLAME_MIN) {
      Vector3 flick_pos_end = Input.mousePosition;
      if(Vector3.Distance(flick_pos_start, flick_pos_end) > FLICK_DISTANCE_MIN ) {
        // 左回転
        if(flick_pos_start.x - flick_pos_end.x > FLICK_DISTANCE_MIN) {
          SetFlickRotate(new Vector3(0, 90,0));
        }
        // 右回転
        if(flick_pos_end.x - flick_pos_start.x > FLICK_DISTANCE_MIN) {
          SetFlickRotate(new Vector3(0,-90,0));
        }
        // 上回転
        if(flick_pos_end.y - flick_pos_start.y > FLICK_DISTANCE_MIN) {
          SetFlickRotate(new Vector3( 90,0,0));
        }
        // 下回転
        if(flick_pos_start.y - flick_pos_end.y > FLICK_DISTANCE_MIN) {
          SetFlickRotate(new Vector3(-90,0,0));
        }
      }
    }
  }

  private void SetFlickRotate(Vector3 rot) {
    obj.transform.rotation = rotate_pos;
    Vector3 local_rot = obj.transform.worldToLocalMatrix.MultiplyVector(rot);
    flick_frame = 0;
    rotate_pos = rotate_pos * Quaternion.Euler(local_rot);
  }
}
