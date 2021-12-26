using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RetroLauncher.WebAPI.Controllers
{
    public class SelectiveSerializer : DefaultContractResolver
    {
        private readonly string[] _fields;

        public SelectiveSerializer(string fields)
        {
            var fieldColl = fields.Split(',');
            _fields = fieldColl
                .Select(f => f.ToLower().Trim())
                .ToArray();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = o => _fields.Contains(member.Name.ToLower());                       

            return property;
        }
    }
}
