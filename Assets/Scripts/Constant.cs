using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstantNs {
  // タグ
  public enum TAG_CONV {
    NONE,
    MAIN_CAMERA,
    GAME_CTRL,
    INPUT_CTRL,
    GRID,
    GRIDC,
  };

  // 周回したclamp(ex: 8,10,24 -> 22)
  public class Constant {
    public static float Orbit(float value, float min, float max) {
      float ret = value - min;
      if (ret < 0) { return max + ret; }
      return ret % (max - min) + min;
    }

    public static string GetTag(TAG_CONV e_tag) {
      switch (e_tag) {
        default: Debug.Assert(false, "Not Exist Tag :" + e_tag); return "";
        case TAG_CONV.MAIN_CAMERA : return "MainCamera"     ;
        case TAG_CONV.GAME_CTRL   : return "GameController" ;
        case TAG_CONV.INPUT_CTRL  : return "InputController";
        case TAG_CONV.GRID        : return "Grid"           ;
        case TAG_CONV.GRIDC       : return "GridCenter"     ;
      }
    }
  }
}
