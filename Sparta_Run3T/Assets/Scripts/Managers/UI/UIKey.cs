using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIKey
{
    None,
    #region 타이틀
    UI_TITLE_PANEL,
    UI_TITLE_PLAY_BUTTON,
    UI_TITLE_EXIT_BUTTON,
    UI_TITLE_SETTING_BUTTON,
    UI_TITLE_LOGO,
    #endregion

    #region 설정
    UI_SETTING_PANEL,
    UI_SETTING_BACK_BUTTON,
    #endregion

    #region HUD
    UI_HUD_SCORE_TEXT,
    UI_HUD_BESTSCORE_TEXT,
    UI_HUD_HP_BAR,
    #endregion

    #region GAMEOVER
    UI_GAMEMOVER_PANEL,
    UI_GAMEMOVER_RETRY_BUTTON,
    UI_GAMEMOVER_TITLE_BUTTON,
    #endregion

}