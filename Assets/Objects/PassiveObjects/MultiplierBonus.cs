using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class MultiplierBonus : PassiveKokTreeObjects
    {
        public MultiplierBonus()
        {
            objectName = "Bedding";
            kokButtonDescription = "Such luxury - they can sleep on their food";
            kokButtonUnlockPrice = 35000;
            multiplicationBonus = 20;
        }
    }
}