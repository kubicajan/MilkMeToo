using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class Bedding : PassiveKokTreeObjects
    {
        public Bedding()
        {
            objectName = "Bedding";
            kokButtonDescription = "Such luxury - they can sleep on their food";
            kokButtonUnlockPrice = 5000;
        }
    }
}