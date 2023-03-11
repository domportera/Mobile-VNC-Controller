namespace CustomDotNetExtensions
{
    public interface IIdentified
    {
        ulong ID { get; }
        DebugMode DebugMode { get; }

        void HighlightInEditor();
    }

    public interface IIdentifiedNamed : IIdentified
    {
        string Name { get; }
    }

    public enum DebugMode { Off, ErrorsOnly, Debug }

    public static class IIdentifiedExtensions
    {
        public static bool ShouldLogErrors(this IIdentified obj)
        {
            return obj.DebugMode > DebugMode.Off;
        }

        public static bool ShouldLog(this IIdentified obj)
        {
            return obj.DebugMode > DebugMode.ErrorsOnly;
        }

        public static string GenerateName(this IIdentified sender)
        {
            return $"{sender.GetType().Name} ({sender.ID.ToString()})";
        }

        //despite the reference count, this is called when an IIdentifiedNamed is passed as
        //an argument into methods accepting IIdentified parameters
        public static string GenerateName(this IIdentifiedNamed sender)
        {
            return $"{sender.GetType().Name} \"{sender.Name}\" ({sender.ID.ToString()})";
        }
    }
}
