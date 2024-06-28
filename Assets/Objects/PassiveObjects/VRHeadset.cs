
using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class VRHeadset : PassiveKokTreeObjects
    {
        public VRHeadset()
        {
            objectName = "VR Headset";
            kokButtonDescription = "Put them into the matrix and make them feel better";
            kokButtonUnlockPrice = 350000;
            multiplicationBonus = 30;
        }
    }
}