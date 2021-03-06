﻿///========================================
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
    if(IsClickable()) {
      ChangeObject(gameController.players.ActPlayer.type);
      m_is_interactive = false;

      gameController.EndTurn();
    }
  }
  // オブジェクトのリセット
  public void ResetObject() {
    Debug.Assert(gameController, "gamecontroller is null");
    ChangeObject(PLAYER_TYPE.NONE);
    m_is_interactive = true;
  }
  // オブジェクトはクリック可能か
  public bool IsClickable() {
    return m_type == PLAYER_TYPE.NONE && m_is_interactive;
  }
  
  //----- プロパティ -----//
  public bool Interactive { get { return m_is_interactive; } set { m_is_interactive = value; } }
  public PLAYER_TYPE Type { get { return m_type          ; } }


  //===== private Function =====//
  //----- オブジェクト変更関連 -----//
  // オブジェクトの変更
  private void ChangeObject(PLAYER_TYPE type) {
    m_type = type;
    Player player = gameController.players.GetPlayer(m_type);
    if(transform.childCount > 0) {
      Destroy(transform.GetChild(0).gameObject);
    }
    if(type != PLAYER_TYPE.NONE) {
      GameObject create_obj = Instantiate (player.obj.gameObject, transform);
      create_obj.transform.position = transform.position;
      create_obj.transform.LookAt(camera.transform);
    }
  }
}
