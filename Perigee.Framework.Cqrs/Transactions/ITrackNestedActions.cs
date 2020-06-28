namespace Perigee.Framework.Cqrs.Transactions
{
    public interface ITrackNestedActions
    {
        bool InNestedAction();

        void DiveIn();
        void BubbleOut();
    }

    public class TrackNestedActions : ITrackNestedActions
    {
        private int _nestedLevel;

        public bool InNestedAction()
        {
            return _nestedLevel > 0;
        }

        public void DiveIn()
        {
            _nestedLevel++;
        }

        public void BubbleOut()
        {
            _nestedLevel--;
        }
    }
}