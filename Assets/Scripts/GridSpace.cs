///========================================
/// ノード描画スペース
///========================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {
  private GameController  gameController  ;
  private new GameObject  camera          ;

  private PLAYER_TYPE     m_type          ;
  private bool            m_is_interactive;
  
  //===== Event Function =====//
  // 初期化
  void Start() {
    camera = GameObject.FindGameObjectWithTag("MainCamera");
  }


  //===== Public Function =====//
  //----- 初期設定関連 -----//
  public void SetGameControllerReference(GameController controller) {
    gameController = controller;
  }

  //----- オブジェクト変更関連 -----//
  // オブジェクトの設定
  public void SetObject() {
    if(m_type == PLAYER_TYPE.NONE && m_is_interactive) {
      var player = gameController.GetActPlayer();
      m_type = player.type;
      ChangeObject(m_type);
      m_is_interactive = false;

      gameController.EndTurn();
    }
  }
  // オブジェクトのリセット
  public void ResetObject() {
    m_type = PLAYER_TYPE.NONE;
    Debug.Assert(gameController, "gamecontroller is null");
    ChangeObject(m_type);
    m_is_interactive = true;
  }
  
  //----- プロパティ -----//
  public bool Interactive { get { return m_is_interactive; } set { m_is_interactive = value; } }
  public PLAYER_TYPE Type { get { return m_type          ; } }


  //===== private Function =====//
  //----- オブジェクト変更関連 -----//
  // オブジェクトの変更
  private void ChangeObject(PLAYER_TYPE type) {
    Player player = gameController.GetPlayer(type);
    if(transform.childCount > 0) {
      Destroy(transform.GetChild(0).gameObject);
    }
    Debug.Assert(player.obj, "not obj");
    GameObject create_obj = Instantiate (player.obj.gameObject, transform);
    create_obj.transform.position = transform.position;
    if(type != PLAYER_TYPE.NONE) {
      create_obj.transform.LookAt(camera.transform);
    }
  }
}
