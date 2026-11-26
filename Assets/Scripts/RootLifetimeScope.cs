using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private AudioManager audioManager;
    //[SerializeField] private ButtonPressedSound buttonPressedSound;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SequenceManager>(Lifetime.Singleton);
        builder.Register<StageProgressManager>(Lifetime.Singleton);
        builder.Register<SaveManager>(Lifetime.Singleton);

        builder.RegisterComponent(audioManager);
        //builder.RegisterComponent(buttonPressedSound);
    }
}
