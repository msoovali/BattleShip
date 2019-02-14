using System;

namespace MenuSystem
{
    public class MenuItem
    {
        public string Description { get; set; }

        public Action ActionToRun { get; set; }

        public bool IsDefaultChoice { get; set; } = false;

        public MenuType MenuType { get; set; } = MenuType.Regular;

        public override string ToString()
        {
            return Description;
        }
    }
}