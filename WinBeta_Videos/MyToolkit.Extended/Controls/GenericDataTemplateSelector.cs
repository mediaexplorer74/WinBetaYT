﻿//-----------------------------------------------------------------------
// <copyright file="TypeDataTemplate.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

#if WINRT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace MyToolkit.Controls
{
    [ContentProperty(Name = "Template")]
    public class TypeDataTemplate
    {
        public string Type { get; set; }
        public DataTemplate Template { get; set; }
    }

    [ContentProperty(Name = "Templates")]
    public class GenericDataTemplateSelector : DataTemplateSelector
    {
        public List<TypeDataTemplate> Templates { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public GenericDataTemplateSelector()
        {
            Templates = new List<TypeDataTemplate>();
        }

#if WINRT
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
#else
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
#endif
        {
            var tpl = Templates.SingleOrDefault(t => t.Type == item.GetType().Name);
            if (tpl != null)
                return tpl.Template;
            return DefaultTemplate;
        }
    }
}

#endif