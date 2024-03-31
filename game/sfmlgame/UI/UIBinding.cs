
namespace sfmlgame.UI
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
