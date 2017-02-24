using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player {
  public Image  panel;
  public Text   text;
  public Button button;
}

[System.Serializable]
public class PlayerColor {
  public Color panelColor;
  public Color textColor;
}

public class GameController : MonoBehaviour {
  //----- Definition Variable -----//
  public  GameObject  gameOverPanel;
  public  GameObject  restartButton;
  public  GameObject  startInfo;
  public  Player      playerX;
  public  Player      playerO;
  public  PlayerColor  activePlayerColor;
  public  PlayerColor  inactivePlayerColor;
  public  Text        gameOverText;
  public  Text[]      buttonList;
  private string      playerSide;
  private int         moveCount;

  //----- State Function      -----//
  private void Awake() {
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    moveCount   = 0;

    SetGameControllerReferenceOnButtons();
  }
  
  //----- Private Function    -----//
  private void SetGameControllerReferenceOnButtons() {
    foreach(Text bt in buttonList) {
      bt.GetComponentInParent<GridSpace>().SetGameControllerReference(this);
    }
  }

  private void SetBoardInteractible(bool toggle) {
    foreach (Text bt in buttonList) {
      bt.GetComponentInParent<Button>().interactable = toggle;
    }
  }

  private void SetPlayerButtons(bool toggle) {
    playerX.button.interactable = toggle;
    playerO.button.interactable = toggle;
  }

  private void StartGame() {
    SetBoardInteractible(true);
    SetPlayerButtons(false);
    startInfo.SetActive(false);
  }

  private void ChangePlayer(string player) {
    playerSide = player;
    if (player == "X") {
      SetPlayerColors(playerX, playerO);
    } else {
      SetPlayerColors(playerO, playerX);
    }
  }

  private void SetPlayerColor(Player player, PlayerColor color) {
    player.panel.color = color.panelColor;
    player.text.color  = color.textColor;
  }
  private void SetPlayerColors(Player newPlayer, Player oldPlayer) {
    SetPlayerColor(newPlayer,   activePlayerColor);
    SetPlayerColor(oldPlayer, inactivePlayerColor);
  }
  private void SetPlayerColorsInactive() {
    SetPlayerColor(playerO, inactivePlayerColor);
    SetPlayerColor(playerX, inactivePlayerColor);
  }

  private void SetGameOverText(string value) {
    gameOverPanel.SetActive(true);
    gameOverText.text = value;
  }

  private void GameOver(string winningPlayer) {
    restartButton.SetActive(true);
    SetBoardInteractible(false);
    if(winningPlayer == "draw") {
      SetPlayerColorsInactive();
      SetGameOverText("It's a Draw!");
    } else {
      SetGameOverText(winningPlayer + " Wins!");
    }
  }

  //----- Public Function     -----//
  public void SetStartingSide(string startingSide) {
    ChangePlayer(startingSide);
    StartGame();
  }

  public string GetPlaySide() {
    return playerSide;
  }

  public void ChangeSide() {
    if (playerSide == "X") {
      ChangePlayer("O");
    } else {
      ChangePlayer("X");
    }
  }

  public void EndTurn () {
    moveCount++;
    if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) {
      GameOver(playerSide);
    }
    else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide) {
      GameOver(playerSide);
    }
    else if (moveCount >= 9) {
      GameOver("draw");
    }
    else {
      ChangeSide();
    }
  }

  public void RestartGame() {
    moveCount = 0;
    foreach (Text bt in buttonList) {
      bt.text = "";
    }
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    startInfo.SetActive(true);
    SetPlayerButtons(true);
    SetPlayerColorsInactive();
  }

}
