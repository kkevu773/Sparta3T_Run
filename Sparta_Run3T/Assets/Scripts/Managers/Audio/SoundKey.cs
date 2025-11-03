using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundKey
{
    None,

    #region 플레이어
    SFX_PLAYER_JUMP,
    SFX_PLAYER_DOUBLEJUMP,
    SFX_PLAYER_SLIDE,
    SFX_PLAYER_LAND,
    #endregion

    #region 아이템 및 충돌
    SFX_ITEM_COIN,
    SFX_ITEM_PICKUP,
    SFX_OBSTACLE_HIT,
    SFX_ITEM_HEAL,
    #endregion

    #region UI
    SFX_UI_UICLICK,
    SFX_UI_GAMEOVER,
    #endregion

    #region 배경 음악
    BGM_DEFAULT,
    BGM_STAGE1,
    BGM_RESULT,
    BGM_BUFF,
    #endregion

}