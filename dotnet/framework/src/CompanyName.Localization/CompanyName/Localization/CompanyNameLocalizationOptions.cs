using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace CompanyName.Localization
{
    public class CompanyNameLocalizationOptions : LocalizationOptions
    {
        public Type DefaultResourceType { get; set; } = typeof(DefaultResource);
    }
}
