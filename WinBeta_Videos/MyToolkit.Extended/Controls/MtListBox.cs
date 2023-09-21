﻿//-----------------------------------------------------------------------
// <copyright file="MtListBox.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

#if !WPF

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using MyToolkit.UI;

#if WP7 || WP8 || SL5
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
#endif
#if WP7 || WP8
using Microsoft.Phone.Controls;
#endif
#if WINRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
#endif

// developed by Rico Suter (http://rsuter.com), http://mytoolkit.codeplex.com

namespace MyToolkit.Controls
{
    /// <summary>A <see cref="ListBox"/> with additional features. </summary>
    public class MtListBox : ListBox
    {
        private event EventHandler<ScrolledToEndEventArgs> _scrolledToEnd;
        private ScrollViewer _scrollViewer;
        private double _lastExtentHeight = 0;
        private bool _bindingCreated = false;
        private FrameworkElement _lastElement = null;
        private Thickness _lastElementMargin;
        private bool _allowIsScrollingChanges;
        private bool _eventRegistred = false;

        /// <summary>Initializes a new instance of the <see cref="MtListBox"/> class. </summary>
        public MtListBox()
        {
            LayoutUpdated += RegisterScrollEvent;
            Unloaded += OnUnloaded;

#if WINRT
            DefaultStyleKey = typeof(MtListBox);
#else
            ItemContainerStyle = (Style)XamlReader.Load(
                @"<Style TargetType=""ListBoxItem"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Setter Property=""Template"">
                        <Setter.Value>
                            <ControlTemplate>
                                <ContentPresenter HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch""/>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style>");
#endif
        }

        /// <summary>Gets the <see cref="ListBoxItem"/> for a given item. </summary>
        /// <param name="item">The item. </param>
        /// <returns>The <see cref="ListBoxItem"/>. </returns>
        public ListBoxItem GetListBoxItemFromItem(object item)
        {
            return (ListBoxItem)ItemContainerGenerator.ContainerFromItem(item);
        }

        /// <summary>Gets the view's <see cref="ScrollViewer"/>. </summary>
        public ScrollViewer ScrollViewer
        {
            get { return _scrollViewer; }
        }

        /// <summary>Scrolls to the given offset. </summary>
        /// <param name="offset">The offset. </param>
        /// <returns>Returns false if the <see cref="ScrollViewer"/> was not loaded. </returns>
        public bool ScrollToVerticalOffset(double offset)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.InvalidateScrollInfo();
                _scrollViewer.ScrollToVerticalOffset(offset);
                return true;
            }
            return false;
        }

        /// <summary>Stops the current scrolling. </summary>
        /// <returns>Returns false if the <see cref="ScrollViewer"/> was not loaded. </returns>
        public bool StopScrolling()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.InvalidateScrollInfo();
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset);
                return true;
            }
            return false;
        }

#if WINRT
        protected override void OnApplyTemplate()
#else
        public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();
            _scrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");

            UpdateInnerMargin();
            RegisterVerticalOffsetChangedHandler();
        }

#region Scroll jumping fix

#if WP8 || WP7

        public static readonly DependencyProperty UseScrollFixProperty =
            DependencyProperty.Register("UseScrollFix", typeof(bool), typeof(MtListBox), new PropertyMetadata(true));

        /// <summary>Gets or sets a value indicating whether the scroll fix sould be applied. </summary>
        public bool UseScrollFix
        {
            get { return (bool) GetValue(UseScrollFixProperty); }
            set { SetValue(UseScrollFixProperty, value); }
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            if (UseScrollFix)
            {
                var page = (PhoneApplicationPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                page.Focus();
            }

            base.OnManipulationCompleted(e);
        }

#endif

#endregion

#region Scroll to end

        /// <summary>Gets or sets a value indicating whether scrolled to end events should be triggered. </summary>
        public bool TriggerScrolledToEndEvents { get; set; }

        /// <summary>Occurs when the user scrolled to the end of the view. </summary>
        public event EventHandler<ScrolledToEndEventArgs> ScrolledToEnd
        {
            add
            {
                _scrolledToEnd += value;
                RegisterVerticalOffsetChangedHandler();
            }
            remove { _scrolledToEnd -= value; }
        }

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (MtListBox)d;
            if (!ctrl.TriggerScrolledToEndEvents || ctrl._scrolledToEnd == null)
                return;

            var viewer = ctrl._scrollViewer;
            if (viewer != null)
            {
                var triggered = ctrl._lastExtentHeight == viewer.ExtentHeight;
                if (!triggered && viewer.VerticalOffset >= viewer.ScrollableHeight - viewer.ViewportHeight - viewer.ViewportHeight / 2)
                {
                    ctrl._lastExtentHeight = viewer.ExtentHeight;
                    var handler = ctrl._scrolledToEnd;
                    if (handler != null)
                        handler(ctrl, new ScrolledToEndEventArgs(viewer));
                }
            }
        }

        private static readonly DependencyProperty InternalOffsetProperty = DependencyProperty.Register(
            "InternalOffset", typeof(double), typeof(MtListBox),
            new PropertyMetadata(default(double), OnVerticalOffsetChanged));

        private void RegisterVerticalOffsetChangedHandler()
        {
            if (_scrollViewer == null || _bindingCreated || _scrolledToEnd == null)
                return;

            TriggerScrolledToEndEvents = true;

            var binding = new Binding();
            binding.Source = _scrollViewer;
            binding.Path = new PropertyPath("VerticalOffset");
            binding.Mode = BindingMode.OneWay;
            SetBinding(InternalOffsetProperty, binding);

            _bindingCreated = true;
        }

#endregion

#region Inner margin

        /// <summary>Gets or sets the inner margin. </summary>
        public Thickness InnerMargin
        {
            get { return (Thickness)GetValue(InnerMarginProperty); }
            set { SetValue(InnerMarginProperty, value); }
        }

        public static readonly DependencyProperty InnerMarginProperty =
            DependencyProperty.Register("InnerMargin", typeof(Thickness),
                typeof(MtListBox), new PropertyMetadata(new Thickness(), InnerMarginChanged));

        private static void InnerMarginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((MtListBox)obj).UpdateInnerMargin();
        }

#if WINRT

        private void UpdateInnerMargin()
        {
            if (_scrollViewer != null)
            {
                var itemsPresenter = (ItemsPresenter)_scrollViewer.Content;
                if (itemsPresenter != null)
                    itemsPresenter.Margin = InnerMargin;
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            OnPrepareContainerForItem(new PrepareContainerForItemEventArgs(element, item));
        }

#elif WP7 || WP8 || SL5

        private void UpdateInnerMargin()
        {
            AddLastItemMargin();

            if (_scrollViewer != null)
            {
                var itemsPresenter = (ItemsPresenter)_scrollViewer.Content;
                if (itemsPresenter != null)
                    itemsPresenter.Margin = InnerMargin;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (InnerMargin.Top > 0.0 || InnerMargin.Bottom > 0.0)
            {
                if (Items != null && Items.Count > 0)
                {
                    var currentLastElement = ItemContainerGenerator.ContainerFromItem(Items.Last());
                    if (currentLastElement != null && currentLastElement != _lastElement)
                    {
                        if (_lastElement != null && ItemContainerGenerator.ItemFromContainer(_lastElement) != null)
                            RestoreLastItemMargin();

                        _lastElement = (FrameworkElement)currentLastElement;
                        _lastElementMargin = _lastElement.Margin;

                        AddLastItemMargin();
                    }
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            OnPrepareContainerForItem(new PrepareContainerForItemEventArgs(element, item));

            if (Items != null && (InnerMargin.Top > 0.0 || InnerMargin.Bottom > 0.0))
            {
                if (Items.IndexOf(item) == Items.Count - 1) // is last element of list
                {
                    if (_lastElement != element) // check that margin is not already set
                    {
                        RestoreLastItemMargin();

                        _lastElement = (FrameworkElement)element;
                        _lastElementMargin = _lastElement.Margin;

                        AddLastItemMargin();
                    }
                }
                else if (_lastElement == element) // if last element is not last anymore => recycled 
                    RestoreLastItemMargin();
                else if (_lastElement != null && Items.IndexOf(_lastElement) != Items.Count - 1) // last element is not last element anymore => items added
                    RestoreLastItemMargin();
            }
        }

        private void AddLastItemMargin()
        {
            if (_lastElement != null)
            {
                _lastElement.Margin = new Thickness(
                    _lastElementMargin.Left,
                    _lastElementMargin.Top,
                    _lastElementMargin.Right,
                    _lastElementMargin.Bottom + InnerMargin.Top + InnerMargin.Bottom);
            }
        }

        private void RestoreLastItemMargin()
        {
            if (_lastElement != null)
            {
                _lastElement.Margin = _lastElementMargin;
                _lastElement = null;
            }
        }

#endif

#endregion

#region Prepare container for item event

        /// <summary>Occurs when a new container control gets created. </summary>
        public event EventHandler<PrepareContainerForItemEventArgs> PrepareContainerForItem;

        private void OnPrepareContainerForItem(PrepareContainerForItemEventArgs args)
        {
            var copy = PrepareContainerForItem;
            if (copy != null)
                copy(this, args);
        }

#endregion

#region Scrolling

        /// <summary>Occurs when the scrolling state changed. </summary>
        public event EventHandler<ScrollingStateChangedEventArgs> ScrollingStateChanged;

        public static readonly DependencyProperty IsScrollingProperty =
            DependencyProperty.Register("IsScrolling", typeof(bool),
            typeof(MtListBox), new PropertyMetadata(false, (o, args) => ((MtListBox)o).IsScrollingPropertyChanged(args)));

        /// <summary>Gets a value indicating whether the user is currently scrolling the view. </summary>
        public bool IsScrolling
        {
            get { return (bool)GetValue(IsScrollingProperty); }
            internal set
            {
                // "Unlock" the ability to set the property
                _allowIsScrollingChanges = true;
                SetValue(IsScrollingProperty, value);
                _allowIsScrollingChanges = false;
            }
        }

        private bool _isScrolling = false;

        protected virtual void OnScrollingStateChanged(ScrollingStateChangedEventArgs e)
        {
            // Must be empty
        }

        /// <exception cref="InvalidOperationException">The IsScrolling property is read-only. </exception>
        internal void IsScrollingPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_allowIsScrollingChanges != true)
                throw new InvalidOperationException("The IsScrolling property is read-only. ");

            _isScrolling = (bool)e.NewValue;

            var args = new ScrollingStateChangedEventArgs((bool)e.OldValue, (bool)e.NewValue);
            OnScrollingStateChanged(args);

            var handler = ScrollingStateChanged;
            if (handler != null)
                handler(this, args);
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_isScrolling)
            {
                var handler = ScrollingStateChanged;
                if (handler != null)
                    handler(this, new ScrollingStateChangedEventArgs(true, false));
            }
        }

        private void ScrollingStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            IsScrolling = e.NewState.Name == "Scrolling";
        }

#if WINRT
        private void RegisterScrollEvent(object s, object o)
#else
        private void RegisterScrollEvent(object s, EventArgs eventArgs)
#endif
        {
            if (_eventRegistred)
                return;

            if (_scrollViewer != null)
            {
                var child = _scrollViewer.GetVisualChild(0);
                var group = child.GetVisualStateGroup("ScrollStates"); // TODO: Fix this => group is null in WinRT
                if (group != null)
                {
                    group.CurrentStateChanging -= ScrollingStateChanging;
                    group.CurrentStateChanging += ScrollingStateChanging;
                    _eventRegistred = true;
                }
            }
        }

#endregion
    }

    /// <summary>A <see cref="ListBox"/> with additional features. </summary>
    [Obsolete("Use MtListBox instead. 8/31/2014")]
    public class ExtendedListBox : MtListBox
    {
    }

    /// <summary>Contains information for the scrolling state changed event. </summary>
    public class ScrollingStateChangedEventArgs : EventArgs
    {
        /// <summary>Gets the old scrolling state. </summary>
        public bool OldValue { get; private set; }

        /// <summary>Gets the new scrolling state. </summary>
        public bool NewValue { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="ScrollingStateChangedEventArgs"/> class. </summary>
        public ScrollingStateChangedEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>Contains information for the scrolled to end event. </summary>
    public class ScrolledToEndEventArgs : EventArgs
    {
        /// <summary>Gets the involved scroll viewer. </summary>
        public ScrollViewer ScrollViewer { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ScrolledToEndEventArgs"/> class. </summary>
        public ScrolledToEndEventArgs(ScrollViewer viewer)
        {
            ScrollViewer = viewer;
        }
    }
}

#endif