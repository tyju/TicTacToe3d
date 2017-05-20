using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = ConstantNs.Constant;

public class InputController : MonoBehaviour {
  public  float      FLICK_SPEED = 5;  // フリック時のオブジェクト回転速度

  public  GameObject FlickTargetObj; // フリック時に回転するオブジェクト(中心点)
  static private Quaternion FlickTargetRot; // フリック時のオブジェクト回転の目標角度


  //===== Event Function =====//
	void Start () {
    Flick.Clear();
    Click.Clear();

    FlickTargetRot = FlickTargetObj.transform.rotation;
  }
	void Update () {
    // データ更新
    Flick.Update();
    Click.Update();

    // 処理
    SetFlickRotate(Flick.rotation);
    ChgFlickObj   (              );
    ChgClickObj   (Click.Object  );
	}


  //===== Private Function =====//
  // フリックによる回転の設定
  private void SetFlickRotate(Vector3 rot) {
    if(rot == Vector3.zero) return;

    FlickTargetObj.transform.rotation = FlickTargetRot;
    Vector3 local_rot = FlickTargetObj.transform.worldToLocalMatrix.MultiplyVector(rot);
    Quaternion new_target_rot = FlickTargetRot * Quaternion.Euler(local_rot);
    
    // 上下回転中か
    bool is_now_rotY = Mathf.Abs(Mathf.DeltaAngle(0, FlickTargetRot.eulerAngles.x)) > 85f
      || Mathf.Abs(Mathf.DeltaAngle(0, FlickTargetRot.eulerAngles.z)) > 85f;

    // 今回上下回転か
    bool is_rotY = Mathf.Abs(Mathf.DeltaAngle(0, new_target_rot.eulerAngles.x)) > 85f
      || Mathf.Abs(Mathf.DeltaAngle(0, new_target_rot.eulerAngles.z)) > 85f;

    // 上下90度回転している場合は戻る回転しか許可しない
    if(is_rotY && is_now_rotY) {
      return;
    }

    FlickTargetRot = new_target_rot;

    Flick.Clear();
  }

  // フリックによる回転
  private void ChgFlickObj() {
    FlickTargetObj.transform.rotation = Quaternion.Slerp(FlickTargetObj.transform.rotation, FlickTargetRot, FLICK_SPEED * Time.deltaTime);
  }

  // クリックによるオブジェクトの変更
  private void ChgClickObj(GameObject click_obj) {
    if(click_obj == null) return;

    // ○×オブジェクトの場合、オブジェクトを設定
    if(Click.Object.tag == Cst.GetTag(ConstantNs.TAG_CONV.GRID)) {
      Click.Object.GetComponent<GridSpace>().SetObject();
      Click.Clear();
    }
  }
}

public static class Click {
  //===== Static Definition =====//
  private static GameObject clickObj;
  
  //===== Static Function =====//
  //----- 処理 -----//
  // 初期化
  public static void Clear() {
    clickObj = null;
  }

  // 更新
  public static void Update() {
    // クリックオブジェクトの取得
    if(Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit = new RaycastHit();
      if(Physics.Raycast(ray, out hit)) {
        clickObj = hit.collider.gameObject;
      } else {
        clickObj = null;
      }
    }
  }

  //----- アクセサ -----//
  // プロパティ
  public static GameObject Object { get { return clickObj; } }
}

public static class Flick {
  //===== Static Definication =====//
  static public  int   FLICK_FLAME_MIN    = 10;  // フリックとみなすフレーム数
  static public  int   FLICK_DISTANCE_MIN = 50;  // フリックとみなすマウス差分位置

  static private int       flick_frame        ;  // フリックカウント値(フレーム)
  static private Vector3   flick_pos_start    ;  // フリック開始時点のマウス位置

  //===== Static Function =====//
  //----- 処理 -----//
  // 初期化
  public static void Clear() {
    flick_frame = 0;
  }

  // 更新
  public static void Update() {
    increaseFrame();
  }

  //----- アクセサ -----//
  // 回転方向(Vector3)の取得
  public static Vector3 rotation { get {
    if(flick_frame > FLICK_FLAME_MIN) {
      Vector3 flick_pos_end = Input.mousePosition;
      if(Vector3.Distance(flick_pos_start, flick_pos_end) > FLICK_DISTANCE_MIN ) {
        // 左回転
        if(flick_pos_start.x - flick_pos_end.x > FLICK_DISTANCE_MIN) {
          return new Vector3(0, 90,0);
        }
        // 右回転
        if(flick_pos_end.x - flick_pos_start.x > FLICK_DISTANCE_MIN) {
          return new Vector3(0,-90,0);
        }
        // 上回転
        if(flick_pos_end.y - flick_pos_start.y > FLICK_DISTANCE_MIN) {
          return new Vector3( 90,0,0);
        }
        // 下回転
        if(flick_pos_start.y - flick_pos_end.y > FLICK_DISTANCE_MIN) {
          return new Vector3(-90,0,0);
        }
      }
    }
    return Vector3.zero;
  } }

  //===== Private Function =====//
  // マウスの挙動によりフリック数[frame]を加算
  private static void increaseFrame() {
    if(Input.GetMouseButtonDown(0)) {
      flick_pos_start = Input.mousePosition;
      flick_frame = 1;
    }
    if(Input.GetMouseButton(0)) {
      if(flick_frame != 0) { flick_frame++; }
    }
    if(Input.GetMouseButtonUp(0)) {
      Flick.Clear();
    }
  }
}