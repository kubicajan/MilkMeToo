using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class Jetel : PassiveKokTreeObject
    {
        public Jetel()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            objectName = "Jetel";
            kokButtonDescription = "Maybe it is time to start feeding your cows";
            kokButtonUnlockPrice = 5;
        }
    }
}