using System;
using UnityEngine;

public static class GameSystemUtility
{
    /// <summary>
    /// 属性を考慮してダメージを計算します
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fromAttck">攻撃する属性</param>
    /// <param name="toAttck">相手の属性</param>
    /// <param name="pairAnnihilationAction">対消滅ダメージ発生時のAction</param>
    /// <returns></returns>
    public static int CalcDamage(int damage, int fromAttck, int toAttck, Action pairAnnihilationAction = null)
    {
        float damageRate = 1f;

        if (fromAttck == 1) // 火
        {
            if (toAttck == 4)
            {
                damageRate = 2;
            }
            else if (toAttck == 3)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 2) // 風
        {
            if (toAttck == 1)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 3) // 水
        {
            if (toAttck == 2)
            {
                damageRate = 2;
            }
            else if (toAttck == 1)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 4) // 土
        {
            if (toAttck == 3)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 5) // エーテル
        {
            if (toAttck is 1 or 2 or 3 or 4)
            {
                damageRate = 1.5f;
            }
            else if (toAttck == 6)
            {
                pairAnnihilationAction?.Invoke();
            }
        }
        else if (fromAttck == 6) // 虚空
        {
            if (toAttck is 1 or 2 or 3 or 4)
            {
                damageRate = 0.5f;
            }
            else if (toAttck == 5)
            {
                damageRate = 2;
                pairAnnihilationAction?.Invoke();
            }
        }

        return Mathf.CeilToInt(damage * damageRate);
    }
}
