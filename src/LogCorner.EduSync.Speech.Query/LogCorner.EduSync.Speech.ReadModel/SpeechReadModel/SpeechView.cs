using System;

namespace LogCorner.EduSync.Speech.ReadModel.SpeechReadModel
{
    public class SpeechView : Entity<Guid>
    {
        public string Title { get; private set; }

        public string Url { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }

        public SpeechView(Guid id, string title, string description, string url, string type)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            Type = type;
        }
    }
}