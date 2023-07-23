using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class Bedding : PassiveKokTreeObject
    {
        public Bedding()
        {
            objectName = "Bedding";
            kokButtonDescription = "Such luxury - they can sleep on their food";
            kokButtonUnlockPrice = 5;
        }
    }
}