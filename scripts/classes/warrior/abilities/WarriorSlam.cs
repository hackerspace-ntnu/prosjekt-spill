using Godot;

public partial class WarriorSlam : Area3D
{
    [Export]
    private float damage = 50.0f;
    [Export]
    private float upwardForce = 500.0f; // Force to move object

    Timer lifetimeTimer;
    public override void _Ready()
    {
        lifetimeTimer = GetNode<Timer>("EffectLifetime");
        lifetimeTimer.Timeout += OnLifetimeTimeout;
        lifetimeTimer.Start();

        GetTree().CreateTimer(0.1f).Timeout += () => Slam();
    }

    public void Slam()
    {
        // Get all bodies within area
        var bodiesInRange = GetOverlappingBodies();
        GD.Print("Bodies in range count: ", bodiesInRange.Count);

        // Go through all bodies and apply force 
        foreach (var body in bodiesInRange)
        {
            if (body is RigidBody3D rigidBody)
            {
                GD.Print("Hit body");
                // Apply force to the body
                rigidBody.ApplyCentralImpulse(new Vector3(0, upwardForce, 0));
            }
        }
    }

    public void OnLifetimeTimeout() 
    {
        // Remove slam spell when timed out
        QueueFree();
    }
}