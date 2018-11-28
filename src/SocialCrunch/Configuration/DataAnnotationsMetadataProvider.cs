using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace SocialCrunch.Configuration
{
    public class DataAnnotationsMetadataProvider : IValidationMetadataProvider
    {
        /// <summary>
        ///     Returns the resource message key prefix for data annotations.
        /// </summary>
        private const string KeyPrefix = "DataAnnotations_";

        /// <summary>
        ///     Gets the type of the ressource message.
        /// </summary>
        public Type ErrorMessageResourceType => typeof(ResX);

        /// <inheritdoc />
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var attr in context.Attributes)
            {
                if (attr is ValidationAttribute validationAttr)
                {
                    validationAttr.ErrorMessageResourceName = KeyPrefix + attr.GetType().Name;
                    validationAttr.ErrorMessageResourceType = ErrorMessageResourceType;
                    break;
                }
            }
        }
    }
}
