using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using TikiTankCommon.Patterns;
using TikiTankCommon.Devices;

namespace TikiTankCommon
{

    public class DeviceList
    {
        public IEnumerable<IShowable> Items { get; set; }
    }

    public class DeviceConverter : CustomCreationConverter<IShowable>
    {
        public override IShowable Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IShowable Create(Type objectType, JObject jObject)
        {   
            if( jObject["ConsoleStrip"] != null )
                return new ConsoleStrip();
            if( jObject["SpiLedStrip"] != null )
                return new SpiLedStrip();
            if (jObject["DmxLedStrip"] != null)
                return new DmxLedStrip();

            throw new ApplicationException(String.Format("The device type {0} is not supported!", objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object defaultValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }

    public class PatternConverter : CustomCreationConverter<IPattern>
    {
        public override IPattern Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IPattern Create(Type objectType, JObject jObject)
        {
            switch (jObject["Class"].ToString())
            {
                case "TreadEffect":
                    return new TreadEffect();
                case "CameraFlashes":
                    return new CameraFlashes();
                case "Glow":
                    return new Glow();
           }

            throw new ApplicationException(String.Format("The device type {0} is not supported!", objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object defaultValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
