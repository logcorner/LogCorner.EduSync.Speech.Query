namespace LogCorner.EduSync.Speech.ReadModel.SpeechReadModel
{
    public class Entity<T>
    {
        public long Version { get; protected set; }
        public T Id { get; protected set; }
    }
}