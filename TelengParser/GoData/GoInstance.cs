using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelengParser.GoData
{
    public class GoInstance
    {
        private Dictionary<string, GoObject> _instanceTypes;
        private string _mainObjectKey;

        public GoInstance(Dictionary<string, GoObject> instanceTypes, string mainObject)
        {
            _instanceTypes = instanceTypes;
            _mainObjectKey = mainObject;
        }

        public string GetRandomMainObject()
        {
            dynamic mainObjectInstance = GetRandomInstanceObject(_mainObjectKey);

            return JsonConvert.SerializeObject(mainObjectInstance, Formatting.Indented);

        }

        public dynamic GetRandomInstanceObject(string objectKey)
        {
            ExpandoObject mainObjectInstance = new ExpandoObject();

            GoObject objectToInstance = _instanceTypes[objectKey];

            var mainObjectInstanceDict = mainObjectInstance as IDictionary<string, dynamic>;


            var propertyIterator = objectToInstance._properties.Keys.GetEnumerator();

            while (propertyIterator.MoveNext())
            {
                GoProperty currProp = objectToInstance._properties[propertyIterator.Current];

                dynamic propertyInstance = currProp.GenerateTrashValue(this);

                mainObjectInstanceDict.Add(propertyIterator.Current, propertyInstance);
            }

            return mainObjectInstanceDict;
        }
        
    }
}
