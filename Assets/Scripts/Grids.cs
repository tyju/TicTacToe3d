using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class Grids {
  private static Grids instance = new Grids();
  public  static Grids Instance { get { return instance; } }
  private Grids() { }

  //===== Public Definication =====//
  public GridSpace [] gridList;

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
    Array.ForEach(gridList, i => i.ResetObject());
  }

  //----- アクセサ -----//
  public bool GridsInteractive { set { Array.ForEach(gridList, i => i.Interactive = value); } }
  
  // 指定されたGridは選択可能か
  public bool IsClickable(GameObject obj) {
    var grid_idx = Array.FindIndex(gridList, i => i.transform.gameObject == obj);
    // 1つ下のGridは出現後でなければ選択できない
    var under_grid_idx = grid_idx + 3;
    if(under_grid_idx % 9 > 2 && gridList[under_grid_idx].IsClickable()) {
      return false;
    }
    // 選択可能でなければ選択できない
    return gridList[grid_idx].IsClickable();
  }
  
  //----- 勝敗判定 -----//
  // 勝敗判定 : 勝利
  public bool IsWin() {
    // ヨコX
    for(int i = 1 ; i+1  < gridList.Length; i+=3) { if(IsWin(i-1 , i, i+1 )) { return true; } }
    // ヨコY
    for(int i = 3 ; i+3  < (gridList.Length)/3; i=(i+9)%(gridList.Length-1)) { if(IsWin(i-3 , i, i+3 )) { return true; } }
    // ヨコZ
    for(int i = 9 ; i+9  < gridList.Length; i+=1) { if(IsWin(i-9 , i, i+9 )) { return true; } }
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
    return !gridList.Any(i => i.Type == PLAYER_TYPE.NONE);
  }
    
  
  //===== Private Function =====//
  // gameController指定
  private void SetGameController(GameController gm_ctrler) {
    Array.ForEach(gridList, i => i.SetGameControllerReference(gm_ctrler));
  }

  // 勝敗判定 : 勝利 : 1列
  private bool IsWin(int a, int b, int c) {
    Debug.Assert(gridList.Length > a && gridList.Length > b && gridList.Length > c, a + " " + b + " " + c);
    PLAYER_TYPE type = gridList[a].Type;
    if(type == PLAYER_TYPE.NONE) { return false; }

    if(  gridList[b].Type == type
      && gridList[c].Type == type) {
      Debug.Log(a + " " + b + " " + c);
      return true;
    }
    return false;
  }
}
