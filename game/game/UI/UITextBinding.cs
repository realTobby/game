using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UITextBinding
    {
        private Func<string> valueGetter;

        public string Value => valueGetter?.Invoke() ?? "";

        public UITextBinding(Func<string> valueGetter)
        {
            this.valueGetter = valueGetter;
        }
    }
}
