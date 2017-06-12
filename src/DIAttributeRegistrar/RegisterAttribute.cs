﻿using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; private set; }

        public Type AsType { get; private set; }

        public string[] Tags { get; private set; }

        public RegisterAttribute(Type asType = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params string[] tags)
        {
            this.ServiceLifetime = serviceLifetime;
            this.AsType = asType;
            this.Tags = tags == null ? new string[] { } : tags.Where(t => t != null).ToArray();
        }

        public RegisterAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params string[] tags) : this(null, serviceLifetime, tags)
        {

        }

        public RegisterAttribute(params string[] tags) : this(null, ServiceLifetime.Scoped, tags)
        {

        }

        public RegisterAttribute() : this(null)
        {

        }
    }
}