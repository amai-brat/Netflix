using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace API.MetadataProviders;

public class CustomMetadataProvider : IDisplayMetadataProvider {
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context) {

        if (context.Key.MetadataKind == ModelMetadataKind.Property) {
    
            context.DisplayMetadata.ConvertEmptyStringToNull = false;
        }
    }
}