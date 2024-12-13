using System;
using UnityEngine;

public static class GameSystemUtility
{
    /// <summary>
    /// �������l�����ă_���[�W���v�Z���܂�
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fromAttck">�U�����鑮��</param>
    /// <param name="toAttck">����̑���</param>
    /// <param name="pairAnnihilationAction">�Ώ��Ń_���[�W��������Action</param>
    /// <returns></returns>
    public static int CalcDamage(int damage, int fromAttck, int toAttck, Action pairAnnihilationAction = null)
    {
        float damageRate = 1f;

        if (fromAttck == 1) // ��
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
        else if (fromAttck == 2) // ��
        {
            if (toAttck == 1)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 3) // ��
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
        else if (fromAttck == 4) // �y
        {
            if (toAttck == 3)
            {
                damageRate = 2;
            }
        }
        else if (fromAttck == 5) // �G�[�e��
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
        else if (fromAttck == 6) // ����
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
