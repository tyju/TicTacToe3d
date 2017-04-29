﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//===== Definition DataType =====//
[System.Serializable]
public class Player {
  public PLAYER_TYPE type;
  public GameObject obj;
  public Image  panel;
  public Text   text;
  public Button button;
}

[System.Serializable]
public class PlayerColor {
  public Color panelColor;
  public Color textColor ;
}

public enum PLAYER_TYPE {
  NONE    ,
  TICK    ,
  CROSS   ,

  NUM     ,
};

public enum PLAYER_COLOR_TYPE {
  ACTIVE = 0,
  INACTIVE  ,

  NUM       ,
}


public class GameController : MonoBehaviour {
  //===== Definition DataType =====//
  public const int GRID_NUM         = 27;
  public const int PLAYER_COLOR_NUM = (int)PLAYER_COLOR_TYPE.NUM;
  public const int PLAYER_NUM       = (int)PLAYER_TYPE.NUM      ;
  //===== Definition Variable =====//
  public  GameObject    gameOverPanel ;
  public  GameObject    restartButton ;
  public  GameObject    startInfo     ;
  public  GameObject [] gridList      = new GameObject  [GRID_NUM];
  public  PlayerColor[] playerColors  = new PlayerColor [PLAYER_COLOR_NUM];
  public  Player     [] playerList    = new Player      [PLAYER_NUM];
  public  Player        actPlayer     ;
  public  Text          gameOverText  ;

  //===== State Function =====//
  private void Awake() {
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    SetButtonsInteractible (true );

    SetGameControllerReferenceOnGrids();
    ResetGrids             (     );
    SetGridsInteractible   (false);
  }
  
  //===== Private Function =====//
  //----- Grid関連 -----//
  // グリッドのgameController指定
  private void SetGameControllerReferenceOnGrids() {
    foreach(var grid in gridList) {
      grid.GetComponent<GridSpace>().SetGameControllerReference( this );
    }
  }
  // グリッドのインタラクティブ変更
  private void SetGridsInteractible(bool toggle) {
    foreach (var grid in gridList) {
      grid.GetComponent<GridSpace>().Interactive = toggle;
    }
  }
  // グリッドのリセット
  private void ResetGrids() {
    foreach(var grid in gridList) {
      grid.GetComponent<GridSpace>().ResetObject();
    }
  }

  //----- 表示関連 -----//
  // プレイヤボタンの設定
  private void SetButtonsInteractible(bool toggle) {
    foreach(var player in playerList) {
      if(player.type != PLAYER_TYPE.NONE) {
        player.button.interactable = toggle;
      }
    }
  }
  // ゲームオーバー画面の設定
  private void SetGameOverText(string value) {
    gameOverPanel.SetActive(true);
    gameOverText.text = value;
  }

  //----- アクティブプレイヤ関連 -----//
  // アクティブプレイヤの変更
  private void ChangePlayer(PLAYER_TYPE act_player_type) {
    foreach(var player in playerList) {
      if(player.type == act_player_type) {
        actPlayer = player;
        player.panel.color = playerColors[(int)PLAYER_COLOR_TYPE.ACTIVE  ].panelColor;
        player.text .color = playerColors[(int)PLAYER_COLOR_TYPE.ACTIVE  ].textColor ;
      } else if(player.panel && player.text) {
        player.panel.color = playerColors[(int)PLAYER_COLOR_TYPE.INACTIVE].panelColor;
        player.text .color = playerColors[(int)PLAYER_COLOR_TYPE.INACTIVE].textColor ;
      }
    }
  }

  //----- Playaer関連 -----//
  // プレイヤの取得 : string
  private Player GetPlayer(string str) {
    foreach(var player in playerList) {
      if(player.text && player.text.text == str) {
        return player;
      }
    }
    Debug.Assert(false, "str(" + str + ") Player is none");
    return null;
  }
  
  //----- ゲーム進行 -----//
  // ゲームオーバー処理
  private void GameOver(PLAYER_TYPE type) {
    restartButton.SetActive(true );
    SetGridsInteractible (false);
    if(type == PLAYER_TYPE.NONE) {
      ChangePlayer(type);
      SetGameOverText("It's a Draw!");
    } else {
      SetGameOverText(GetPlayer(type) + " Wins!");
    }
  }
  

  //===== Public Function =====//
  //----- ゲーム進行 -----//
  // ゲーム開始
  public void GameStart(string side_str) {
    ChangePlayer(GetPlayer(side_str).type);
    SetGridsInteractible  (true );
    SetButtonsInteractible(false);
    startInfo.SetActive   (false);
  }
  // ターン終了
  public void EndTurn () {
    // ヨコX
    for(int i = 0; i < GRID_NUM; i+=3) {
      
    }
    // ヨコY
    // ヨコZ
    // ナナメX
    // ナナメY
    // ナナメZ
    // ナナメ
    ChangeSide();
  }
  // ゲーム終了
  public void GameEnd() {
    foreach (var grid in gridList) {
      grid.GetComponent<GridSpace>().ResetObject();
    }
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    startInfo.SetActive(true);
    SetButtonsInteractible(true);
    ChangePlayer(PLAYER_TYPE.NONE);
  }

  //----- データ取得 -----//
  // プレイヤの取得 : PLAYER_TYPE
  public Player GetPlayer(PLAYER_TYPE type) {
    foreach(var player in playerList) {
      if(player.type == type) {
        return player;
      }
    }
    Debug.Assert(false, "type(" + type + ") Player is none");
    return null;
  }
  // アクティブプレイヤの取得
  public Player GetActPlayer() {
    return actPlayer;
  }
  // アクティブプレイヤの変更
  public void ChangeSide() {
    if (actPlayer.type == PLAYER_TYPE.CROSS) {
      ChangePlayer(PLAYER_TYPE.TICK );
    } else {
      ChangePlayer(PLAYER_TYPE.CROSS);
    }
  }
}