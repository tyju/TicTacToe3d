using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridController {
  //===== Public Definication =====//
  public GameObject [] gridList;

  //===== Public Function =====//
  //----- 処理 -----//
  public void Initialize(GameController gm_ctrler) {
    Debug.Assert(gridList.Length > 0);

    SetGameController(gm_ctrler);
    Clear();
  }

  public void Clear() {
    Reset();
    GridsInteractive = false;
  }
  
  private void Reset() {
    foreach(var grid in gridList) {
      grid.GetComponent<GridSpace>().ResetObject();
    }
  }

  //----- アクセサ -----//
  public bool GridsInteractive { set {
    foreach (var grid in gridList) {
      grid.GetComponent<GridSpace>().Interactive = value;
    }
  } }
  
  
  //----- 勝敗判定 -----//
  // 勝敗判定 : 勝利
  public bool IsWin() {
    // ヨコX
    for(int i = 1 ; i    < gridList.Length; i+=3) { if(IsWin(i-1 , i, i+1 )) { return true; } }
    // ヨコY
    for(int i = 3 ; i    < gridList.Length; i+=1) { if(IsWin(i-3 , i, i+3 )) { return true; } }
    // ヨコZ
    for(int i = 9 ; i    < gridList.Length; i+=1) { if(IsWin(i-9 , i, i+9 )) { return true; } }
    // ナナメX
    for(int i = 12; i+12 < gridList.Length; i+=1) {
      if(IsWin(i-12, i, i+12)) { return true; }
      if(IsWin(i-6 , i, i+6 )) { return true; }
    }
    // ナナメY
    for(int i = 10; i+10 < gridList.Length; i+=3) {
      if(IsWin(i-10, i, i+10)) { return true; }
      if(IsWin(i-8 , i, i+8 )) { return true; }
    }
    // ナナメZ
    for(int i = 4 ; i+4  < gridList.Length; i+=9) {
      if(IsWin(i-4 , i, i+4 )) { return true; }
      if(IsWin(i-2 , i, i+2 )) { return true; }
    }
    // ナナメ
    {
      if(IsWin(6   , 13, 20 )) { return true; }
      if(IsWin(2   , 13, 24 )) { return true; }
    }

    return false;
  }
  // 勝敗判定 : 引き分け
  public bool IsDrow() {
    for(int i = 0; i < gridList.Length; i++) {
      if(gridList[i].GetComponent<GridSpace>().Type == PLAYER_TYPE.NONE) return false;
    }
    return true;
  }
    
  
  //===== Private Function =====//
  // gameController指定
  private void SetGameController(GameController gm_ctrler) {
    foreach(var grid in gridList) {
      grid.GetComponent<GridSpace>().SetGameControllerReference( gm_ctrler );
    }
  }

  // 勝敗判定 : 勝利 : 1列
  private bool IsWin(int a, int b, int c) {
    PLAYER_TYPE type = gridList[a].GetComponent<GridSpace>().Type;
    if(type == PLAYER_TYPE.NONE) { return false; }

    if(  gridList[b].GetComponent<GridSpace>().Type == type
      && gridList[c].GetComponent<GridSpace>().Type == type) {
      return true;
    }
    return false;
  }
}
