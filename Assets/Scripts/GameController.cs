using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
  //===== Public Definition Variable =====//
  [SerializeField]
  public  Players         players       = new Players        ();
  public  Grids           grids         = new Grids          ();
  public  InputController inputCtrler   = new InputController();
  public  UIController    UICtrler      = new UIController   ();

  //===== State Function =====//
  private void Awake() {
    players    .Initialize(    );
    grids      .Initialize(this);
    inputCtrler.Initialize(    );
    UICtrler   .Initialize(    );
  }

  private void Update() {
    inputCtrler.Update();
  }

  //===== Private Function =====//
  //----- ゲーム進行 -----//
  // ゲームオーバー処理
  private void GameOver(PLAYER_TYPE type) {
    UICtrler  .SelectRestart();
    grids     .GridsInteractive = false;
    if(type == PLAYER_TYPE.NONE) {
      players.ActPlayer = players.GetPlayer(type);
      UICtrler.GameOver("It's a Draw!");
    } else {
      UICtrler.GameOver(players.ActPlayer.txt.text + " Wins!");
    }
  }
  

  //===== Public Function =====//
  //----- ゲーム進行 -----//
  // ゲーム開始
  public void GameStart(string side_str) {
    players    .ActPlayer        = players.GetPlayer(side_str);
    grids      .GridsInteractive = true ;
    inputCtrler.GameStart        = true ;
    players    .ButtonInteractive= false;
    UICtrler.GameStart();
  }
  // ターン終了
  public void EndTurn () {
    // 勝敗判定
    if(grids.IsWin()) {
      GameOver(players.ActPlayer.type);
      return;
    }
    if(grids.IsDrow()) {
      GameOver(PLAYER_TYPE.NONE);
      return;
    }

    players.ChangePlayer();
  }
  // ゲーム終了
  public void GameEnd() {
    players    .Clear();
    grids      .Clear();
    inputCtrler.Clear();
    UICtrler   .Clear();
  }
}
