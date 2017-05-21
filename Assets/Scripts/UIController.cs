using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIController {
  //===== Public Definition =====//
  public  GameObject      gameOverPnl;
  public  GameObject      restartBtn ;
  public  GameObject      infoPnl    ;


  //===== Event Function =====//
  public void Initialize() {
    Clear();
  }

  public void Clear() {
    SelectStart();
  }


  //===== Public Function =====//
  public void SelectStart() {
    gameOverPnl.SetActive(false);
    restartBtn .SetActive(false);
    infoPnl    .SetActive(true );
  }

  public void GameStart() {
    infoPnl    .SetActive(false);
  }

  public void SelectRestart() {
    restartBtn .SetActive(true );
  }
  
  public void GameOver(string text) {
    gameOverPnl.SetActive(true );
    gameOverPnl.GetComponentInChildren<Text>().text = text;
  }
}
