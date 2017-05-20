using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = ConstantNs.Constant;

[System.Serializable]
public class InputController{
  public  Flick flick;
  private bool isGameStart; // ゲーム中か ※setはプロパティから

  //===== Event Function =====//
  public void Initialize() {
    flick = Flick.Instance;
    flick.Initialize();

    Clear();
  }

  public void Clear() {
    flick.Clear();

    isGameStart = false;
  }
	public void Update() {
    flick.ChgFlickObj();

    if(Input.GetMouseButtonDown(0)) {
      flick.Start(Input.mousePosition);
    }
    if(Input.GetMouseButton(0)) {
      flick.Update(Input.mousePosition);
      if(!flick.IsFlicked() && isGameStart) {
        flick.Action();
      }
    }
    if(Input.GetMouseButtonUp(0)) {
      if(!flick.IsFlicked()) {
        Click.Action(Input.mousePosition);
      }

      flick.Clear();
    }
	}


  //===== Public Function =====//
  public bool GameStart { set { Clear(); isGameStart = value; } }
}

public static class Click {
  //===== Event Function =====//
  public static void Action(Vector3 mouse_pos) {
    // オブジェクトをクリックしているか判定
    Ray ray = Camera.main.ScreenPointToRay(mouse_pos);
    RaycastHit hit = new RaycastHit();
    if(Physics.Raycast(ray, out hit)) {
      GameObject obj = hit.collider.gameObject;
      ChgClickObj(hit.collider.gameObject);
    }
  }


  //===== Private Function =====//
  // クリックによるオブジェクトの変更
  static private void ChgClickObj(GameObject click_obj) {
    if(click_obj == null) return;

    // ○×オブジェクトの場合、オブジェクトを設定
    if(click_obj.tag == Cst.GetTag(ConstantNs.TAG_CONV.GRID)) {
      click_obj.GetComponent<GridSpace>().SetObject();
    }
  }
}

[System.Serializable]
public class Flick {
  private static Flick instance = new Flick();
  public  static Flick Instance { get { return instance; } }

  //===== Static Definication =====//
  public  int   FLICK_DISTANCE_MIN = 50 ; // フリックとみなすマウス差分位置
  public  int   FLICK_FLAME_MIN    = 10 ; // フリックとみなすフレーム数
  public  float FLICK_SPEED        = 5  ; // フリック時のオブジェクト回転速度

  public  GameObject FlickTargetObj     ; // フリック時に回転するオブジェクト(中心点)
  private Quaternion FlickTargetRot     ; // フリック時のオブジェクト回転の目標角度
  private int        flick_frame        ; // フリックカウント値(フレーム)
  private Vector3    flick_pos_start    ; // フリック開始時点のマウス位置
  private Vector3    flick_pos_end      ; // フリック終了時点のマウス位置
  private bool       is_flicked         ; // 現在フリック中か

  //===== Event Function =====//
  public void Initialize() {
    FlickTargetObj = GameObject.FindGameObjectWithTag(Cst.GetTag(ConstantNs.TAG_CONV.GRIDC));
    FlickTargetRot = FlickTargetObj.transform.rotation;
    Clear();
  }

  public void Clear() {
    flick_pos_start = Vector3.zero;
    flick_pos_end   = Vector3.zero;
    flick_frame     = 0;
    is_flicked      = false;
  }

  public void Start(Vector3 start_pos) {
    flick_pos_start = start_pos;
  }
  
  public void Update(Vector3 click_pos) {
    if(flick_pos_start != Vector3.zero) {
      flick_pos_end = click_pos;
      frameIncrease();
    }
  }

  public void Action() {
    SetFlickRotate();
  }


  //===== Public Function =====//
  public bool IsFlicked() {
    return is_flicked;
  }

  // フリックによる回転
  public void ChgFlickObj() {
    FlickTargetObj.transform.rotation = Quaternion.Slerp(FlickTargetObj.transform.rotation, FlickTargetRot, FLICK_SPEED * Time.deltaTime);
  }
  

  //===== Private Function =====//
  private void frameIncrease() {
    flick_frame++;
  }

  // 回転方向(Vector3)の取得
  public Vector3 GetRotate() {
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
    return Vector3.zero;
  }

  // フリックによる回転の設定
  private void SetFlickRotate() {
    Vector3 rot = GetRotate();
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
    is_flicked = true;
  }
}