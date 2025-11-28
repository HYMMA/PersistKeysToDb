namespace PersistKeysToDb
{
    /// <summary>
    /// Code first model used by <see cref="DapperXmlRepository"/>.
    /// </summary>
    public class DapperDataProtectionKey
    {
        /// <summary>
        /// The entity identifier of the <see cref="DapperDataProtectionKey"/>.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The friendly name of the <see cref="DapperDataProtectionKey"/>.
        /// </summary>
        public string? FriendlyName { get; set; }

        /// <summary>
        /// The XML representation of the <see cref="DapperDataProtectionKey"/>.
        /// </summary>
        public string? Xml { get; set; }
    }

}
