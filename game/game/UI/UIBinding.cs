using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UIBinding<T>
    {
        private Func<T> valueGetter;

        public T Value => (T)valueGetter.Invoke();

        public UIBinding(Func<T> valueGetter)
        {
            this.valueGetter = valueGetter;
        }
    }
}
