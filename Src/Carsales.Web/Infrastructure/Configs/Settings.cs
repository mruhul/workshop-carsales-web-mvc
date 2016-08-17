namespace Carsales.Web.Infrastructure.Configs
{
    public interface ISettings<out T> where T: class, new()
    {
        T Value { get; }
    }

    public abstract class SettingsBase<T> : ISettings<T> where T : class, new()
    {
        private static readonly object Lock = new object();
        private static T settings;

        public T Value
        {
            get
            {
                if (settings != null) return settings;

                lock (Lock)
                {
                    if (settings != null) return settings;

                    settings = SettingsHelper.GetSettings<T>(SectionName);

                    return settings;
                }
            }
        }

        protected abstract string SectionName { get; }
    }
}