using System.Web.UI.WebControls;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// An abstract class that is a base for html 5 controls
    /// </summary>
    public abstract class Html5Control : WebControl
    {
        //private string p;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hemrika.SharePresence.Html5.WebControls.Html5Control" /> class
        /// </summary>
        public Html5Control() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Hemrika.SharePresence.Html5.WebControls.Html5Control" /> class which renders the specified tag
        /// </summary>
        /// <param name="tag">Tag of the html element</param>
        public Html5Control(string tag) : base(tag) {}

        public Html5Control(HtmlTextWriterTag tag) : base (tag) {}

        /// <summary>
        /// Saves an object in the ViewState of the control
        /// </summary>
        /// <param name="key">Key to store the object</param>
        /// <param name="value">The value to store</param>
        protected virtual void SetViewState(string key, object value)
        {
            ViewState[key] = value;
        }
        /// <summary>
        /// Fetches a value from ViewState and returns default value of the type if it does not exist
        /// </summary>
        /// <typeparam name="T">Type of the object desired to fetch</typeparam>
        /// <param name="key">Key to retrieve the object</param>
        /// <returns>Value in ViewState and default if ViewState does not contain the key</returns>
        protected virtual T GetViewState<T>(string key)
        {
            return GetViewState<T>(key, default(T));
        }
        /// <summary>
        /// Fetches a value from ViewState and returns the defaultValue provided if it does not exist
        /// </summary>
        /// <typeparam name="T">Type of the object desired to fetch</typeparam>
        /// <param name="key">Key to retrieve the object</param>
        /// <param name="defaultValue">The desired default value if ViewState does not contain the key</param>
        /// <returns>Value in ViewState and defaultValue if ViewState does not contain the key</returns>
        protected virtual T GetViewState<T>(string key, T defaultValue)
        {
            var value = ViewState[key];
            if (value == null)
                return defaultValue;
            return (T)value;
        }
    }
}
