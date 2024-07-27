using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public List<Item> equipItem = new List<Item>();
        public int killcnt = 0;
        public float HP = 100f;
        public float damage = 25;
        public float speed = 6.0f;
    }

    [System.Serializable]
    public class Item
    {
        public enum ITEMTYPE {HP, SPEED, GRENADE, DAMAGE}
        public enum ITEMCAL {VALUE, PERSENT};

        public ITEMTYPE itemtype;   //아이템 종류
        public ITEMCAL itemcal;     //아이템 계산 종류
        public string name;         //아이템 이름
        public string detail;       //아이템 소개
        public float value;         //아이템 값
    }
}