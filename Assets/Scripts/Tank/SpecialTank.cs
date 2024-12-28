using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpecialTank : MonoBehaviour
{
    private List<Special> specialsHavePlayer;
    public Dictionary<typeSpecial, Action<GameObject>> actionSpecialsDict;

    private Coroutine invisibilityCoro;
    [SerializeField] private float timeInvisibility;

    private void Awake()
    {
        specialsHavePlayer = new List<Special>();
        specialsHavePlayer.Add(new Special(positionSpecial.one, typeSpecial.none, null));
        specialsHavePlayer.Add(new Special(positionSpecial.two, typeSpecial.none, null));
        specialsHavePlayer.Add(new Special(positionSpecial.three, typeSpecial.none, null));

        actionSpecialsDict = new Dictionary<typeSpecial, Action<GameObject>>();
        actionSpecialsDict.Add(typeSpecial.invisibility, Invisibility);
    }
    private bool CheckTypeSpecial(typeSpecial special)
    {
        foreach (Special sp in specialsHavePlayer)
        {
            if (sp.Equals(new Special(positionSpecial.free, special, null))) return true;
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
                sp.action = actionSpecialsDict[special];
                checkFree = true;
                GameManager.singleton.TakeNewSpecial(sp);
                return;
            }
        }
        if (!checkFree)
        {
            int index = UnityEngine.Random.Range(0, 3);
            specialsHavePlayer[index].type = special;
            specialsHavePlayer[index].action = actionSpecialsDict[special];
            GameManager.singleton.TakeNewSpecial(specialsHavePlayer[index]);
        }
    }
    public void DeleteSpecial(typeSpecial special)
    {
        foreach (Special sp in specialsHavePlayer)
        {
            if (sp.Equals(new Special(positionSpecial.free, special, null)))
            {
                sp.type = typeSpecial.none;
                sp.action = null;
            }
        }
    }

    public void Invisibility(GameObject gameObject)
    {
        if (invisibilityCoro != null)
        {
            StopCoroutine(invisibilityCoro);
        }
        StartCoroutine(InvisibilityCoro(gameObject));
    }
    private IEnumerator InvisibilityCoro(GameObject gameObject)
    {
        gameObject.GetComponent<Animator>().SetTrigger("invisON");
        yield return new WaitForSeconds(timeInvisibility);
        gameObject.GetComponent<Animator>().SetTrigger("invisOFF");
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
    public Action<GameObject> action;

    public Special(positionSpecial positionSpecial, typeSpecial type, Action<GameObject> action)
    {
        this.positionSpecial = positionSpecial;
        this.type = type;
        this.action = action;
    }
    public Special(Special special)
    {
        positionSpecial = special.positionSpecial;
        type = special.type;
        action = special.action;
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Special)) return false;
        Special other = (Special)obj;
        return type == other.type;
    }
}