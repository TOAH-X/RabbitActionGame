using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [Header("“GƒLƒƒƒ‰ID")]
    public int enemyId = 1;                             //“G‚ÌID(0‚Íƒfƒ‚—p‚È‚Ì‚Å1‚©‚çn‚ß‚é)
    [Header("“GƒLƒƒƒ‰–¼")]
    public String enemyName;                            //“G‚Ì–¼‘O
    [Header("í‘°–¼")]
    public String enemyRace;                            //“G‚Ìí‘°
    [Header("“G‚Ì‘®«")]
    public int enemyAttribute = 1;                      //“G‚Ì‘®«
    [Header("“G‚ÌUŒ‚—Í")]
    public int enemyAttack = 100;                       //“G‚ÌUŒ‚—Í(Œµ–§‚É‚ÍŠî‘bUŒ‚—Í)
    [Header("“G‚ÌUŒ‚—Í‚ÌŠî‘b”{—¦")]
    public float enemyAttackBaseRate = 1;               //“G‚ÌUŒ‚—Í‚ÌŠî‘b”{—¦
    [Header("“G‚ÌÅ‘åHP")]
    public int enemyMaxHp = 10000;                      //“G‚ÌÅ‘åHP
    [Header("“G‚ÌÅ‘åHP‚ÌŠî‘b”{—¦")]
    public float enemyMaxHpBaseRate = 1;                //“G‚ÌÅ‘åHP‚ÌŠî‘b”{—¦
    [Header("“GƒCƒ‰ƒXƒg")]
    public Sprite enemyPicture;                         //“G‚ÌƒCƒ‰ƒXƒg
}
