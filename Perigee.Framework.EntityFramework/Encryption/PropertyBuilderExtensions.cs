namespace Perigee.Framework.EntityFramework.Encryption
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Perigee.Framework.Base.Encryption;

    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<TProperty> EnableEncryption<TProperty>(this PropertyBuilder<TProperty> builder, IEncryptionProvider encryptionProvider)
        {
            return builder.HasConversion(new EncryptionConverter(encryptionProvider));
        }

    }
}
