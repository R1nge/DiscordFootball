using Manager.Powerups;

namespace View.Powerups
{
    public class SpeedPowerupView : PowerupViewBase
    {
        private void Start() => Powerup = new SpeedPowerup(powerupDataSo);
    }
}