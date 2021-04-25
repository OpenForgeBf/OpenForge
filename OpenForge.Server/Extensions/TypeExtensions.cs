// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace OpenForge.Server.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetSerializableProperties(this Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<IgnoreDataMemberAttribute>() == null && p.CanRead && p.CanWrite).OrderBy(x => x.MetadataToken);
    }
}
