using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class ChildLabour : PassiveKokTreeObject
    {
        public ChildLabour()
        {
            objectName = "Child Labour";
            kokButtonDescription = "Let everyone help";
            kokButtonUnlockPrice = 5;
        }
    }
}