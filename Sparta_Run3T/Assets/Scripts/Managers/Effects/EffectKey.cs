using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectKey
{
    None,

    #region 플레이어
    PLAYER_JUMP,
    PLAYER_DOUBLEJUMP,
    PLAYER_SLIDE,
    PLAYER_LAND,
    #endregion

    #region 아이템
    ITEM_COIN,
    ITEM_REDCOIN,
    ITEM_HEAL,
    ITEM_1,
    ITEM_2,
    #endregion

    #region 충돌
    HIT_OBSTACLE,
    HIT_DAMAGE,
    #endregion


}