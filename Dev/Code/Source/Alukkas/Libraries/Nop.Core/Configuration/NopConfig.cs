using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Nop.Core.Configuration
{
    /// <summary>
    /// Represents a NopConfig
    /// </summary>
    public partial class NopConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new NopConfig();
            var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
            if (dynamicDiscoveryNode != null && dynamicDiscoveryNode.Attributes != null)
            {
                var attribute = dynamicDiscoveryNode.Attributes["Enabled"];
                if (attribute != null)
                    config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
            }

            var engineNode = section.SelectSingleNode("Engine");
            if (engineNode != null && engineNode.Attributes != null)
            {
                var attribute = engineNode.Attributes["Type"];
                if (attribute != null)
                    config.EngineType = attribute.Value;
            }

            var startupNode = section.SelectSingleNode("Startup");
            if (startupNode != null && startupNode.Attributes != null)
            {
                var attribute = startupNode.Attributes["IgnoreStartupTasks"];
                if (attribute != null)
                    config.IgnoreStartupTasks = Convert.ToBoolean(attribute.Value);
            }

            var themeNode = section.SelectSingleNode("Themes");
            if (themeNode != null && themeNode.Attributes != null)
            {
                var attribute = themeNode.Attributes["basePath"];
                if (attribute != null)
                    config.ThemeBasePath = attribute.Value;
            }

            var dependencyRegistrarNode = section.SelectSingleNode("DependencyRegistrars");
            var registrars = new List<Type>();
            if (dependencyRegistrarNode != null && dependencyRegistrarNode.ChildNodes.Count > 0)
            {
               
                foreach (var child in dependencyRegistrarNode.ChildNodes)
                {
                    var registrarElement = ((XmlElement)child);
                    if (registrarElement.Attributes!=null)
                    {
                        var nameAttribute = registrarElement.Attributes["Name"];
                        var assemblyAttribute = registrarElement.Attributes["Assembly"];
                        if (nameAttribute != null && assemblyAttribute != null)
                        {

                            var assembly = Assembly.Load(assemblyAttribute.Value);                          
                            registrars.Add(assembly.GetType(nameAttribute.Value));
                        }
                    }
                }
            }

            config.DependencyRegistrarTypes = new ReadOnlyCollection<Type>(registrars);

            var routeProviderNode = section.SelectSingleNode("RouteProviders");
            var routeProviders = new List<Type>();
            if (routeProviderNode != null && routeProviderNode.ChildNodes.Count > 0)
            {

                foreach (var child in routeProviderNode.ChildNodes)
                {
                    var providerElement = ((XmlElement)child);
                    if (providerElement.Attributes != null)
                    {
                        var nameAttribute = providerElement.Attributes["Name"];
                        var assemblyAttribute = providerElement.Attributes["Assembly"];
                        if (nameAttribute != null && assemblyAttribute != null)
                        {
                            
                            var assembly = Assembly.Load(assemblyAttribute.Value);
                           
                            routeProviders.Add(assembly.GetType(nameAttribute.Value));
                        }
                    }
                }
            }

            config.RouteProviderTypes = new ReadOnlyCollection<Type>(routeProviders);

            var probingNode = section.SelectSingleNode("ProbingRequired");
            config.ProbingRequired = new Dictionary<string, bool>();
            if (probingNode != null && probingNode.Attributes.Count>0)
            {
                var depedencyAttribute = probingNode.Attributes["DependencyRegistrar"];
                if (depedencyAttribute != null)
                {
                    config.ProbingRequired.Add("DependencyRegistrar", Convert.ToBoolean(depedencyAttribute.Value));
                }
            }

            return config;
        }
        
        /// <summary>
        /// In addition to configured assemblies examine and load assemblies in the bin directory.
        /// </summary>
        public bool DynamicDiscovery { get; private set; }

        /// <summary>
        /// A custom <see cref="IEngine"/> to manage the application instead of the default.
        /// </summary>
        public string EngineType { get; private set; }

        /// <summary>
        /// Specifices where the themes will be stored (~/Themes/)
        /// </summary>
        public string ThemeBasePath { get; private set; }

        /// <summary>
        /// Indicates whether we should ignore startup tasks
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }
        /// <summary>
        /// returns list of idependencyregistrars
        /// </summary>
        public ReadOnlyCollection<Type> DependencyRegistrarTypes { get; private set; }

        /// <summary>
        /// returns list of irouteproviders
        /// </summary>
        public ReadOnlyCollection<Type> RouteProviderTypes { get; private set; }

        /// <summary>
        /// returns list of probing settings
        /// </summary>
        public Dictionary<string, bool> ProbingRequired { get; private set; }
    }
}
