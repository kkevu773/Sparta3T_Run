using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTestPlayer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EffectManager.Instance.Play(EffectKey.PLAYER_JUMP, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EffectManager.Instance.Play(EffectKey.PLAYER_DOUBLEJUMP, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            EffectManager.Instance.Play(EffectKey.PLAYER_SLIDE, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            EffectManager.Instance.Play(EffectKey.PLAYER_LAND, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            EffectManager.Instance.Play(EffectKey.ITEM_COIN, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            EffectManager.Instance.Play(EffectKey.ITEM_REDCOIN, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            EffectManager.Instance.Play(EffectKey.HIT_OBSTACLE, transform.position);

        if (Input.GetKeyDown(KeyCode.Alpha8))
            EffectManager.Instance.Play(EffectKey.HIT_DAMAGE, transform.position);
    }
}
