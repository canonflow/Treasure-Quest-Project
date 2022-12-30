using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreasureQuest
{
    public class Player
    {
        // Untuk menampung stages dari Player
        public List<int> Stages = new List<int> { 1 };
        
        // Untuk menampung Items dan statu kepunyaan dari Player
        public Dictionary<string, bool> Items = new Dictionary<string, bool>
        {
            {"Map", false },
            {"Blue Diamond", false },
            {"Golden Skull", false },
            {"Green Potion", false }
        };

        public bool HasKey = false;

        public bool IsComplete()
        {
            bool complete = true;
            foreach(KeyValuePair<string, bool> pairs in this.Items)
            {
                if (pairs.Value == false)
                {
                    complete = false;
                    break;
                }
            }

            return complete;
        }
        
        // public List<string> Items = new List<string>();
    }
}
