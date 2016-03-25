using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH, SPEED_BOOST, FOCUS, ARMOUR, WEAPON };
public enum Quality {BRONZE, SILVER, GOLD, PLATINUM}

public class item : MonoBehaviour
{
    public ItemType type;
    public Quality quality;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxSize;

    public string itemName;
    public string description;

    //Wes
    /************** BEGIN MODIFY **************/ 
    private float _strength, _intellect, _agility, _stamina;
    private float _power, _healing, _armour, _crit, _health;

    #region All Gets and Sets

    public float agility { get { return _agility; } }
    public float strength { get { return _strength; } }
    public float intellect { get { return _intellect; } }
    public float stamina { get { return _stamina; } }

    public float power { get { return _power; } }
    public float healing { get { return _healing; } }
    public float armour { get { return _armour; } }
    public float crit { get { return _crit; } }
    public float health { get { return _health; } }

    #endregion

    /************** END MODIFY **************/

    public void use()
	{
		switch(type)
		{
			case ItemType.MANA:
				Debug.Log("I just used a mana potion");
				break;
			case ItemType.HEALTH:
				Debug.Log("I just used a health potion");
				break;
			case ItemType.SPEED_BOOST:
				Debug.Log("Many fast. Much speed. woW");
				break;
            case ItemType.ARMOUR:
                Debug.Log("I just equipped some armour");
                break;
            case ItemType.WEAPON:
                Debug.Log("I just equipped a weapon");
                break;
            case ItemType.FOCUS:
                Debug.Log("I now have the power of teh dark side (focus equipped)");
                break;
			default:
				break;
		}
	}

	public string getToolTip()
	{
		string stats = string.Empty;
		string color = string.Empty;
		string newLine = string.Empty;

		if(description != string.Empty)
		{
			newLine = "\n";
		}
        
		switch(quality)
		{
			case Quality.BRONZE:
				color = "Bronze";
				break;
			case Quality.SILVER:
				color = "Silver";
				break;
			case Quality.GOLD:
				color = "Gold";
				break;
            case Quality.PLATINUM:
                color = "Platinum";
                break;
		}
        
		if(_strength > 0)
		{
			stats += "\n+" + _strength.ToString() + " Strength";
		}

		if(_intellect > 0)
		{
			stats += "\n+" + _intellect.ToString() + " Intellect";
		}

		if(agility > 0)
		{
			stats += "\n+" + _agility.ToString() + " Agility";
		}

		if(_stamina > 0)
		{
			stats += "\n+" + _stamina.ToString() + " Stamina";
		}

        if (_power > 0)
        {
            stats += "\n+" + _power.ToString() + " Power";
        }

        if (_healing > 0)
        {
            stats += "\n+" + _healing.ToString() + " Healing";
        }

        if (_health > 0)
        {
            stats += "\n+" + _health.ToString() + " Health";
        }

        if (_armour > 0)
        {
            stats += "\n+" + _armour.ToString() + " Armour";
        }

        if (_crit > 0)
        {
            stats += "\n+" + _crit.ToString() + " Crit";
        }
        
        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=teal>" + newLine + "{1}</color></i>{2}</size>", itemName, description, stats);
	}

    public void removeItem()
    {
        Destroy(this.gameObject);
    }
}
