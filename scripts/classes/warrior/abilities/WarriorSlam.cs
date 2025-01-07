using Godot;

public partial class WarriorSlam : Area3D
{
    [Export]
    private float damage = 50.0f;
    [Export]
    private float force = 5.0f; // Force to move object

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

        Godot.Vector3 originPosition = GlobalTransform.Origin;

        // Go through all bodies and apply force 
        foreach (var body in bodiesInRange)
        {
            if (body is RigidBody3D rigidBody)
            {
                Godot.Vector3 direction = (rigidBody.GlobalTransform.Origin - originPosition).Normalized();

                direction = (direction + Godot.Vector3.Up).Normalized();

                // Apply force to the body
                rigidBody.ApplyCentralImpulse(direction * force);
            }
        }
    }

    public void OnLifetimeTimeout() 
    {
        // Remove slam spell when timed out
        QueueFree();
    }
}