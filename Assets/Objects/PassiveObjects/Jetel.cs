using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class Jetel : PassiveKokTreeObjects
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