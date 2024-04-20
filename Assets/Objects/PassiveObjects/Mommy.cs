using Managers;
using Objects.Abstract;

namespace Objects
{
    public class Mommy : PassiveKokTreeObjects
    {
        public delegate void OnRestartDelegate();

        public static event OnRestartDelegate OnRestart;
        public static int magicResetValue = 100;
        private int timesRestarted = 0;

        public Mommy()
        {
            objectName = "Mommy";
            kokButtonDescription = $"She will finally be proud of you. This will restart your progress. \n \n {timesRestarted} times proud so far.";
            kokButtonUnlockPrice = 5;
        }

        public override void BuyUpgrade()
        {
            RestartEverything();
            MoneyManagerSingleton.instance.ResetMoney();
            MoneyManagerSingleton.instance.ResetMultiplicationAndAddToIt(magicResetValue);
        }

        private void RestartEverything()
        {
            magicResetValue = 100 * (timesRestarted + 1);
            timesRestarted++;
            OnRestart?.Invoke();
        }
    }
}