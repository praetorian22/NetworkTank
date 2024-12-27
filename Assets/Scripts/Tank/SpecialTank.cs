using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTank : MonoBehaviour
{
    private List<Special> specialsHavePlayer;

    private void Awake()
    {
        specialsHavePlayer = new List<Special>();
        specialsHavePlayer.Add(new Special(positionSpecial.one, typeSpecial.none));
        specialsHavePlayer.Add(new Special(positionSpecial.two, typeSpecial.none));
        specialsHavePlayer.Add(new Special(positionSpecial.three, typeSpecial.none));
    }
    private bool CheckTypeSpecial(typeSpecial special)
    {
        foreach (Special sp in specialsHavePlayer)
        {
            if (sp.Equals(new Special(positionSpecial.free, special))) return true;
        }
        return false;
    }
    public void AddSpecial(typeSpecial special)
    {
        if (CheckTypeSpecial(special)) return;
        bool checkFree = false;
        foreach (Special sp in specialsHavePlayer)
        {
            if (sp.type == typeSpecial.none)
            {
                sp.type = special;
                checkFree = true;
                GameManager.singleton.TakeNewSpecial(sp);
                return;
            }
        }
        if (!checkFree)
        {
            specialsHavePlayer[Random.Range(0,3)].type = special;
        }
    }
    public void DeleteSpecial(typeSpecial special)
    {
        foreach (Special sp in specialsHavePlayer)
        {
            if (sp.Equals(new Special(positionSpecial.free, special)))
            {
                sp.type = typeSpecial.none;
            }
        }
    }
}

public enum typeSpecial
{
    invisibility,
    boost,
    fireWall,
    none,
}
public enum positionSpecial
{
    one,
    two,
    three,
    free
}
public class Special
{
    private positionSpecial positionSpecial;
    public typeSpecial type;
    public positionSpecial PositionSpecial => positionSpecial;

    public Special(positionSpecial positionSpecial, typeSpecial type)
    {
        this.positionSpecial = positionSpecial;
        this.type = type;
    }
    public Special(Special special)
    {
        positionSpecial = special.positionSpecial;
        type = special.type;
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Special)) return false;
        Special other = (Special)obj;
        return type == other.type;
    }
}