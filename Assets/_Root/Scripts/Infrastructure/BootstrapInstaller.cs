using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this);
        }

        public void Initialize()
        {
            Application.runInBackground = true;
        }
    }
}