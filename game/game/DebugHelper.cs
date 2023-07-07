using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class DebugHelper
    {
        private static DebugHelper _instance = null;
        public static DebugHelper Instance => _instance;

        public DebugHelper()
        {
            if (Instance == null) _instance = this;
        }

        public bool IsDebugMode = true;



    }
}
