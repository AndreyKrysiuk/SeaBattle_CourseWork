using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle_CourseWork
{
    public class ShipSet
    {
        public List<Ship> shipSet;
        public int countShips;

        public ShipSet()
        {
            shipSet = new List<Ship>();
        }
    }

    public abstract class ShipSetBuilder
    {
        public ShipSet shipSet;

        public void CreateShipSet()
        {
            shipSet = new ShipSet();
        }

        public abstract void SetCountShips();
        public abstract void SetShipSet();

        public ShipSet GetShipSet()
        {
            return shipSet;
        }
    }

    public class ClassicShipSetBuilder : ShipSetBuilder
    {
        public override void SetCountShips()
        {
            shipSet.countShips = 10;
        }

        public override void SetShipSet()
        {
            shipSet.shipSet.Add(new FourDeckShip());
            shipSet.shipSet.Add(new ThreeDeckShip());
            shipSet.shipSet.Add(new ThreeDeckShip());
            shipSet.shipSet.Add(new DoubleDeckShip());
            shipSet.shipSet.Add(new DoubleDeckShip());
            shipSet.shipSet.Add(new DoubleDeckShip());
            shipSet.shipSet.Add(new SingleDeckShip());
            shipSet.shipSet.Add(new SingleDeckShip());
            shipSet.shipSet.Add(new SingleDeckShip());
            shipSet.shipSet.Add(new SingleDeckShip());

        }
    }

    public class SingleDeckBattleBuilder : ShipSetBuilder
    {
        public override void SetCountShips()
        {
            shipSet.countShips = 12;
        }

        public override void SetShipSet()
        {
            for(int i = 0; i < 12; i++)
            {
                shipSet.shipSet.Add(new SingleDeckShip());
            }
        }
    }

    public class TwoShipsBattleBuilder : ShipSetBuilder
    {
        public override void SetCountShips()
        {
            shipSet.countShips = 2;
        }

        public override void SetShipSet()
        {
            shipSet.shipSet.Add(new FourDeckShip());
            shipSet.shipSet.Add(new ThreeDeckShip());
        }
    }


    public class ShipSetGenerator
    {
        public ShipSet GenerateShipSet(ShipSetBuilder shipSetBuilder)
        {
            shipSetBuilder.CreateShipSet();
            shipSetBuilder.SetCountShips();
            shipSetBuilder.SetShipSet();
            return shipSetBuilder.GetShipSet();
        }
    }
}
