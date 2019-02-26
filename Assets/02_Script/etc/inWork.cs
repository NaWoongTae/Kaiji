using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inWork{

    public enum work
    {
        채광,
        공장,
        방직
    }

    work _work;

    int day;

    public work WORK
    {
        get { return _work; }
        set { _work = value; }
    }

    public int DAY
    {
        get { return day; }
        set { day = value; }
    }

    public inWork()
    {
        _work = work.채광;
        day = 0;
    }
}
