using System.ComponentModel;

namespace ClassExpSys
{
    /// <summary>
    /// Represents object attribute.
    /// </summary>
    public class ObjectAttribute : INotifyPropertyChanged
    {
        /// <summary>
        /// Shows if current attribute is used.
        /// </summary>
        private bool used;

        /// <summary>
        /// Gets and sets if current attribute is used.
        /// </summary>
        public bool Used
        {
            get
            {
                return used;
            }
            set
            {
                used = value;
                if (used)
                {
                    Unused = false;
                }
                InvokePropertyChanged(new PropertyChangedEventArgs("Used"));
            }
        }

        /// <summary>
        /// Shows if current attribute is not used.
        /// </summary>
        private bool unused;

        /// <summary>
        /// Gets and sets if current attribute is not used.
        /// </summary>
        public bool Unused
        {
            get
            {
                return unused;
            }
            set
            {
                unused = value;
                if (unused)
                {
                    Used = false;
                }
                InvokePropertyChanged(new PropertyChangedEventArgs("Unused"));
            }
        }

        /// <summary>
        /// Gets attribute name for displaying.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets attribute ID.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Creates attribute with specified name.
        /// </summary>
        /// <param name="id">ID of attribute.</param>
        /// <param name="attrName">Name of attribute.</param>
        public ObjectAttribute(int id, string attrName)
        {
            ID = id;
            Name = attrName;
        }

        /// <summary>
        /// Atrribute fields changing event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes atrribute fields changing event.
        /// </summary>
        /// <param name="e">Contains changed fields name.</param>
        protected void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}
