using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 周回したclamp(ex: 8,10,24 -> 22)
public class Constant{
  public static float Orbit(float value, float min, float max) {
    float ret = value - min;
    if(ret < 0) { return max + ret; }
    return ret % (max - min) + min;
  }
}
