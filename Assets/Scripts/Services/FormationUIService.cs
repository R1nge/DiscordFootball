using Cysharp.Threading.Tasks;
using Manager.GamePlay;

namespace Services
{
    public class FormationUIService
    {
        private readonly FormationManager _formationManager;

        public FormationUIService(FormationManager formationManager)
        {
            _formationManager = formationManager;
        }

        public void SelectFormation(int index, ulong clientId)
        {
            _formationManager.SelectFormation(index, clientId);
        }
    }
}