using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction
{
    int value;
    Stat stat;
    int target;
    bool selected;
    private BattleActionType battleActionType;

    public BattleAction() {
        selected = false;
    }

    public void setBattleAction(BattleActionType newType, Stat newStat, int newValue, int newTarget) {
        stat = newStat;
        value = newValue;
        target = newTarget;
        selected = true;
        battleActionType = newType;
    }

    public Stat getStat() {
        return stat;
    }

     public int getTarget() {
        return target;
    }

     public int getValue() {
        return value;
    }

    public bool isSelected() {
        return selected;
    }

    public void setBattleActionType(BattleActionType value) {
        battleActionType = value;
    }

    public BattleActionType getBattleActionType() {
        return battleActionType;
    }

    public void completeAction() {
        selected = false;
    }
}
