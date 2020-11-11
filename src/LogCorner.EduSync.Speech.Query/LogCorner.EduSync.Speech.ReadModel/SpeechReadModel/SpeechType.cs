namespace LogCorner.EduSync.Speech.ReadModel.SpeechReadModel
{
    public class SpeechType
    {
        public int Value { get; }
        public string Name { get; }

        public SpeechType(int value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}