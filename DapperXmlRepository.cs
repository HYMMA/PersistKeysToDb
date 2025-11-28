using Dapper;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace PersistKeysToDb
{
    public class DapperXmlRepository : IXmlRepository
    {
        private string _conn;
        private ILogger<DapperXmlRepository> _logger;
        public DapperXmlRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _conn = connectionString;
            _logger = loggerFactory.CreateLogger<DapperXmlRepository>()
                        ?? throw new ArgumentNullException(nameof(ArgumentNullException));
        }
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var XmlElements = new List<XElement>();
            using (var sql = new SqlConnection())
            {
                //insert into ProductAndFeature (FeatureId, ProductId) values(@featId,@productId)
                sql.ConnectionString = _conn;
                sql.Open();
                var elements = sql.Query<string>(@$"select D.Xml from DataProtectionKey D");
                foreach (var element in elements)
                {
                    XmlElements.Add(XElement.Parse(element));
                }
            }
            return XmlElements;
        }
        public void StoreElement(XElement element, string friendlyName)
        {
            using (var sql = new SqlConnection())
            {
                //insert into ProductAndFeature (FeatureId, ProductId) values(@featId,@productId)
                sql.ConnectionString = _conn;
                sql.Open();
                var xml = element.ToString(SaveOptions.DisableFormatting);
                var query = @$"
                                    if (not exists (select * from DataProtectionKey D where D.FriendlyName='{friendlyName}'))
                                    BEGIN
                                        insert into DataProtectionKey (FriendlyName,[Xml]) values('{friendlyName}','{xml}')
                                    END";
                sql.Execute(query);
            }
        }
    }
}