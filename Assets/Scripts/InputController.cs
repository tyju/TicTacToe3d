using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = ConstantNs.Constant; using Tag = ConstantNs.TAG_CONV;

public class InputController : MonoBehaviour {
  public  GameObject  GridsCObj;
  public  int   FLICK_FLAME_MIN    = 50;  // フリックとみなすフレーム数
  public  int   FLICK_DISTANCE_MIN = 10;  // フリックとみなすマウス差分位置
  public  float FLICK_SPEED        = 10;  // フリック時のオブジェクト回転速度

  private float     flick_frame        ;  // フリックカウント値(フレーム)
  private Vector3   flick_pos_start    ;  // フリック開始時点のマウス位置
  private Quaternion m_rotate_pos      ;  // フリック時のオブジェクト回転の目標角度

  //===== Event Function =====//
	void Start () {
    flick_frame = 0;
    m_rotate_pos  = GridsCObj.transform.rotation;
  }
	void Update () {
    GameObject click_obj = GetClickObject();
    if (click_obj) {
      if(click_obj.tag == Cst.GetTag(Tag.GRID)) {
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
    GridsCObj.transform.rotation = Quaternion.Slerp(GridsCObj.transform.rotation, m_rotate_pos, FLICK_SPEED * Time.deltaTime);
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

  // フリックによる回転の設定
  private void SetFlickRotate(Vector3 rot) {
    GridsCObj.transform.rotation = m_rotate_pos;
    Vector3 local_rot = GridsCObj.transform.worldToLocalMatrix.MultiplyVector(rot);
    flick_frame = 0;
    bool is_rotY = Mathf.Abs(Cst.Orbit(m_rotate_pos.eulerAngles.x, -180f, 180f)) > 85f
      || Mathf.Abs(Cst.Orbit(m_rotate_pos.eulerAngles.z, -180f, 180f)) > 85f;

    Quaternion rotate_pos = m_rotate_pos * Quaternion.Euler(local_rot);

    // 上下90度回転している場合は戻る回転しか許可しない
    if(is_rotY && (Mathf.Abs(Cst.Orbit(rotate_pos.eulerAngles.x, -180f, 180f)) > 85f
      || Mathf.Abs(Cst.Orbit(rotate_pos.eulerAngles.z, -180f, 180f)) > 85f)) {
      return;
    }

    m_rotate_pos = rotate_pos;
  }
}
