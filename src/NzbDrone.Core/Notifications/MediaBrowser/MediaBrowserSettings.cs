﻿using System;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;

namespace NzbDrone.Core.Notifications.MediaBrowser
{
    public class MediaBrowserSettingsValidator : AbstractValidator<MediaBrowserSettings>
    {
        public MediaBrowserSettingsValidator()
        {
            RuleFor(c => c.Host).NotEmpty();
            RuleFor(c => c.ApiKey).NotEmpty();
        }
    }

    public class MediaBrowserSettings : IProviderConfig
    {
        private static readonly MediaBrowserSettingsValidator Validator = new MediaBrowserSettingsValidator();

        public MediaBrowserSettings()
        {
            Port = 8096;
        }

        [FieldDefinition(0, Label = "Host")]
        public String Host { get; set; }

        [FieldDefinition(1, Label = "Port")]
        public Int32 Port { get; set; }

        [FieldDefinition(2, Label = "API Key")]
        public String ApiKey { get; set; }

        [FieldDefinition(3, Label = "Send Notifications", HelpText = "Have MediaBrowser send notfications to configured providers", Type = FieldType.Checkbox)]
        public Boolean Notify { get; set; }

        [FieldDefinition(4, Label = "Update Library", HelpText = "Update Library on Download & Rename?", Type = FieldType.Checkbox)]
        public Boolean UpdateLibrary { get; set; }

        [JsonIgnore]
        public String Address { get { return String.Format("{0}:{1}", Host, Port); } }
        
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Host) && Port > 0;
            }
        }

        public ValidationResult Validate()
        {
            return Validator.Validate(this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
