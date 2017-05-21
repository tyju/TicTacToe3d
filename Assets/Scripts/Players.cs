using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

//===== Definition DataType =====//
[System.Serializable]
public class Player {
  public PLAYER_TYPE type;
  public GameObject  obj;
  public Image       pnl;
  public Text        txt;
  public Button      btn;
}

[System.Serializable]
public class PlayerColor {
  public PLAYER_COLOR_TYPE type      ;
  public Color             panelColor;
  public Color             textColor ;
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


[System.Serializable]
public class Players {
  //===== Private Definition DataType =====//
  private const int PLAYER_COLOR_NUM = (int)PLAYER_COLOR_TYPE.NUM;
  private const int PLAYER_NUM       = (int)PLAYER_TYPE      .NUM;
  //===== Public Definition Variable =====//
  public  PlayerColor[]   playerColors  = new PlayerColor [PLAYER_COLOR_NUM];
  public  Player     []   playerList    = new Player      [PLAYER_NUM];
  //===== Private Definition Variable =====//
  private Player          actPlayer;


  //===== Event Function =====//
  public void Initialize() {
    Clear();
  }

  public void Clear () {
    ButtonInteractive = true;
    ActPlayer         = GetPlayer(PLAYER_TYPE.NONE);
  }


  //===== Public Function =====//
  // アクセサ
  public bool   ButtonInteractive { set { Array.ForEach(playerList.Where(i => i.btn != null).ToArray(), i => i.btn.interactable = value); } }
  public Player ActPlayer         { get { return actPlayer     ; } set { ChangePlayer(value.type); } }
  
  // アクティブプレイヤの変更
  public void ChangePlayer() {
    ChangePlayer(ActPlayer.type == PLAYER_TYPE.CROSS ? PLAYER_TYPE.TICK : PLAYER_TYPE.CROSS);
  }

  // プレイヤの取得 : string
  public Player GetPlayer(string      str ) { return Array.Find(playerList, i => i.txt && i.txt.text == str); }
  public Player GetPlayer(PLAYER_TYPE type) { return Array.Find(playerList, i => i.type == type            ); }

  
  //===== Private Function =====//
  // アクティブプレイヤの変更
  private void ChangePlayer(PLAYER_TYPE act_player_type) {
    actPlayer = GetPlayer(act_player_type);

    var nnull_list = playerList.Where(i => i.pnl != null && i.txt != null).ToArray();
    foreach(var player in nnull_list) {
      PlayerColor color;
      if(player.type == ActPlayer.type) {
        color = Array.Find(playerColors, i => i.type == PLAYER_COLOR_TYPE.ACTIVE  );
      } else {
        color = Array.Find(playerColors, i => i.type == PLAYER_COLOR_TYPE.INACTIVE);
      }
      player.pnl.color = color.panelColor;
      player.txt.color = color.textColor ;
    }
  }
}
