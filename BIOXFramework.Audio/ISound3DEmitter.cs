using Microsoft.Xna.Framework;

namespace BIOXFramework.Audio
{
    public interface ISound3DEmitter
    {
        Vector3 SoundEmitterPosition { get; }
        Vector3 SoundEmitterForward { get; }
        Vector3 SoundEmitterUp { get; }
        Vector3 SoundEmitterVelocity { get; }
    }
}