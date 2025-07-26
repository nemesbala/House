using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class InventoryDisplayer : MonoBehaviour
{
    public GameObject Material;
    public GameObject Food;
    public GameObject Special;

    [Header("Sawmill")]
    public TextMeshProUGUI TLumber;
    public TextMeshProUGUI TWoodenBeam;
    public TextMeshProUGUI TWoodenPanel;
    public TextMeshProUGUI TLog;

    [Header("Farm")]
    public TextMeshProUGUI TTomato;
    public TextMeshProUGUI TPotato;
    public TextMeshProUGUI TWheat;
    public TextMeshProUGUI THop;
    public TextMeshProUGUI TEgg;
    public TextMeshProUGUI TMilk;
    public TextMeshProUGUI TCarrot;
    public TextMeshProUGUI TRaspberry;

    [Header("Brickyard")]
    public TextMeshProUGUI TBrick;
    public TextMeshProUGUI TRoofTile;
    public TextMeshProUGUI TConcrete;
    public TextMeshProUGUI TTile;

    [Header("Food Factory")]
    public TextMeshProUGUI TBread;
    public TextMeshProUGUI TBeer;
    public TextMeshProUGUI TDoughnut;
    public TextMeshProUGUI TCheese;

    [Header("Steel Mill")]
    public TextMeshProUGUI TNail;
    public TextMeshProUGUI TSteelBeam;
    public TextMeshProUGUI TMetalSheet;
    public TextMeshProUGUI TSteelRod;
    public TextMeshProUGUI TScrew;

    [Header("Collectibles")]
    public TextMeshProUGUI TOldPainting;
    public TextMeshProUGUI TFlower;
    public TextMeshProUGUI TSignature;

    [Header("Rewards")]
    public TextMeshProUGUI TStar;
    public TextMeshProUGUI TSpinTicket;
    public TextMeshProUGUI TEliteSpinTicket;
    public TextMeshProUGUI TBronzeMedal;
    public TextMeshProUGUI TSilverMedal;
    public TextMeshProUGUI TGoldMedal;
    public TextMeshProUGUI TCitizenTypeChange;
    public TextMeshProUGUI TInstantRent;
    public TextMeshProUGUI TInstantBuild;

    // Start is called before the first frame update
    public void Start()
    {
        string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
        int[] data = new int[37];
        // Check if the file exists, otherwise initialize with 0s
        if (!File.Exists(inventoryFilePath))
        {
            //Debug.Log("File does not exist. Initializing with 34 zeros.");
            data[0] = 10;
            data[34] = 1;
            using (StreamWriter writer = new StreamWriter(inventoryFilePath))
            {
                foreach (int element in data)
                {
                    writer.WriteLine(element); // Write each element on a new line
                }
            }
        }
        else
        {
            string[] datas = File.ReadAllLines(inventoryFilePath);

            int Lumber = int.Parse(datas[0]);
            int WoodenBeam = int.Parse(datas[1]);
            int WoodenPanel = int.Parse(datas[2]);
            int Log = int.Parse(datas[3]);
            int Tomato = int.Parse(datas[4]);
            int Potato = int.Parse(datas[5]);
            int Wheat = int.Parse(datas[6]);
            int Hop = int.Parse(datas[7]);
            int Egg = int.Parse(datas[8]);
            int Milk = int.Parse(datas[9]);
            int Carrot = int.Parse(datas[10]);
            int Raspberry = int.Parse(datas[11]);
            int Brick = int.Parse(datas[12]);
            int RoofTile = int.Parse(datas[13]);
            int Concrete = int.Parse(datas[14]);
            int Tile = int.Parse(datas[15]);
            int Bread = int.Parse(datas[16]);
            int Beer = int.Parse(datas[17]);
            int Doughnut = int.Parse(datas[18]);
            int Cheese = int.Parse(datas[19]);
            int Nail = int.Parse(datas[20]);
            int SteelBeam = int.Parse(datas[21]);
            int MetalSheet = int.Parse(datas[22]);
            int SteelRod = int.Parse(datas[23]);
            int Screw = int.Parse(datas[24]);
            int OldPainting = int.Parse(datas[25]);
            int Flower = int.Parse(datas[26]);
            int Signature = int.Parse(datas[27]);
            int Star = int.Parse(datas[28]);
            int SpinTicket = int.Parse(datas[29]);
            int EliteSpinTicket = int.Parse(datas[30]);
            int BronzeMedal = int.Parse(datas[31]);
            int SilverMedal = int.Parse(datas[32]);
            int GoldMedal = int.Parse(datas[33]);
            int CitizenTypeChange = int.Parse(datas[34]);
            int InstantRent = int.Parse(datas[35]);
            int InstantBuild = int.Parse(datas[36]);

            TLumber.text = Lumber.ToString();
            TWoodenBeam.text = WoodenBeam.ToString();
            TWoodenPanel.text = WoodenPanel.ToString();
            TLog.text = Log.ToString();
            TTomato.text = Tomato.ToString();
            TPotato.text = Potato.ToString();
            TWheat.text = Wheat.ToString();
            THop.text = Hop.ToString();
            TEgg.text = Egg.ToString();
            TMilk.text = Milk.ToString();
            TCarrot.text = Carrot.ToString();
            TRaspberry.text = Raspberry.ToString();
            TBrick.text = Brick.ToString();
            TRoofTile.text = RoofTile.ToString();
            TConcrete.text = Concrete.ToString();
            TTile.text = Tile.ToString();
            TBread.text = Bread.ToString();
            TBeer.text = Beer.ToString();
            TDoughnut.text = Doughnut.ToString();
            TCheese.text = Cheese.ToString();
            TNail.text = Nail.ToString();
            TSteelBeam.text = SteelBeam.ToString();
            TMetalSheet.text = MetalSheet.ToString();
            TSteelRod.text = SteelRod.ToString();
            TScrew.text = Screw.ToString();
            TOldPainting.text = OldPainting.ToString();
            TFlower.text = Flower.ToString();
            TSignature.text = Signature.ToString();
            TStar.text = Star.ToString();
            TSpinTicket.text = SpinTicket.ToString();
            TEliteSpinTicket.text = EliteSpinTicket.ToString();
            TBronzeMedal.text = BronzeMedal.ToString();
            TSilverMedal.text = SilverMedal.ToString();
            TGoldMedal.text = GoldMedal.ToString();
            TCitizenTypeChange.text = CitizenTypeChange.ToString();
            TInstantRent.text = InstantRent.ToString();
            TInstantBuild.text = InstantBuild.ToString();
        }
    }

    public void Materials()
    {
        Material.SetActive(true);
        Food.SetActive(false);
        Special.SetActive(false);
    }
    public void Foods()
    {
        Material.SetActive(false);
        Food.SetActive(true);
        Special.SetActive(false);
    }
    public void Specials()
    {
        Material.SetActive(false);
        Food.SetActive(false);
        Special.SetActive(true);
    }
}
