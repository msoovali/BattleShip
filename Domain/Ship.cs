namespace Domain
{
    public class Ship
    {
        public int ShipLength { get; set; }

        public ShipLayout ShipLayout { get; set; } = ShipLayout.Horizontal;

        public Ship(int shipLength)
        {
            ShipLength = shipLength;
        }
    }
}