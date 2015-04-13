using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Xml;

namespace ClassExpSys
{
    /// <summary>
    /// Expert system.
    /// </summary>
    public class ExpertSystem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets all available attributes.
        /// </summary>
        public List<ObjectAttribute> Attributes { get; private set; }

        /// <summary>
        /// Gets list of objects.
        /// </summary>
        public List<ExpertObject> AllObjects { get; private set; }

        /// <summary>
        /// Gets filtered list of objects which have specified by user attributes.
        /// </summary>
        public List<ExpertObject> FilteredObjects { get; private set; }

        /// <summary>
        /// Creates expert system with empty database.
        /// </summary>
        public ExpertSystem()
        {
            Attributes = new List<ObjectAttribute>();
            AllObjects = new List<ExpertObject>();
            FilteredObjects = new List<ExpertObject>();
        }

        /// <summary>
        /// Loads database from XML file.
        /// </summary>
        /// <param name="xmlFileName">XML file name.</param>
        public void LoadDatabase(string xmlFileName)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(xmlFileName);
            }
            catch (Exception)
            {
                throw new Exception("Can not open file.");
            }
            
            //Load attributes.
            XmlNodeList attrList = doc.SelectNodes("//Database//Attributes//A");
            if (attrList == null)
            {
                throw new XmlSyntaxException("There is no valid attributes list.");
            }
            foreach (XmlNode attr in attrList)
            {
                if (attr.Attributes == null)
                {
                    throw new XmlSyntaxException("Invalid attribute.");
                }

                XmlAttribute id = attr.Attributes["ID"];
                XmlAttribute caption = attr.Attributes["Caption"];
                if (id == null || caption == null)
                {
                    throw new XmlSyntaxException("Invalid attribute syntax.");
                }

                //Add attribute to dictionary.
                ObjectAttribute objAttr = new ObjectAttribute(int.Parse(id.Value), caption.Value);
                Attributes.Add(objAttr);
                objAttr.PropertyChanged += objectAttribute_PropertyChanged;
            }

            //Load objects list.
            XmlNodeList objList = doc.SelectNodes("//Database//Objects//O");
            if (objList == null)
            {
                throw new XmlSyntaxException("There is no valid objects list.");
            }
            foreach (XmlNode obj in objList)
            {
                if (obj.Attributes == null)
                {
                    throw new XmlSyntaxException("Invalid object.");
                }

                XmlAttribute name = obj.Attributes["Name"];
                XmlAttribute attrs = obj.Attributes["Attributes"];
                if (name == null || attrs == null)
                {
                    throw new XmlSyntaxException("Invalid object syntax.");
                }

                //Create object and add to the list.
                ExpertObject expObj = new ExpertObject(name.Value);
                expObj.SetAttributes(attrs.Value);
                AllObjects.Add(expObj);
            }

            FilteredObjects.AddRange(AllObjects);

            //Invoke echanging events.
            InvokePropertyChanged(new PropertyChangedEventArgs("Attributes"));
            InvokePropertyChanged(new PropertyChangedEventArgs("AllObjects"));
            InvokePropertyChanged(new PropertyChangedEventArgs("FilteredObjects"));
        }

        /// <summary>
        /// Reacts on changes in attributes properties.
        /// </summary>
        /// <param name="sender">Changed attribute.</param>
        /// <param name="e">Changing details.</param>
        private void objectAttribute_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Filter();
        }

        public void Filter()
        {
            //Collect only selected attributes.
            List<ObjectAttribute> yesAttr = Attributes.Where(x => x.Used && !x.Unused).ToList();
            List<ObjectAttribute> noAttr = Attributes.Where(x => !x.Used && x.Unused).ToList();

            FilteredObjects = AllObjects.FindAll(obj =>
                                                    {
                                                        var yes = yesAttr.FindAll(x => obj.Attributes.Contains(x.ID));
                                                        var no = noAttr.FindAll(x => obj.Attributes.Contains(x.ID));
                                                        return yes.Count == yesAttr.Count && no.Count == 0;
                                                    });

            InvokePropertyChanged(new PropertyChangedEventArgs("FilteredObjects"));
        }

        /// <summary>
        /// Expert system properties changing event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes expert system properties changing event.
        /// </summary>
        /// <param name="e">Contains changed property name.</param>
        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}
