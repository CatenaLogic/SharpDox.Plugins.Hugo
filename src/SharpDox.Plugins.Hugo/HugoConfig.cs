// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HugoConfig.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using System;
    using System.ComponentModel;
    using Sdk.Config;
    using Sdk.Config.Attributes;

    [Name(typeof(HugoStrings), "Hugo")]
    public class HugoConfig : IConfigSection
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool? _ignorePrivateMembers;
        private string _rootPrefix;
        //private string _primaryColor;
        //private string _secondaryColor;

        [Name(typeof(HugoStrings), nameof(IgnorePrivateMembers))]
        public bool IgnorePrivateMembers
        {
            get { return _ignorePrivateMembers ?? true; }
            set
            {
                _ignorePrivateMembers = value;
                OnPropertyChanged(nameof(IgnorePrivateMembers));
            }
        }

        [Name(typeof(HugoStrings), nameof(RootPrefix))]
        public string RootPrefix
        {
            get { return _rootPrefix ?? string.Empty; }
            set
            {
                _rootPrefix = value;
                OnPropertyChanged(nameof(RootPrefix));
            }
        }

        //[Name(typeof(HugoStrings), nameof(PrimaryColor))]
        //[ConfigEditor(EditorType.Colorpicker)]
        //public string PrimaryColor
        //{
        //    get { return _primaryColor ?? "#F58026"; }
        //    set
        //    {
        //        _primaryColor = value;
        //        OnPropertyChanged(nameof(PrimaryColor));
        //    }
        //}

        //[Name(typeof(HugoStrings), nameof(SecondaryColor))]
        //[ConfigEditor(EditorType.Colorpicker)]
        //public string SecondaryColor
        //{
        //    get { return _secondaryColor ?? "#F58026"; }
        //    set
        //    {
        //        _secondaryColor = value;
        //        OnPropertyChanged(nameof(SecondaryColor));
        //    }
        //}

        public Guid Guid
        {
            get { return new Guid("4E2916FC-CBB3-4816-B9D3-D8334734D4B9"); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}